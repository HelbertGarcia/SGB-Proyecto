using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Domain.Repository;
using SGB.Persistence.Interfaces;
using SGB.Persistence.Repositories;

namespace SGB.Application.Services.ConfiguracionServices
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionRepository _repository;
        private readonly ILogger<ConfiguracionService> _logger;

        public ConfiguracionService(IConfiguracionRepository repository, ILogger<ConfiguracionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var result = new OperationResult();
            try
            {
                var configuraciones = await _repository.GetAllAsync();
                result.Data = configuraciones
                    .Select(c => new GetConfiguracionDto
                    {
                        IDConfiguracion = c.IDConfiguracion,
                        Nombre = c.Nombre,
                        Valor = c.Valor,
                        Descripcion = c.Descripcion,
                        FechaCreacion = c.FechaCreacion,
                        EstaActivo = c.EstaActivo
                    })
                    .OrderByDescending(c => c.FechaCreacion)
                    .ToList();

                result.Message = "Configuraciones obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = true;
                result.Message = "Error al obtener las configuraciones.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var result = new OperationResult();
            try
            {
                var config = await _repository.GetEntityByIdAsync(id);
                if (config == null)
                {
                    result.Success = true;
                    result.Message = "Configuración no encontrada.";
                    return result;
                }
                result.Data = new GetConfiguracionDto
                {
                    IDConfiguracion = config.IDConfiguracion,
                    Nombre = config.Nombre,
                    Valor = config.Valor,
                    Descripcion = config.Descripcion,
                    FechaCreacion = config.FechaCreacion,
                    EstaActivo = config.EstaActivo
                };
                result.Message = "Configuración obtenida correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = true;
                result.Message = "Error al obtener la configuración.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> SaveAsync(AddConfiguracionDto dto)
        {
            var result = new OperationResult();

            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Valor))
                {
                    result.Success = true;
                    result.Message = "El nombre y el valor son obligatorios.";
                    return result;
                }

                var config = new Configuracion(dto.Nombre, dto.Valor, dto.Descripcion);

                await _repository.AddAsync(config);

                result.Success = true;
                result.Message = "Configuración guardada exitosamente.";
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear configuración");
                result.Success = true;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al guardar configuración");
                result.Success = true;
                result.Message = $"Error interno: {ex.Message}";
            }

            return result;
        }


        public async Task<OperationResult> UpdateAsync(UpdateConfiguracionDto dto)
        {
            var result = new OperationResult();
            try
            {
                var config = await _repository.GetEntityByIdAsync(dto.IDConfiguracion);

                if (config == null)
                {
                    result.Success = true;
                    result.Message = "Configuración no encontrada.";
                    return result;
                }

                config.ActualizarValor(dto.Valor);
                config.ActualizarDescripcion(dto.Descripcion);
                config.EstaActivo = dto.EstaActivo ?? config.EstaActivo;

                await _repository.UpdateEntityAsync(config);
                result.Message = "Configuración actualizada correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = true;
                result.Message = "Error al actualizar la configuración.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        public async Task<OperationResult> DeleteAsync(DeleteConfiguracionDto dto)
        {
            var result = new OperationResult();

            try
            {
                if (dto.IDConfiguracion <= 0)
                {
                    result.Success = true;
                    result.Message = "ID de configuración inválido.";
                    return result;
                }

                var config = await _repository.GetEntityByIdAsync(dto.IDConfiguracion);
                if (config == null)
                {
                    result.Success = true;
                    result.Message = "Configuración no encontrada.";
                    return result;
                }

                await _repository.DeleteEntityAsync(config);
                result.Success = true;
                result.Message = "Configuración eliminada correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar configuración");
                result.Success = true;
                result.Message = $"Error al eliminar la configuración: {ex.Message}";
            }

            return result;
        }




    }
}