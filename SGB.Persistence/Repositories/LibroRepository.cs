using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
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
            _logger = loggerFactory.CreateLogger<LibroRepository>();
            _configuration = configuration;
        }

        #region "Métodos Optimizados Corregidos"

        public async Task<Libro> ObtenerParaActualizacionAsync(string isbn)
        {
            return await Entity.FirstOrDefaultAsync(l => l.ISBN == isbn);
        }

        public async Task<OperationResult> ObtenerDetallesDTOPorIsbnAsync(string isbn)
        {
            try
            {
                var libroDto = await (from libro in Entity
                                      join categoria in _context.Categoria on libro.IDCategoria equals categoria.Id
                                      where libro.ISBN == isbn
                                      select new LibroDto 
                                      {
                                          ISBN = libro.ISBN,
                                          Titulo = libro.Titulo,
                                          Autor = libro.Autor,
                                          Editorial = libro.Editorial,
                                          FechaPublicacion = libro.FechaPublicacion,
                                          NombreCategoria = categoria.Nombre,
                                          Estado = libro.EstaActivo ? "Disponible" : "Inactivo",
                                          FechaRegistro = libro.FechaRegistro
                                      })
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync();

                return new OperationResult { Data = libroDto };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:GetById"];
                _logger.LogError(ex, "{ErrorMessage} para el ISBN: {ISBN}", errorMessage, isbn);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

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
                var errorMessage = _configuration["ErrorMessages:Libros:Delete"];
                _logger.LogError(ex, "{ErrorMessage} para el ISBN: {ISBN}", errorMessage, isbn);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error al eliminar el libro." };
            }
        }

        public async Task<OperationResult> BuscarPorAutorAsync(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El autor no puede estar vacío." });
            }

            return await base.FindByConditionAsync(l => l.Autor.Contains(autor) && l.EstaActivo);
        }

        public async Task<OperationResult> BuscarPorTituloAsync(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El título no puede estar vacío." });
            }

            return await base.FindByConditionAsync(l => l.Titulo.Contains(titulo) && l.EstaActivo);
        }

        public async Task<OperationResult> BuscarPorEditorialAsync(string editorial)
        {
            if (string.IsNullOrWhiteSpace(editorial))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "La editorial no puede estar vacía." });
            }

            return await base.FindByConditionAsync(l => l.Editorial.Contains(editorial) && l.EstaActivo);
        }

        public async Task<OperationResult> BuscarPorIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El ISBN no puede estar vacío." });
            }

            return await base.FindByConditionAsync(l => l.ISBN == isbn && l.EstaActivo);
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
                var errorMessage = _configuration["ErrorMessages:Libros:BuscarPorCategoria"];
                _logger.LogError(ex, "{ErrorMessage} para la categoría: {Categoria}", errorMessage, nombreCategoria);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error inesperado al buscar por categoría." };
            }
        }

        #endregion
    }
}