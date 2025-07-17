using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class ConfiguracionRepository : BaseRepository<Configuracion>, IConfiguracionRepository
    {
        private readonly ILogger<ConfiguracionRepository> _logger;
        private readonly IConfiguration _configuration;

        public ConfiguracionRepository(SGBContext context,
                                       ILoggerFactory loggerFactory,
                                       IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<ConfiguracionRepository>();
        }

        #region "Métodos Heredados Sobrescritos"
        public override async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var configuracion = await Entity.FindAsync(id);
                if (configuracion == null)
                {
                    return OperationResult<bool>.Failure("Configuración no encontrada.");
                }

                configuracion.Deshabilitar(); 

                var updateResult = await base.UpdateAsync(configuracion);

               
                return OperationResult<bool>.Success(updateResult.IsSuccess, updateResult.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:DeleteError"] ?? "Ocurrió un error al desactivar la configuración.";
                _logger.LogError(ex, "{ErrorMessage} para el ID: {ConfigID}", errorMessage, id);
                return OperationResult<bool>.Failure(errorMessage);
            }
        }

        #endregion

        #region "Implementación de IConfiguracionRepository"
        public async Task<OperationResult<Configuracion>> ObtenerPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return OperationResult<Configuracion>.Failure("El nombre de la configuración no puede estar vacío.");
            }

            try
            {
                var configuracion = await Entity.AsNoTracking()
                                                .FirstOrDefaultAsync(c => c.Nombre == nombre);

                return OperationResult<Configuracion>.Success(configuracion);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:GetByNameError"] ?? "Ocurrió un error al buscar la configuración por nombre.";
                _logger.LogError(ex, "{ErrorMessage} para el nombre: {Nombre}", errorMessage, nombre);
                return OperationResult<Configuracion>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<Configuracion>> ObtenerPorIdAsync(int idConfiguracion)
        {
            try
            {
                var configuracion = await Entity.AsNoTracking()
                                                .FirstOrDefaultAsync(c => c.IDConfiguracion == idConfiguracion);

                if (configuracion == null)
                    return OperationResult<Configuracion>.Failure("Configuración no encontrada.");

                return OperationResult<Configuracion>.Success(configuracion);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Configuracion:GetByIdError"] ?? "Ocurrió un error al obtener la configuración por ID.";
                _logger.LogError(ex, "{ErrorMessage} para el ID: {ConfigID}", errorMessage, idConfiguracion);
                return OperationResult<Configuracion>.Failure(errorMessage);
            }
        }
        #endregion
    }
}