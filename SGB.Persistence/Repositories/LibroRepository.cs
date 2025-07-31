using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Api.Contracts.Repository.Interfaces;
using SGB.Api.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
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
            _logger = loggerFactory.CreateLogger<LibroRepository>();
            _configuration = configuration;
        }

        #region "Overridden Base Methods"

        public override async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var libroParaEliminar = await Entity.FindAsync(id);

                if (libroParaEliminar == null)
                {
                    return OperationResult<bool>.Failure("Libro no encontrado.");
                }

                libroParaEliminar.Deshabilitar();
                var updateResult = await base.UpdateAsync(libroParaEliminar);

                return OperationResult<bool>.Success(updateResult.IsSuccess, updateResult.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:Delete"];
                _logger.LogError(ex, "{ErrorMessage} para el Libro con ID: {LibroID}", errorMessage, id);
                return OperationResult<bool>.Failure(errorMessage ?? "Ocurrió un error al eliminar el libro.");
            }
        }

        #endregion

        #region "ILibroRepository Implementation"

        public async Task<OperationResult<IEnumerable<Libro>>> BuscarPorAutorAsync(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
                return OperationResult<IEnumerable<Libro>>.Failure("El autor no puede estar vacío.");

            return await base.FindByConditionAsync(l => l.Autor.Contains(autor) && l.EstaActivo);
        }

        public async Task<OperationResult<IEnumerable<Libro>>> BuscarPorTituloAsync(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return OperationResult<IEnumerable<Libro>>.Failure("El título no puede estar vacío.");

            return await base.FindByConditionAsync(l => l.Titulo.Contains(titulo) && l.EstaActivo);
        }

        public async Task<OperationResult<Libro>> BuscarPorIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return OperationResult<Libro>.Failure("El ISBN no puede estar vacío.");

            try
            {
                var libro = await Entity.AsNoTracking().FirstOrDefaultAsync(l => l.ISBN == isbn && l.EstaActivo);
                return OperationResult<Libro>.Success(libro);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:GetById"];
                _logger.LogError(ex, "{ErrorMessage} para el ISBN: {ISBN}", errorMessage, isbn);
                return OperationResult<Libro>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<LibroDto>> ObtenerDetallesDTOPorIdAsync(int id)
        {
            try
            {
                var libroDto = await (from libro in Entity
                                      join categoria in _context.Categorias on libro.IDCategoria equals categoria.Id
                                      where libro.Id == id
                                      select new LibroDto
                                      {
                                          Id = libro.Id,
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

                return OperationResult<LibroDto>.Success(libroDto);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:GetById"];
                _logger.LogError(ex, "{ErrorMessage} para el ID: {LibroID}", errorMessage, id);
                return OperationResult<LibroDto>.Failure(errorMessage);
            }
        }

        public async Task<Libro> ObtenerParaActualizacionAsync(int id)
        {
            return await Entity.FindAsync(id);
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> ObtenerTodosConDetallesAsync()
        {
            try
            {
                var listaDto = await (from libro in Entity
                                      join categoria in _context.Categorias on libro.IDCategoria equals categoria.Id
                                      where libro.EstaActivo
                                      select new LibroDto
                                      {
                                          Id = libro.Id,
                                          ISBN = libro.ISBN,
                                          Titulo = libro.Titulo,
                                          Autor = libro.Autor,
                                          Editorial = libro.Editorial,
                                          FechaPublicacion = libro.FechaPublicacion,
                                          NombreCategoria = categoria.Nombre,
                                          Estado = "Disponible",
                                          FechaRegistro = libro.FechaRegistro
                                      })
                                      .AsNoTracking()
                                      .ToListAsync();

                return OperationResult<IEnumerable<LibroDto>>.Success(listaDto);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:GetAll"];
                _logger.LogError(ex, errorMessage);
                return OperationResult<IEnumerable<LibroDto>>.Failure(errorMessage);
            }
        }

        #endregion
    }
}