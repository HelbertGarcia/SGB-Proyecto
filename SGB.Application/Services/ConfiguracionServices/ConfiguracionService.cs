using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Contracts.Mappers;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Validators.BusinessValidators.Configuracion;
using Microsoft.Extensions.Configuration;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Application.Services.ConfiguracionServices
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionRepository _repo;
        private readonly IConfiguracionMapper _mapper;
        private readonly IConfiguracionValidator _validator;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger<ConfiguracionService> _logger;

        public ConfiguracionService(IConfiguracionRepository repo, IConfiguracionMapper mapper, IConfiguracionValidator validator,
                                    IConfiguration configuration, IAppLogger<ConfiguracionService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _validator = validator;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<OperationResult<ConfiguracionDto>> AddAsync(AddConfiguracionDto dto)
        {
            _logger.Info("Iniciando AddAsync para configuración: {0}", dto?.Nombre);

            if (string.IsNullOrWhiteSpace(dto?.Nombre))
                return OperationResult<ConfiguracionDto>.Failure("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(dto?.Valor))
                return OperationResult<ConfiguracionDto>.Failure("El valor no puede estar vacío.");

            var validation = await _validator.ValidarAsync(dto);
            if (!validation.IsSuccess)
                return OperationResult<ConfiguracionDto>.Failure(validation.Message);

            var entity = new Configuracion(dto.Nombre, dto.Valor, dto.Descripcion);
            var repoResult = await _repo.AddAsync(entity);
            if (!repoResult.IsSuccess || repoResult.Data == null)
                return OperationResult<ConfiguracionDto>.Failure(repoResult.Message);

            var dtoResult = _mapper.MapToDto(repoResult.Data);
            return OperationResult<ConfiguracionDto>.Success(dtoResult, "Configuración registrada exitosamente.");
        }

        public async Task<OperationResult<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto)
        {
            _logger.Info("Iniciando UpdateAsync para configuración ID: {0}", id);

            var result = await _repo.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<ConfiguracionDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Configuración no encontrada.");

            var entity = result.Data;
            var validation = await _validator.ValidateForUpdateAsync(dto);
            if (!validation.IsSuccess)
                return OperationResult<ConfiguracionDto>.Failure(validation.Message);

            _mapper.ApplyUpdateDto(entity, dto);

            var updateResult = await _repo.UpdateAsync(entity);
            if (!updateResult.IsSuccess || updateResult.Data == null)
                return OperationResult<ConfiguracionDto>.Failure(updateResult.Message);

            var dtoActualizado = _mapper.MapToDto(updateResult.Data);
            return OperationResult<ConfiguracionDto>.Success(dtoActualizado, "Configuración actualizada correctamente.");
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            _logger.Info("Iniciando DeleteAsync para configuración ID: {0}", id);

            var result = await _repo.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Configuración no encontrada.");

            var entity = result.Data;

            if (entity.Nombre == "config_sistema_base")
                return OperationResult<bool>.Failure("Esta configuración es protegida y no puede eliminarse.");

            return await _repo.DeleteAsync(id);
        }

        public async Task<OperationResult<ConfiguracionDto>> GetByIdAsync(int id)
        {
            _logger.Info("Obteniendo configuración por ID: {0}", id);

            var result = await _repo.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<ConfiguracionDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Configuración no encontrada.");

            var dto = _mapper.MapToDto(result.Data);
            return OperationResult<ConfiguracionDto>.Success(dto);
        }

        public async Task<OperationResult<ConfiguracionDto>> ObtenerPorNombreAsync(string nombre)
        {
            _logger.Info("Buscando configuración por nombre: {0}", nombre);

            var result = await _repo.ObtenerPorNombreAsync(nombre);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<ConfiguracionDto>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Configuración no encontrada.");

            var dto = _mapper.MapToDto(result.Data);
            return OperationResult<ConfiguracionDto>.Success(dto);
        }
        public async Task<OperationResult<IEnumerable<ConfiguracionDto>>> GetAllAsync()
        {
            _logger.Info("Obteniendo todas las configuraciones activas.");

            var result = await _repo.GetAllAsync();
            if (!result.IsSuccess || result.Data == null)
            {
                _logger.Error("Falló la obtención de configuraciones: {0}", result.Message ?? "Sin mensaje.");
                return OperationResult<IEnumerable<ConfiguracionDto>>.Failure(result.Message ?? "No se pudo obtener la lista.");
            }

            var dtoList = result.Data
                .Where(c => c.EstaActivo && !string.IsNullOrWhiteSpace(c.Nombre))
                .Select(c => _mapper.MapToDto(c))
                .ToList();

            _logger.Info("Se obtuvieron {0} configuraciones activas.", dtoList.Count);

            return OperationResult<IEnumerable<ConfiguracionDto>>.Success(dtoList, "Lista obtenida correctamente.");
        }


    }
}
