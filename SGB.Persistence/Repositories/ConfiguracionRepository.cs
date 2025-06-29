using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Persistence.Base;
using SGB.Persistence.Context;


namespace SGB.Persistence.Repositories
{
    public class ConfiguracionRepository : BaseRepository<Configuracion>, IConfiguracionRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<LibroRepository> _logger;
        private readonly IConfiguration _configuration;


        public ConfiguracionRepository(SGBContext context,ILoggerFactory loggerFactory, IConfiguration configuration)
       : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<LibroRepository>();
        }

        #region "Metodos Heredados BaseRepository"
        public async Task<OperationResult> ActualizarConfiguracionAsync(int id, string valor, string descripcion = null, bool? estaActivo = null)
        {
            try
            {
                var configuracion = await Entity.FindAsync(id);
                if (configuracion == null)
                    return new OperationResult { Success = false, Message = "Configuración no encontrada." };

                configuracion.Valor = valor;
                if (descripcion != null)
                    configuracion.Descripcion = descripcion;
                if (estaActivo.HasValue)
                    configuracion.EstaActivo = estaActivo.Value;

                Entity.Update(configuracion);
                await _context.SaveChangesAsync();

                return new OperationResult();
            }
            catch (Exception ex)
            {
                var errorMsg = _configuration["ErrorMessages:ConfiguracionRepository:UpdateError"];
                _logger.LogError(ex, errorMsg);
                return new OperationResult { Success = false, Message = errorMsg };
            }
        }

        public async Task<OperationResult> DeshabilitarConfiguracionAsync(int id)
        {
            try
            {
                var configuracion = await Entity.FindAsync(id);
                if (configuracion == null)
                    return new OperationResult { Success = false, Message = "Configuración no encontrada." };

                configuracion.EstaActivo = false;
                await _context.SaveChangesAsync();

                return new OperationResult();
            }
            catch (Exception ex)
            {
                var errorMsg = _configuration["ErrorMessages:ConfiguracionRepository:DeleteError"];
                _logger.LogError(ex, errorMsg);
                return new OperationResult { Success = false, Message = errorMsg };
            }
        }

        public async Task<OperationResult> ObtenerConfiguracionAsync(int id)
        {
            try
            {
                var configuracion = await Entity.AsNoTracking()
                                                .FirstOrDefaultAsync(c => c.IDConfiguracion == id);
                if (configuracion == null)
                    return new OperationResult { Success = false, Message = "Configuración no encontrada." };

                return new OperationResult { Data = configuracion };
            }
            catch (Exception ex)
            {
                var errorMsg = _configuration["ErrorMessages:ConfiguracionRepository:GetError"];
                _logger.LogError(ex, errorMsg);
                return new OperationResult { Success = false, Message = errorMsg };
            }
        }
        #endregion



        #region "Implementación de IConfiguracionRepository"
        public async Task<OperationResult> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                var result = await Entity.AsNoTracking()
                                         .Where(c => c.Nombre == nombre)
                                         .ToListAsync();

                return new OperationResult { Data = result };
            }
            catch (Exception ex)
            {
                var errorMsg = _configuration["ErrorMessages:ConfiguracionRepository:GetError"];
                _logger.LogError(ex, errorMsg);
                return new OperationResult { Success = false, Message = errorMsg };
            }
        }
        #endregion

    }




}
