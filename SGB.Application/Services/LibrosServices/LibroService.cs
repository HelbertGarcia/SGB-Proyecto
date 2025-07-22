using Microsoft.Extensions.Configuration;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Validators.BusinessValidators;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Application.Services.LibrosServices
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly ILibroBusinessValidator _libroValidator;
        private readonly IAppLogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        public LibroService(
            ILibroRepository libroRepository,
            ILibroBusinessValidator libroValidator,
            IAppLogger<LibroService> logger,
            IConfiguration configuration)
        {
            _libroRepository = libroRepository;
            _libroValidator = libroValidator;
            _logger = logger;
            _configuration = configuration;
        }

        #region Implementación de IBaseService

        public async Task<OperationResult<LibroDto>> AddAsync(AddLibroDto dto)
        {
            _logger.Info("Iniciando proceso para agregar libro con ISBN {0}", dto?.ISBN);
            try
            {
                var validationResult = await _libroValidator.ValidateForAddAsync(dto);
                if (!validationResult.IsSuccess)
                {
                    _logger.Error("Validación de negocio falló al agregar libro: {0}", validationResult.Message);
                    return OperationResult<LibroDto>.Failure(validationResult.Message);
                }

                var categoriaValidada = validationResult.Data;
                var libroEntidad = new Libro(dto.ISBN, dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.AddAsync(libroEntidad);
                if (!repoResult.IsSuccess)
                {
                    _logger.Error("El repositorio falló al agregar el libro: {0}", repoResult.Message);
                    return OperationResult<LibroDto>.Failure(repoResult.Message);
                }

                var libroCreado = repoResult.Data;
                var libroCreadoDto = new LibroDto { /* ... mapeo ... */ };

                _logger.Info("Libro registrado exitosamente con ID {0}", libroCreado.Id);
                return OperationResult<LibroDto>.Success(libroCreadoDto, "Libro creado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en el servicio al agregar libro con ISBN: {0}", dto?.ISBN);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<LibroDto>> UpdateAsync(int id, UpdateLibroDto dto)
        {
            _logger.Info("Iniciando actualización para libro con ID {0}", id);
            try
            {
                var validationResult = await _libroValidator.ValidateForUpdateAsync(id, dto);
                if (!validationResult.IsSuccess)
                {
                    _logger.Error("Validación de negocio para actualización falló: {0}", validationResult.Message);
                    return OperationResult<LibroDto>.Failure(validationResult.Message);
                }

                var libroEntidad = validationResult.Data;
                libroEntidad.ActualizarDetalles(dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.UpdateAsync(libroEntidad);
                if (!repoResult.IsSuccess)
                {
                    _logger.Error("El repositorio falló al actualizar el libro: {0}", repoResult.Message);
                    return OperationResult<LibroDto>.Failure(repoResult.Message);
                }

                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en el servicio al actualizar libro con ID: {0}", id);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            _logger.Info("Iniciando eliminación para libro con ID {0}", id);
            try
            {
                var validationResult = await _libroValidator.ValidateForDeleteAsync(id);
                if (!validationResult.IsSuccess)
                {
                    _logger.Error("Validación de negocio para eliminación falló: {0}", validationResult.Message);
                    return validationResult;
                }

                var repoResult = await _libroRepository.DeleteAsync(id);
                if (!repoResult.IsSuccess)
                {
                    _logger.Error("El repositorio falló al eliminar el libro: {0}", repoResult.Message);
                }

                return repoResult;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en el servicio al eliminar libro con ID: {0}", id);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<LibroDto>> GetByIdAsync(int id)
        {
            _logger.Info("Obteniendo detalles del libro con ID {0}", id);
            return await _libroRepository.ObtenerDetallesDTOPorIdAsync(id);
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> GetAllAsync()
        {
            _logger.Info("Obteniendo todos los libros.");
            return await _libroRepository.ObtenerTodosConDetallesAsync();
        }

        #endregion

        #region Implementación de ILibroService (Métodos Específicos)

        public async Task<OperationResult<LibroDto>> BuscarPorIsbnAsync(string isbn)
        {
            _logger.Info("Buscando libro por ISBN {0}", isbn);
            return await _libroRepository.ObtenerDetallesDTOPorIsbnAsync(isbn);
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> BuscarLibrosAsync(string terminoBusqueda)
        {
            _logger.Info("Buscando libros con el término '{0}'", terminoBusqueda);
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
                return await GetAllAsync();

            return await _libroRepository.BuscarConDetallesAsync(terminoBusqueda);
        }

        #endregion
    }
}