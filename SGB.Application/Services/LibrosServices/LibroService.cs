using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGB.Application.Services.LibrosServices
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        public LibroService(
            ILibroRepository libroRepository,
            IPrestamoRepository prestamoRepository,
            ICategoriaRepository categoriaRepository,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _libroRepository = libroRepository;
            _prestamoRepository = prestamoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = loggerFactory.CreateLogger<LibroService>();
            _configuration = configuration;
        }

        #region Implementación de IBaseService

        public async Task<OperationResult> AddAsync(AddLibroDto dto)
        {
            try
            {
                if (dto is null)
                    return await Task.FromResult(new OperationResult { Success = false, Message = "Datos nulos." });

                var resultadoExistencia = await _libroRepository.BuscarPorIsbnAsync(dto.ISBN);
                if (resultadoExistencia.Success && resultadoExistencia.Data is IEnumerable<Libro> lista && lista.Any())
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Libros:IsbnAlreadyExists"] });

                var categoriaResult = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
                if (categoriaResult == null)
                    return await Task.FromResult(new OperationResult { Success = false, Message = "La categoría especificada no existe." });

                var libroEntidad = new Libro(dto.ISBN, dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var resultadoRepo = await _libroRepository.AddAsync(libroEntidad);
                if (!resultadoRepo.Success) return resultadoRepo;

                var libroCreadoDto = new LibroDto
                {
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoriaResult.Nombre,
                    Estado = "Disponible",
                    FechaRegistro = libroEntidad.FechaRegistro
                };
                return new OperationResult { Success = true, Data = libroCreadoDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el libro con ISBN: {ISBN}", dto?.ISBN);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> UpdateAsync(int id, UpdateLibroDto dto)
        {
            try
            {
                var libroEntidad = await _libroRepository.ObtenerParaActualizacionAsync(id);
                if (libroEntidad == null)
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] });

                var categoriaResult = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
                if (categoriaResult == null)
                    return await Task.FromResult(new OperationResult { Success = false, Message = "La nueva categoría especificada no existe." });

                libroEntidad.ActualizarDetalles(dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                return await _libroRepository.UpdateAsync(libroEntidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el libro con ID: {ID}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var prestamosActivos = await _prestamoRepository.FindByConditionAsync(p => p.Id == id && p.Estado == EstadoPrestamo.Activo);
                if (prestamosActivos.Success && prestamosActivos.Data is IEnumerable<Prestamo> lista && lista.Any())
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Libros:BookIsOnLoan"] });
                }

                return await _libroRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al eliminar el libro con ID: {ID}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            return await _libroRepository.ObtenerDetallesDTOPorIdAsync(id);
        }

        public async Task<OperationResult> GetAllAsync()
        {
            return await _libroRepository.ObtenerTodosConDetallesAsync();
        }

        #endregion

        #region Implementación de ILibroService (Métodos Específicos)

        public async Task<OperationResult> BuscarPorIsbnAsync(string isbn)
        {
            try
            {
                // 1. Llama al método correcto del repositorio.
                var resultadoRepo = await _libroRepository.BuscarPorIsbnAsync(isbn);
                if (!resultadoRepo.Success) return resultadoRepo;

                // 2. Extrae la lista de entidades (aunque solo debería haber una).
                var librosEncontrados = (IEnumerable<Libro>)resultadoRepo.Data;
                var libroEntidad = librosEncontrados.FirstOrDefault();

                // 3. Comprueba si se encontró un libro.
                if (libroEntidad == null)
                {
                    // Es una operación exitosa, pero no se encontró nada.
                    return new OperationResult { Success = true, Data = null };
                }

                // 4. Mapea la entidad a un DTO detallado.
                var categoria = await _categoriaRepository.GetByIdAsync(libroEntidad.IDCategoria);
                var libroDto = new LibroDto
                {
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoria?.Nombre ?? "Desconocida",
                    Estado = libroEntidad.EstaActivo ? "Disponible" : "Inactivo",
                    FechaRegistro = libroEntidad.FechaRegistro
                };

                return new OperationResult { Success = true, Data = libroDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al buscar libro por ISBN: {ISBN}", isbn);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> BuscarLibrosAsync(string terminoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
                return await GetAllAsync();

            try
            {
                Expression<Func<Libro, bool>> filtro = l =>
                    (l.Titulo.Contains(terminoBusqueda) || l.Autor.Contains(terminoBusqueda))
                    && l.EstaActivo;

                var resultadoRepo = await _libroRepository.FindByConditionAsync(filtro);
                if (!resultadoRepo.Success) return resultadoRepo;

                var librosEncontrados = (IEnumerable<Libro>)resultadoRepo.Data;

                // Mapear los resultados a una lista de DTOs detallados.
                var listaDto = new List<LibroDto>();
                foreach (var libro in librosEncontrados)
                {
                    var categoria = await _categoriaRepository.GetByIdAsync(libro.IDCategoria);
                    listaDto.Add(new LibroDto
                    {
                        ISBN = libro.ISBN,
                        Titulo = libro.Titulo,
                        Autor = libro.Autor,
                        Editorial = libro.Editorial,
                        FechaPublicacion = libro.FechaPublicacion,
                        NombreCategoria = categoria?.Nombre ?? "Desconocida",
                        Estado = libro.EstaActivo ? "Disponible" : "Inactivo",
                        FechaRegistro = libro.FechaRegistro
                    });
                }

                return new OperationResult { Success = true, Data = listaDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar libros con el término: {Termino}", terminoBusqueda);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        #endregion
    }
}