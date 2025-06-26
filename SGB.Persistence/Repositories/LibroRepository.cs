using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class LibroRepository : BaseRepository<Libro>, ILibroRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<LibroRepository> _logger;
        private readonly IConfiguration _configuration;

        public LibroRepository(SGBContext context,
                               ILoggerFactory loggerFactory,
                               IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<LibroRepository>();
        }

        #region "Métodos Heredados Sobrescritos"
        public override Task<Libro> GetByIdAsync(int id)
        {
            _logger.LogError("Se intentó usar GetByIdAsync(int) en LibroRepository, que usa ISBN (string) como clave.");
            throw new NotSupportedException("La entidad Libro utiliza un ISBN (string) como clave primaria. Utilice el método BuscarPorIsbnAsync.");
        }

        public override Task<OperationResult> DeleteAsync(int id)
        {
            _logger.LogError("Se intentó usar DeleteAsync(int) en LibroRepository, que usa ISBN (string) como clave.");
            throw new NotSupportedException("La entidad Libro utiliza un ISBN (string) como clave primaria. Utilice el método DeleteLogicoAsync.");
        }
        #endregion

        #region "Métodos Propios de ILibroRepository"

        public async Task<OperationResult> DeleteLogicoAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El ISBN no puede estar vacío." });
            }

            try
            {
                var libroParaEliminar = await Entity.FirstOrDefaultAsync(l => l.ISBN == isbn);

                if (libroParaEliminar == null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = "Libro no encontrado." });
                }

                libroParaEliminar.Deshabilitar();
                return await base.UpdateAsync(libroParaEliminar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la eliminación lógica del libro con ISBN: {ISBN}", isbn);
                var errorMessage = _configuration["ErrorMessages:Libros:Delete"];
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error al eliminar el libro." };
            }
        }

        public async Task<OperationResult> BuscarPorAutorAsync(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El autor no puede estar vacío." });
            }
            try
            {
                var data = await Entity.Where(l => l.Autor.Contains(autor) && l.EstaActivo).AsNoTracking().ToListAsync();
                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:GetAll"]; 
                _logger.LogError(ex, errorMessage, autor);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error inesperado al buscar por autor." };
            }
        }

        public async Task<OperationResult> BuscarPorTituloAsync(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El título no puede estar vacío." });
            }

            try
            {
                var data = await Entity.Where(l => l.Titulo.Contains(titulo) && l.EstaActivo).AsNoTracking().ToListAsync();
                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:LibroRepository:BuscarPorTitulo"];
                _logger.LogError(ex, errorMessage, titulo);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error inesperado al buscar por título." };
            }
        }

        public async Task<OperationResult> BuscarPorEditorialAsync(string editorial)
        {
            if (string.IsNullOrWhiteSpace(editorial))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "La editorial no puede estar vacía." });
            }

            try
            {
                var data = await Entity.Where(l => l.Editorial.Contains(editorial) && l.EstaActivo).AsNoTracking().ToListAsync();
                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:LibroRepository:BuscarPorEditorial"];
                _logger.LogError(ex, errorMessage, editorial);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error inesperado al buscar por editorial." };
            }
        }

        public async Task<OperationResult> BuscarPorIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El ISBN no puede estar vacío." });
            }

            try
            {
                var data = await Entity.AsNoTracking().FirstOrDefaultAsync(l => l.ISBN == isbn && l.EstaActivo);
                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:LibroRepository:BuscarPorIsbn"];
                _logger.LogError(ex, errorMessage, isbn);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error inesperado al buscar por ISBN." };
            }
        }

        public async Task<OperationResult> BuscarPorCategoriaAsync(string nombreCategoria)
        {
            if (string.IsNullOrWhiteSpace(nombreCategoria))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El nombre de la categoría no puede estar vacío." });
            }

            try
            {
                var data = await (from libro in Entity
                                  join categoria in _context.Categoria on libro.IDCategoria equals categoria.Id
                                  where categoria.Nombre == nombreCategoria && libro.EstaActivo
                                  select libro)
                                  .AsNoTracking()
                                  .ToListAsync();
                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:LibroRepository:BuscarPorCategoria"];
                _logger.LogError(ex, errorMessage, nombreCategoria);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error inesperado al buscar por categoría." };
            }
        }

        #endregion
    }
}