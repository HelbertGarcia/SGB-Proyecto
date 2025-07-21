using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Validators.BusinessValidators;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;

namespace SGB.Application.Services.LibrosServices
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly ILibroBusinessValidator _libroValidator;
        private readonly ILogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        public LibroService(
            ILibroRepository libroRepository,
            ILibroBusinessValidator libroValidator,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _libroRepository = libroRepository;
            _libroValidator = libroValidator;
            _logger = loggerFactory.CreateLogger<LibroService>();
            _configuration = configuration;
        }

        #region Implementación de IBaseService

        public async Task<OperationResult<LibroDto>> AddAsync(AddLibroDto dto)
        {
            try
            {
                var validationResult = await _libroValidator.ValidateForAddAsync(dto);
                if (!validationResult.IsSuccess)
                {
                    return OperationResult<LibroDto>.Failure(validationResult.Message);
                }

                var categoriaValidada = validationResult.Data;
                var libroEntidad = new Libro(dto.ISBN, dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.AddAsync(libroEntidad);
                if (!repoResult.IsSuccess)
                    return OperationResult<LibroDto>.Failure(repoResult.Message);

                var libroCreado = repoResult.Data;
                var libroCreadoDto = new LibroDto
                {
                    Id = libroCreado.Id,
                    ISBN = libroCreado.ISBN,
                    Titulo = libroCreado.Titulo,
                    Autor = libroCreado.Autor,
                    Editorial = libroCreado.Editorial,
                    FechaPublicacion = libroCreado.FechaPublicacion,
                    NombreCategoria = categoriaValidada.Nombre,
                    Estado = "Disponible",
                    FechaRegistro = libroCreado.FechaRegistro
                };

                return OperationResult<LibroDto>.Success(libroCreadoDto, "Libro creado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al agregar libro con ISBN: {ISBN}", dto?.ISBN);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<LibroDto>> UpdateAsync(int id, UpdateLibroDto dto)
        {
            try
            {
                var validationResult = await _libroValidator.ValidateForUpdateAsync(id, dto);
                if (!validationResult.IsSuccess)
                    return OperationResult<LibroDto>.Failure(validationResult.Message);

                var libroEntidad = validationResult.Data;
                libroEntidad.ActualizarDetalles(dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.UpdateAsync(libroEntidad);
                if (!repoResult.IsSuccess)
                    return OperationResult<LibroDto>.Failure(repoResult.Message);

                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al actualizar libro con ID: {ID}", id);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var validationResult = await _libroValidator.ValidateForDeleteAsync(id);
                if (!validationResult.IsSuccess)
                    return validationResult;

                return await _libroRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al eliminar libro con ID: {ID}", id);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<LibroDto>> GetByIdAsync(int id)
        {
            return await _libroRepository.ObtenerDetallesDTOPorIdAsync(id);
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> GetAllAsync()
        {
            return await _libroRepository.ObtenerTodosConDetallesAsync();
        }

        #endregion

        #region Implementación de ILibroService (Métodos Específicos)

        public async Task<OperationResult<LibroDto>> BuscarPorIsbnAsync(string isbn)
        {
            return await _libroRepository.ObtenerDetallesDTOPorIsbnAsync(isbn);
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> BuscarLibrosAsync(string terminoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
                return await GetAllAsync();

            return await _libroRepository.BuscarConDetallesAsync(terminoBusqueda);
        }

        #endregion
    }
}