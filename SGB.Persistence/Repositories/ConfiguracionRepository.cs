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
            //-- CORRECCIÓN: Se crea el logger con el tipo correcto.
            _logger = loggerFactory.CreateLogger<ConfiguracionRepository>();
        }

        #region "Métodos Heredados Sobrescritos"

        /// <summary>
        /// Sobrescribe el comportamiento de borrado por defecto para que sea un borrado lógico.
        /// </summary>
        public override async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var configuracion = await Entity.FindAsync(id);
                if (configuracion == null)
                {
                    return OperationResult<bool>.Failure("Configuración no encontrada.");
                }

                configuracion.Deshabilitar(); // Llama al método de la entidad para cambiar su estado.

                var updateResult = await base.UpdateAsync(configuracion);

                // Devuelve el resultado de la operación de actualización.
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

        /// <summary>
        /// Busca una configuración específica por su nombre o clave única.
        /// </summary>
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

        #endregion
    }
}