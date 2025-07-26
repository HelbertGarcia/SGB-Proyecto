using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using static SGB.Application.Extensions.Loggin.LoggerExtensions;

namespace SGB.Persistence.Repositories
{
    public class ConfiguracionRepository : BaseRepository<Configuracion>, IConfiguracionRepository
    {
        private readonly IAppLogger<ConfiguracionRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly SGBContext _context;

        public ConfiguracionRepository(
            SGBContext context,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IAppLogger<ConfiguracionRepository> logger)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region "Override DeleteAsync"

        public override async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var configuracion = await _context.Configuraciones.FindAsync(id);

                if (configuracion == null)
                    return OperationResult<bool>.Failure("Configuración no encontrada.");

                configuracion.Deshabilitar();

                var updateResult = await base.UpdateAsync(configuracion);
                return OperationResult<bool>.Success(updateResult.IsSuccess, updateResult.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:DeleteError"] ??
                                   "Ocurrió un error al desactivar la configuración.";

                _logger.Error(ex, $"{errorMessage} para el ID: {id}");
                return OperationResult<bool>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<List<Configuracion>>> GetAllAsync()
        {
            try
            {
                var lista = await _context.Configuraciones
                    .AsNoTracking()
                    .Where(c => c.EstaActivo)
                    .ToListAsync();

                return OperationResult<List<Configuracion>>.Success(lista);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:GetAllError"] ??
                                   "Ocurrió un error al obtener la lista de configuraciones.";
                _logger.Error(ex, errorMessage);
                return OperationResult<List<Configuracion>>.Failure(errorMessage);
            }
        }


        #endregion

        #region "Implementación personalizada IConfiguracionRepository"

        public async Task<OperationResult<Configuracion>> ObtenerPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return OperationResult<Configuracion>.Failure("El nombre de la configuración no puede estar vacío.");

            try
            {
                var configuracion = await _context.Configuraciones
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Nombre == nombre);

                return OperationResult<Configuracion>.Success(configuracion);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:GetByNameError"] ??
                                   "Ocurrió un error al buscar la configuración por nombre.";

                _logger.Error(ex, $"{errorMessage} para el nombre: {nombre}");
                return OperationResult<Configuracion>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<Configuracion>> ObtenerPorIdAsync(int idConfiguracion)
        {
            try
            {
                var configuracion = await _context.Configuraciones
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.IDConfiguracion == idConfiguracion);

                if (configuracion == null)
                    return OperationResult<Configuracion>.Failure("Configuración no encontrada.");

                return OperationResult<Configuracion>.Success(configuracion);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:GetByIdError"] ??
                                   "Ocurrió un error al obtener la configuración por ID.";

                _logger.Error(ex, $"{errorMessage} para el ID: {idConfiguracion}");
                return OperationResult<Configuracion>.Failure(errorMessage);
            }
        }

        #endregion
    }
}
