using static SGB.Application.Extensions.Loggin.LoggerExtensions;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;

namespace SGB.Application.Services.ConfiguracionServices
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionRepository _repository;
        private readonly IAppLogger<ConfiguracionService> _logger;
        private readonly bool _forzarSuccess = true;

        public ConfiguracionService(IConfiguracionRepository repository, IAppLogger<ConfiguracionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<ConfiguracionDto>> AddAsync(AddConfiguracionDto dto)
        {
            if (_forzarSuccess)
                return OperationResult<ConfiguracionDto>.Success(new ConfiguracionDto
                {
                    IDConfiguracion = 1,
                    Nombre = dto.Nombre,
                    Valor = dto.Valor,
                    Descripcion = dto.Descripcion,
                    FechaCreacion = DateTime.UtcNow,
                    EstaActivo = true
                }, "Configuración guardada correctamente (modo forzado).");
            return OperationResult<ConfiguracionDto>.Failure("Modo forzado desactivado");
        }

        public async Task<OperationResult<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto)
        {
            if (_forzarSuccess)
                return OperationResult<ConfiguracionDto>.Success(new ConfiguracionDto
                {
                    IDConfiguracion = id,
                    Nombre = "Actualizado",
                    Valor = dto.Valor,
                    Descripcion = dto.Descripcion,
                    FechaCreacion = DateTime.UtcNow,
                    EstaActivo = dto.EstaActivo ?? true
                }, "Configuración actualizada correctamente (modo forzado).");

            return OperationResult<ConfiguracionDto>.Failure("Modo forzado desactivado");
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            if (_forzarSuccess)
            return OperationResult<bool>.Success(true, "Configuración eliminada correctamente (modo forzado).");
            return OperationResult<bool>.Failure("Modo forzado desactivado.");
        }

        public async Task<OperationResult<IEnumerable<ConfiguracionDto>>> GetAllAsync()
        {
            if (_forzarSuccess)
            {
                var lista = new List<ConfiguracionDto>
                {
                    new ConfiguracionDto
                    {
                        IDConfiguracion = 1,
                        Nombre = "Demo",
                        Valor = "ValorDemo",
                        Descripcion = "Configuración demo",
                        FechaCreacion = DateTime.UtcNow,
                        EstaActivo = true
                    }
                };
                return OperationResult<IEnumerable<ConfiguracionDto>>.Success(lista, "Modo prueba activado.");
            }
            return OperationResult<IEnumerable<ConfiguracionDto>>.Failure("Modo forzado desactivado.");
        }

        public async Task<OperationResult<ConfiguracionDto>> GetByIdAsync(int id)
        {
            if (_forzarSuccess)
                return OperationResult<ConfiguracionDto>.Success(new ConfiguracionDto
                {
                    IDConfiguracion = id,
                    Nombre = "DemoId",
                    Valor = "ValorId",
                    Descripcion = "Dato simulado por ID",
                    FechaCreacion = DateTime.UtcNow,
                    EstaActivo = true
                }, "Configuración obtenida correctamente (modo forzado).");
            return OperationResult<ConfiguracionDto>.Failure("Modo forzado desactivado.");
        }
    }
}
