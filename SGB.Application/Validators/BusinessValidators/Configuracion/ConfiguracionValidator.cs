using SGB.Domain.Base;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Api.Contracts.Repository.Interfaces;

namespace SGB.Api.Validators.BusinessValidators.Configuracion
{
    public class ConfiguracionValidator : IConfiguracionValidator
    {
        private readonly IConfiguracionRepository _configuracionRepository;

        public ConfiguracionValidator(IConfiguracionRepository configuracionRepository)
        {
            _configuracionRepository = configuracionRepository;
        }

        public async Task<OperationResult<bool>> ValidarAsync(AddConfiguracionDto dto)
        {
            var existente = await _configuracionRepository.ObtenerPorNombreAsync(dto.Nombre);
            if (existente.IsSuccess && existente.Data != null)
            return OperationResult<bool>.Failure("Ya existe una configuración con ese nombre.");
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> ValidateForUpdateAsync(UpdateConfiguracionDto dto)
        {
            var actual = await _configuracionRepository.GetByIdAsync(dto.IDConfiguracion);
            if (!actual.IsSuccess || actual.Data == null)
            return OperationResult<bool>.Failure("La configuración que desea actualizar no existe.");
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                var duplicado = await _configuracionRepository.ObtenerPorNombreAsync(dto.Nombre);
                if (duplicado.IsSuccess &&
                    duplicado.Data != null &&
                    duplicado.Data.IDConfiguracion != dto.IDConfiguracion)
                {
                    return OperationResult<bool>.Failure("Ya existe otra configuración con ese nombre.");
                }
            }
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> ValidateForDeleteAsync(int id)
        {
            var actual = await _configuracionRepository.GetByIdAsync(id);
            if (!actual.IsSuccess || actual.Data == null)
            return OperationResult<bool>.Failure("La configuración que desea deshabilitar no existe.");
            var nombre = actual.Data.Nombre?.ToLowerInvariant() ?? string.Empty;
            if (nombre.Contains("sistema") || nombre.Contains("protegida"))
            return OperationResult<bool>.Failure("Esta configuración es protegida y no puede deshabilitarse.");
            return OperationResult<bool>.Success(true);
        }
    }
}
