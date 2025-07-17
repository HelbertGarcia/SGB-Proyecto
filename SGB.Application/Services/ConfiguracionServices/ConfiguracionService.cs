using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;
using System.Linq;
using SGB.Domain.Entities.Configuracion;

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

        public async Task<OperationResult<ConfiguracionDto>> AddAsync(AddConfiguracionDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Valor))
                    return OperationResult<ConfiguracionDto>.Failure("El nombre y el valor son obligatorios.");

                var entity = new Configuracion(dto.Nombre, dto.Valor, dto.Descripcion);
                var result = await _repository.AddAsync(entity);

                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<ConfiguracionDto>.Failure(result.Message);

                var dtoResult = new ConfiguracionDto
                {
                    IDConfiguracion = result.Data.IDConfiguracion,
                    Nombre = result.Data.Nombre,
                    Valor = result.Data.Valor,
                    Descripcion = result.Data.Descripcion,
                    FechaCreacion = result.Data.FechaCreacion,
                    EstaActivo = result.Data.EstaActivo
                };

                return OperationResult<ConfiguracionDto>.Success(dtoResult, "Configuración guardada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al guardar configuración.");
                return OperationResult<ConfiguracionDto>.Failure("Error interno al guardar configuración.");
            }
        }

        public async Task<OperationResult<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                var entity = result.Data;

                if (!result.IsSuccess || entity == null)
                    return OperationResult<ConfiguracionDto>.Failure("Configuración no encontrada.");

                entity.Valor = dto.Valor;
                entity.Descripcion = dto.Descripcion;
                entity.EstaActivo = dto.EstaActivo ?? entity.EstaActivo;

                var updateResult = await _repository.UpdateAsync(entity);
                if (!updateResult.IsSuccess || updateResult.Data == null)
                    return OperationResult<ConfiguracionDto>.Failure(updateResult.Message);

                var dtoResult = new ConfiguracionDto
                {
                    IDConfiguracion = updateResult.Data.IDConfiguracion,
                    Nombre = updateResult.Data.Nombre,
                    Valor = updateResult.Data.Valor,
                    Descripcion = updateResult.Data.Descripcion,
                    FechaCreacion = updateResult.Data.FechaCreacion,
                    EstaActivo = updateResult.Data.EstaActivo
                };

                return OperationResult<ConfiguracionDto>.Success(dtoResult, "Configuración actualizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar configuración.");
                return OperationResult<ConfiguracionDto>.Failure("Error al actualizar la configuración.");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return OperationResult<bool>.Failure("ID inválido para eliminación.");

                var result = await _repository.GetByIdAsync(id);
                var entity = result.Data;

                if (!result.IsSuccess || entity == null)
                    return OperationResult<bool>.Failure("Configuración no encontrada.");

                var deleteResult = await _repository.DeleteAsync(entity.IDConfiguracion);
                return deleteResult.IsSuccess
                    ? OperationResult<bool>.Success(true, "Configuración eliminada correctamente.")
                    : OperationResult<bool>.Failure(deleteResult.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar configuración.");
                return OperationResult<bool>.Failure("Error al eliminar la configuración.");
            }
        }

        public async Task<OperationResult<IEnumerable<ConfiguracionDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();

                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<IEnumerable<ConfiguracionDto>>.Failure(result.Message);

                var dtos = result.Data.Select(c => new ConfiguracionDto
                {
                    IDConfiguracion = c.IDConfiguracion,
                    Nombre = c.Nombre,
                    Valor = c.Valor,
                    Descripcion = c.Descripcion,
                    FechaCreacion = c.FechaCreacion,
                    EstaActivo = c.EstaActivo
                });

                return OperationResult<IEnumerable<ConfiguracionDto>>.Success(dtos, "Configuraciones obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las configuraciones.");
                return OperationResult<IEnumerable<ConfiguracionDto>>.Failure("Error al obtener las configuraciones.");
            }
        }

        public async Task<OperationResult<ConfiguracionDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                var entity = result.Data;

                if (!result.IsSuccess || entity == null)
                    return OperationResult<ConfiguracionDto>.Failure("Configuración no encontrada.");

                var dto = new ConfiguracionDto
                {
                    IDConfiguracion = entity.IDConfiguracion,
                    Nombre = entity.Nombre,
                    Valor = entity.Valor,
                    Descripcion = entity.Descripcion,
                    FechaCreacion = entity.FechaCreacion,
                    EstaActivo = entity.EstaActivo
                };

                return OperationResult<ConfiguracionDto>.Success(dto, "Configuración obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración por ID.");
                return OperationResult<ConfiguracionDto>.Failure("Error al obtener la configuración.");
            }
        }
    }
}

