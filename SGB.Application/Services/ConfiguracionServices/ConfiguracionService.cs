using Microsoft.Extensions.Configuration;
using SGB.Application.Contracts.Mappers;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Validators.BusinessValidators;
using SGB.Application.Validators.BusinessValidators.Configuracion;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Application.Services.ConfiguracionServices
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionRepository _repository;
        private readonly IConfiguracionMapper _mapper;
        private readonly IConfiguracionValidator _validator;
        private readonly IAppLogger<ConfiguracionService> _logger;
        private readonly IConfiguration _configuration;

        public ConfiguracionService(
            IConfiguracionRepository repository,
            IConfiguracionMapper mapper,
            IConfiguracionValidator validator,
            IAppLogger<ConfiguracionService> logger,
            IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<ConfiguracionDto>> AddAsync(AddConfiguracionDto dto)
        {
            _logger.Info("Iniciando proceso para agregar configuración: {0}", dto?.Nombre);
            try
            {
                var validationResult = await _validator.ValidateForAddAsync(dto);
                if (!validationResult.IsSuccess)
                {
                    _logger.Error("Validación fallida al agregar configuración: {0}", validationResult.Message);
                    return OperationResult<ConfiguracionDto>.Failure(validationResult.Message);
                }

                var nuevaConfiguracion = _mapper.MapFromDto(dto);
                var repoResult = await _repository.AddAsync(nuevaConfiguracion);
                if (!repoResult.IsSuccess)
                {
                    _logger.Error("Error al persistir configuración: {0}", repoResult.Message);
                    return OperationResult<ConfiguracionDto>.Failure(repoResult.Message);
                }

                var dtoResult = _mapper.MapToDto(repoResult.Data);
                _logger.Info("Configuración creada con éxito, ID: {0}", repoResult.Data.IDConfiguracion);
                return OperationResult<ConfiguracionDto>.Success(dtoResult, "Configuración registrada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al agregar configuración: {0}", dto?.Nombre);
                return OperationResult<ConfiguracionDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto)
        {
            _logger.Info("Iniciando actualización para configuración ID: {0}", id);
            try
            {
                var result = await _repository.ObtenerPorIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                {
                    _logger.Error("Configuración no encontrada para ID: {0}", id);
                    return OperationResult<ConfiguracionDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);
                }

                var entity = result.Data;
                _mapper.ApplyUpdateDto(entity, dto);

                var updateResult = await _repository.UpdateAsync(entity);
                if (!updateResult.IsSuccess)
                {
                    _logger.Error("Error al actualizar configuración: {0}", updateResult.Message);
                    return OperationResult<ConfiguracionDto>.Failure(updateResult.Message);
                }

                var dtoResult = _mapper.MapToDto(updateResult.Data);
                return OperationResult<ConfiguracionDto>.Success(dtoResult, "Configuración actualizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en actualización de configuración ID: {0}", id);
                return OperationResult<ConfiguracionDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            _logger.Info("Iniciando eliminación de configuración ID: {0}", id);
            try
            {
                var validationResult = await _validator.ValidateForDeleteAsync(id);
                if (!validationResult.IsSuccess)
                {
                    _logger.Error("Validación falló antes de eliminar configuración: {0}", validationResult.Message);
                    return validationResult;
                }

                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al eliminar configuración ID: {0}", id);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<ConfiguracionDto>> GetByIdAsync(int id)
        {
            _logger.Info("Buscando configuración por ID: {0}", id);
            var result = await _repository.ObtenerPorIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<ConfiguracionDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);

            var dto = _mapper.MapToDto(result.Data);
            return OperationResult<ConfiguracionDto>.Success(dto);
        }

        public async Task<OperationResult<IEnumerable<ConfiguracionDto>>> GetAllAsync()
        {
            _logger.Info("Consultando lista completa de configuraciones.");
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<IEnumerable<ConfiguracionDto>>.Failure(result.Message);

            var lista = result.Data
                .Where(c => !string.IsNullOrWhiteSpace(c.Nombre))
                .Select(c => _mapper.MapToDto(c));

            return OperationResult<IEnumerable<ConfiguracionDto>>.Success(lista, "Lista obtenida correctamente.");
        }
    }
}
