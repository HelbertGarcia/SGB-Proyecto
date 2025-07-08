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

        #region "Métodos Heredados Sobrescritos"

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var libroParaEliminar = await Entity.FindAsync(id);
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
                _logger.LogError(ex, "{ErrorMessage} para el Libro con ID: {LibroID}", errorMessage, id);
                return new OperationResult { Success = false, Message = errorMessage ?? "Ocurrió un error al eliminar el libro." };
            }
        }

        #endregion

        #region "Implementación de ILibroRepository"

        public async Task<OperationResult> BuscarPorAutorAsync(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
                return await Task.FromResult(new OperationResult { Success = false, Message = "El autor no puede estar vacío." });
            return await base.FindByConditionAsync(l => l.Autor.Contains(autor) && l.EstaActivo);
        }

        public async Task<OperationResult> BuscarPorTituloAsync(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return await Task.FromResult(new OperationResult { Success = false, Message = "El título no puede estar vacío." });
            return await base.FindByConditionAsync(l => l.Titulo.Contains(titulo) && l.EstaActivo);
        }

        public async Task<OperationResult> BuscarPorIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return await Task.FromResult(new OperationResult { Success = false, Message = "El ISBN no puede estar vacío." });
            return await base.FindByConditionAsync(l => l.ISBN == isbn && l.EstaActivo);
        }

        public async Task<OperationResult> ObtenerDetallesDTOPorIdAsync(int id)
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
                return new OperationResult { Data = libroDto };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:GetById"];
                _logger.LogError(ex, "{ErrorMessage} para el ID: {LibroID}", errorMessage, id);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        // --- IMPLEMENTACIÓN DE MÉTODOS FALTANTES ---

        /// <summary>
        /// Obtiene una entidad Libro con seguimiento para ser actualizada.
        /// </summary>
        public async Task<Libro> ObtenerParaActualizacionAsync(int id)
        {
            // No se usa AsNoTracking() porque necesitamos que EF Core rastree la entidad.
            return await Entity.FindAsync(id);
        }

        /// <summary>
        /// Obtiene una lista de todos los libros activos, proyectada directamente a DTOs.
        /// </summary>
        public async Task<OperationResult> ObtenerTodosConDetallesAsync()
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

                return new OperationResult { Data = listaDto };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Libros:GetAll"];
                _logger.LogError(ex, errorMessage);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        #endregion
    }
}