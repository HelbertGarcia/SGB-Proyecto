using Microsoft.Extensions.Configuration;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Validators.BusinessValidators;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Application.Services.LibrosServices
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ICategoriaBusinessValidator _categoriaValidator;
        private readonly IAppLogger<CategoriaService> _logger; 
        private readonly IConfiguration _configuration;

        public CategoriaService(
            ICategoriaRepository categoriaRepository,
            ICategoriaBusinessValidator categoriaValidator,
            IAppLogger<CategoriaService> logger,
            IConfiguration configuration)
        {
            _categoriaRepository = categoriaRepository;
            _categoriaValidator = categoriaValidator;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<CategoriaDto>> AddAsync(AddCategoriaDto dto)
        {
            _logger.Info("Iniciando proceso para agregar categoría: {0}", dto?.Nombre);
            try
            {
                var validationResult = await _categoriaValidator.ValidateForAddAsync(dto);
                if (!validationResult.IsSuccess)
                {
                    _logger.Error("Validación de negocio falló al agregar categoría: {0}", validationResult.Message);
                    return OperationResult<CategoriaDto>.Failure(validationResult.Message);
                }

                var nuevaCategoria = new Categoria(dto.Nombre);
                var repoResult = await _categoriaRepository.AddAsync(nuevaCategoria);
                if (!repoResult.IsSuccess)
                {
                    _logger.Error("El repositorio falló al agregar la categoría: {0}", repoResult.Message);
                    return OperationResult<CategoriaDto>.Failure(repoResult.Message);
                }

                var categoriaCreada = repoResult.Data;
                var categoriaDto = new CategoriaDto(categoriaCreada.Id, categoriaCreada.Nombre, categoriaCreada.EstaActivo);

                _logger.Info("Categoría registrada exitosamente con ID {0}", categoriaCreada.Id);
                return OperationResult<CategoriaDto>.Success(categoriaDto, "Categoría creada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en el servicio al crear categoría: {0}", dto?.Nombre);
                return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<CategoriaDto>> UpdateAsync(int id, UpdateCategoriaDto dto)
        {
            _logger.Info("Iniciando actualización para categoría con ID {0}", id);
            try
            {
                var categoriaResult = await _categoriaRepository.GetByIdAsync(id);
                if (!categoriaResult.IsSuccess || categoriaResult.Data == null)
                {
                    _logger.Error("No se encontró la categoría con ID {0} para actualizar.", id);
                    return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);
                }

                var categoriaEntidad = categoriaResult.Data;
                categoriaEntidad.ActualizarNombre(dto.Nombre);

                var repoResult = await _categoriaRepository.UpdateAsync(categoriaEntidad);
                if (!repoResult.IsSuccess)
                {
                    _logger.Error("El repositorio falló al actualizar la categoría: {0}", repoResult.Message);
                    return OperationResult<CategoriaDto>.Failure(repoResult.Message);
                }

                var categoriaDto = new CategoriaDto(repoResult.Data.Id, repoResult.Data.Nombre, repoResult.Data.EstaActivo);
                return OperationResult<CategoriaDto>.Success(categoriaDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en el servicio al actualizar categoría con ID: {0}", id);
                return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            _logger.Info("Iniciando eliminación para categoría con ID {0}", id);
            try
            {
                var validationResult = await _categoriaValidator.ValidateForDeleteAsync(id);
                if (!validationResult.IsSuccess)
                {
                    _logger.Error("Validación de negocio para eliminación falló: {0}", validationResult.Message);
                    return validationResult;
                }

                return await _categoriaRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en el servicio al eliminar categoría con ID: {0}", id);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<CategoriaDto>> GetByIdAsync(int id)
        {
            _logger.Info("Obteniendo categoría por ID: {0}", id);
            var result = await _categoriaRepository.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<CategoriaDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);

            var dto = new CategoriaDto(result.Data.Id, result.Data.Nombre, result.Data.EstaActivo);
            return OperationResult<CategoriaDto>.Success(dto);
        }

        public async Task<OperationResult<IEnumerable<CategoriaDto>>> GetAllAsync()
        {
            _logger.Info("Obteniendo todas las categorías activas.");
            var result = await _categoriaRepository.GetAllAsync(); 
            if (!result.IsSuccess)
                return OperationResult<IEnumerable<CategoriaDto>>.Failure(result.Message);

            var dtoList = result.Data.Select(c => new CategoriaDto(c.Id, c.Nombre, c.EstaActivo));
            return OperationResult<IEnumerable<CategoriaDto>>.Success(dtoList);
        }
    }
}