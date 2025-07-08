using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Validators.BusinessValidators; 
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Libro;
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
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILibroBusinessValidator _libroValidator;
        private readonly ILogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        public LibroService(
            ILibroRepository libroRepository,
            ICategoriaRepository categoriaRepository,
            ILibroBusinessValidator libroValidator,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _libroRepository = libroRepository;
            _categoriaRepository = categoriaRepository;
            _libroValidator = libroValidator;
            _logger = loggerFactory.CreateLogger<LibroService>();
            _configuration = configuration;
        }

        #region Implementación de IBaseService

        public async Task<OperationResult> AddAsync(AddLibroDto dto)
        {
            try
            {
                var validationResult = await _libroValidator.ValidateForAddAsync(dto);
                if (!validationResult.Success)
                {
                    return validationResult; 
                }

                var categoriaValidada = (Categoria)validationResult.Data;
                var libroEntidad = new Libro(dto.ISBN, dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.AddAsync(libroEntidad);
                if (!repoResult.Success) return repoResult;

                var libroCreadoDto = new LibroDto
                {
                    Id = libroEntidad.Id,
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoriaValidada.Nombre,
                    Estado = "Disponible",
                    FechaRegistro = libroEntidad.FechaRegistro
                };

                return new OperationResult { Success = true, Data = libroCreadoDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al agregar libro con ISBN: {ISBN}", dto?.ISBN);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> UpdateAsync(int id, UpdateLibroDto dto)
        {
            try
            {
                var validationResult = await _libroValidator.ValidateForUpdateAsync(id, dto);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                var libroEntidad = (Libro)validationResult.Data;

                var categoriaValidada = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);

                libroEntidad.ActualizarDetalles(dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.UpdateAsync(libroEntidad);
                if (!repoResult.Success) return repoResult;

                var libroActualizadoDto = new LibroDto
                {
                    Id = libroEntidad.Id,
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoriaValidada?.Nombre ?? "Desconocida", 
                    Estado = libroEntidad.EstaActivo ? "Disponible" : "Inactivo",
                    FechaRegistro = libroEntidad.FechaRegistro
                };

                return new OperationResult { Success = true, Data = libroActualizadoDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al actualizar libro con ID: {ID}", id);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var validationResult = await _libroValidator.ValidateForDeleteAsync(id);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                return await _libroRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al eliminar libro con ID: {ID}", id);
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
                var resultadoRepo = await _libroRepository.BuscarPorIsbnAsync(isbn);
                if (!resultadoRepo.Success)
                {
                    return resultadoRepo;
                }

                var librosEncontrados = (IEnumerable<Libro>)resultadoRepo.Data;
                var libroEntidad = librosEncontrados.FirstOrDefault();

                if (libroEntidad == null)
                {
                    return new OperationResult { Success = true, Data = null };
                }

                var categoria = await _categoriaRepository.GetByIdAsync(libroEntidad.IDCategoria);
                var libroDto = new LibroDto
                {
                    Id = libroEntidad.Id,
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

                var listaDto = new List<LibroDto>();

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