using SGB.Domain.Base;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Domain.Entities.Configuracion;
using SGB.Application.Contracts.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Persistence.Repositories
{
    public class ConfiguracionRepository : BaseRepository<Configuracion>, IConfiguracionRepository
    {
        private readonly IAppLogger<ConfiguracionRepository> _logger;
        private readonly IConfiguration _configuration;

        public ConfiguracionRepository(SGBContext context,ILoggerFactory loggerFactory, IConfiguration configuration,IAppLogger<ConfiguracionRepository> logger) 
        : base(context, loggerFactory, configuration) 
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public async Task<OperationResult<IEnumerable<Configuracion>>> GetAllAsync()
        {
            try
            {
                var configs = await Entity
                    .AsNoTracking()
                    .Where(c => c.EstaActivo) 
                    .ToListAsync();
                return OperationResult<IEnumerable<Configuracion>>.Success(configs);
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Configuracion:GetAll"];
                _logger.Error(ex, "{0}", msg);
                return OperationResult<IEnumerable<Configuracion>>.Failure(msg ?? "Error al obtener configuraciones.");
            }
        }

        public override async Task<OperationResult<Configuracion>> AddAsync(Configuracion entity)
        {
            try
            {
                if (entity.Nombre == "config_base")
                {
                    var msg = "No se permite agregar configuración protegida.";
                    _logger.Error(msg + " - Clave: {Clave}", entity.Nombre);
                    return OperationResult<Configuracion>.Failure(msg);
                }
                return await base.AddAsync(entity);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:Add"];
                _logger.Error(ex, "{ErrorMessage} - Clave: {Clave}", errorMessage, entity.Nombre);
                return OperationResult<Configuracion>.Failure(errorMessage ?? "Error al agregar configuración.");
            }
        }

        public override async Task<OperationResult<Configuracion>> UpdateAsync(Configuracion entity)
        {
            try
            {
                if (entity.Nombre == "config_base")
                {
                    var msg = "Esta configuración es protegida y no puede modificarse.";
                    _logger.Error(msg + " - Clave: {Clave}", entity.Nombre);
                    return OperationResult<Configuracion>.Failure(msg);
                }
                return await base.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:Update"];
                _logger.Error(ex, "{ErrorMessage} - Clave: {Clave}", errorMessage, entity.Nombre);
                return OperationResult<Configuracion>.Failure(errorMessage ?? "Error al actualizar configuración.");
            }
        }


        public override async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var configParaEliminar = await Entity.FindAsync(id);
                if (configParaEliminar == null)
                {
                    var msg = _configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Configuración no encontrada.";
                    _logger.Error("DeleteAsync falló: {0} - ID: {1}", msg, id);
                    return OperationResult<bool>.Failure(msg);
                }
                if (configParaEliminar.Nombre == "config_sistema_base")
                {
                    var msg = "Esta configuración es protegida y no puede eliminarse.";
                    _logger.Error(msg + " - ID: {0}", id);
                    return OperationResult<bool>.Failure(msg);
                }
                configParaEliminar.Deshabilitar(); 
                var updateResult = await base.UpdateAsync(configParaEliminar);
                return OperationResult<bool>.Success(updateResult.IsSuccess, updateResult.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:DeleteError"];
                _logger.Error(ex, "{0} - ID: {1}", errorMessage ?? "Error al eliminar configuración", id);
                return OperationResult<bool>.Failure(errorMessage ?? "Ocurrió un error al eliminar la configuración.");
            }
        }

        public async Task<OperationResult<Configuracion>> ObtenerPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _logger.Error("El nombre de configuración está vacío.");
                return OperationResult<Configuracion>.Failure("El nombre de la configuración no puede estar vacío.");
            }
            try
            {
                var configuracion = await Entity
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Nombre == nombre && c.EstaActivo);
                if (configuracion == null)
                {
                    var msg = $"No se encontró configuración activa con el nombre '{nombre}'.";
                    _logger.Error(msg);
                    return OperationResult<Configuracion>.Failure(msg);
                }
                return OperationResult<Configuracion>.Success(configuracion);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:GetByName"];
                _logger.Error(ex, "{0} para el nombre: {1}", errorMessage, nombre);
                return OperationResult<Configuracion>.Failure(errorMessage ?? "Error al obtener configuración por nombre.");
            }
        }
    }
}
