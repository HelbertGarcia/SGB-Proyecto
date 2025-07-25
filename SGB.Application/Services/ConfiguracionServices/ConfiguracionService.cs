using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Application.Services.ConfiguracionServices
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionRepository _repository;
        private readonly IAppLogger<ConfiguracionService> _logger;

        public ConfiguracionService(IConfiguracionRepository repository, IAppLogger<ConfiguracionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<ConfiguracionDto>> AddAsync(AddConfiguracionDto dto)
        {
            try
            {
                var nuevaEntidad = new SGB.Domain.Entities.Configuracion.Configuracion(
                    nombre: dto.Nombre,
                    valor: dto.Valor,
                    descripcion: dto.Descripcion
                );

                await _repository.AddAsync(nuevaEntidad);

                var resultado = new ConfiguracionDto
                {
                    IDConfiguracion = nuevaEntidad.IDConfiguracion,
                    Nombre = nuevaEntidad.Nombre,
                    Valor = nuevaEntidad.Valor,
                    Descripcion = nuevaEntidad.Descripcion,
                    FechaCreacion = nuevaEntidad.FechaCreacion,
                    EstaActivo = nuevaEntidad.EstaActivo
                };

                return OperationResult<ConfiguracionDto>.Success(resultado, "Configuración creada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error en AddAsync", ex);
                return OperationResult<ConfiguracionDto>.Failure("No se pudo crear la configuración.");
            }
        }

        public async Task<OperationResult<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto)
        {
            try
            {
                var entidadResult = await _repository.GetByIdAsync(id);
                var entidad = entidadResult.Data;

                if (entidad == null)
                    return OperationResult<ConfiguracionDto>.Failure("Configuración no encontrada.");

                entidad.Nombre = dto.Nombre;
                entidad.Valor = dto.Valor;
                entidad.Descripcion = dto.Descripcion;
                entidad.EstaActivo = dto.EstaActivo ?? true;

                await _repository.UpdateAsync(entidad);

                var resultado = new ConfiguracionDto
                {
                    IDConfiguracion = entidad.IDConfiguracion,
                    Nombre = entidad.Nombre,
                    Valor = entidad.Valor,
                    Descripcion = entidad.Descripcion,
                    FechaCreacion = entidad.FechaCreacion,
                    EstaActivo = entidad.EstaActivo
                };

                return OperationResult<ConfiguracionDto>.Success(resultado, "Configuración actualizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error en UpdateAsync", ex);
                return OperationResult<ConfiguracionDto>.Failure("No se pudo actualizar la configuración.");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var entidadResult = await _repository.GetByIdAsync(id);
                var entidad = entidadResult.Data;

                if (entidad == null)
                    return OperationResult<bool>.Failure("Configuración no encontrada.");

                entidad.Deshabilitar();
                await _repository.UpdateAsync(entidad);

                return OperationResult<bool>.Success(true, "Configuración deshabilitada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error en DeleteAsync", ex);
                return OperationResult<bool>.Failure("No se pudo eliminar la configuración.");
            }
        }

        public async Task<OperationResult<IEnumerable<ConfiguracionDto>>> GetAllAsync()
        {
            try
            {
                var listaResult = await _repository.GetAllAsync();
                var lista = listaResult.Data ?? Enumerable.Empty<SGB.Domain.Entities.Configuracion.Configuracion>();

                var resultado = lista.Select(c => new ConfiguracionDto
                {
                    IDConfiguracion = c.IDConfiguracion,
                    Nombre = c.Nombre,
                    Valor = c.Valor,
                    Descripcion = c.Descripcion,
                    FechaCreacion = c.FechaCreacion,
                    EstaActivo = c.EstaActivo
                });

                return OperationResult<IEnumerable<ConfiguracionDto>>.Success(resultado, "Lista obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error en GetAllAsync", ex);
                return OperationResult<IEnumerable<ConfiguracionDto>>.Failure("No se pudo obtener la lista.");
            }
        }

        public async Task<OperationResult<ConfiguracionDto>> GetByIdAsync(int id)
        {
            try
            {
                var entidadResult = await _repository.GetByIdAsync(id);
                var entidad = entidadResult.Data;

                if (entidad == null)
                    return OperationResult<ConfiguracionDto>.Failure("Configuración no encontrada.");

                var dto = new ConfiguracionDto
                {
                    IDConfiguracion = entidad.IDConfiguracion,
                    Nombre = entidad.Nombre,
                    Valor = entidad.Valor,
                    Descripcion = entidad.Descripcion,
                    FechaCreacion = entidad.FechaCreacion,
                    EstaActivo = entidad.EstaActivo
                };

                return OperationResult<ConfiguracionDto>.Success(dto, "Configuración obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error en GetByIdAsync", ex);
                return OperationResult<ConfiguracionDto>.Failure("No se pudo obtener la configuración.");
            }
        }
    }
}
