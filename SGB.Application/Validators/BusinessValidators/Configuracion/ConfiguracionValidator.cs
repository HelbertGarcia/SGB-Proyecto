using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;
using SGB.Persistence.Interfaces;

namespace SGB.Application.Validators.BusinessValidators.Configuracion
{
    public class ConfiguracionValidator : IConfiguracionValidator   
    {
        private readonly IConfiguracionRepository _configuracionRepository;

        public ConfiguracionValidator(IConfiguracionRepository configuracionRepository)
        {
            _configuracionRepository = configuracionRepository;
        }

        public async Task<OperationResult> ValidateForAddAsync(AddConfiguracionDto dto)
        {
            var existente = await _configuracionRepository.ObtenerPorNombreAsync(dto.Nombre);
            if (existente.Success && existente.Data != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Ya existe una configuración con ese nombre."
                };
            }
            return new OperationResult
            {
                Success = true
            };
        }

        public async Task<OperationResult> ValidateForUpdateAsync(UpdateConfiguracionDto dto)
        {
            var actual = await _configuracionRepository.ObtenerPorIdAsync(dto.IDConfiguracion);
            if (!actual.Success || actual.Data == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "La configuración que desea actualizar no existe."
                };
            }
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                var duplicado = await _configuracionRepository.ObtenerPorNombreAsync(dto.Nombre);
                if (duplicado.Success && duplicado.Data != null && duplicado.Data.IDConfiguracion != dto.IDConfiguracion)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Ya existe otra configuración con ese nombre."
                    };
                }
            }
            return new OperationResult
            {
                Success = true
            };
        }

        public async Task<OperationResult> ValidateForDeleteAsync(int id)
        {
            var actual = await _configuracionRepository.ObtenerPorIdAsync(id);
            if (!actual.Success || actual.Data == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "La configuración que desea eliminar no existe."
                };
            }
            var nombre = actual.Data.Nombre?.ToLower() ?? string.Empty;
            if (nombre.Contains("sistema") || nombre.Contains("protegida"))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Esta configuración es protegida y no puede eliminarse."
                };
            }
            return new OperationResult
            {
                Success = true
            };
        }



    }
}