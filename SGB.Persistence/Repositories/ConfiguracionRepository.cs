using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;

namespace SGB.Persistence.Repositories
{
    public class ConfiguracionRepository : BaseRepository<Configuracion>, IConfiguracionRepository
    {
        private readonly ILogger<ConfiguracionRepository> _logger;
        private readonly IConfiguration _configuration;

        public ConfiguracionRepository(SGBContext context, ILoggerFactory loggerFactory,
            IConfiguration configuration): base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<ConfiguracionRepository>();
        }

        #region Metodos sobreescritos
        public override async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var config = await GetByIdAsync(id);
                if (config == null)
                {
                    return new OperationResult { Success = false, Message = "Configuración no encontrada." };
                }
                config.EstaActivo = false;
                return await UpdateAsync(config);
            }
            catch (Exception ex)
            {
                const string msg = "Error al deshabilitar configuración.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        public async Task<OperationResult> ObtenerPorIdAsync(int id)
        {
            try
            {
                var configuracion = await Entity.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.IDConfiguracion == id);

                if (configuracion == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Configuración no encontrada."
                    };
                }

                return new OperationResult
                {
                    Success = true,
                    Data = configuracion
                };
            }
            catch (Exception ex)
            {
                const string errorMsg = "Error al obtener configuración por ID.";
                _logger.LogError(ex, errorMsg);
                return new OperationResult
                {
                    Success = false,
                    Message = errorMsg
                };
            }
        }

        #endregion

        #region Metodo de la interface
        public async Task<OperationResult> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                var result = await Entity.AsNoTracking() .Where(c => c.Nombre == nombre).ToListAsync();
                return new OperationResult { Data = result };
            }
            catch (Exception ex)
            {
                const string errorMsg = "Error al buscar configuración por nombre.";
                _logger.LogError(ex, errorMsg);
                return new OperationResult {Success = false, Message = errorMsg};
            }
        }
        #endregion
    }
}
