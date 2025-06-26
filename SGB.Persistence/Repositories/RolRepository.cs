using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class RolRepository : BaseRepository<Rol>, IRolRepository
    {
        private readonly ILogger<RolRepository> _logger;
        private readonly IConfiguration _configuration;

        public RolRepository(SGBContext context,
                             ILoggerFactory loggerFactory,
                             IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<RolRepository>();
        }

        #region "Métodos Heredados Sobrescritos"

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            return await DesactivarRolAsync(id);
        }

        #endregion

        #region "Implementación de IRolRepository"

        public async Task<OperationResult> ObtenerPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El nombre del rol no puede estar vacío." });
            }
            return await base.FindByConditionAsync(r => r.Nombre == nombre);
        }

        public async Task<OperationResult> ObtenerTodosActivosAsync()
        {
            return await base.FindByConditionAsync(r => r.EstaActivo);
        }

        public async Task<OperationResult> ActivarRolAsync(int idRol)
        {
            return await CambiarEstadoRolAsync(idRol, true);
        }

        public async Task<OperationResult> DesactivarRolAsync(int idRol)
        {
            return await CambiarEstadoRolAsync(idRol, false);
        }

        private async Task<OperationResult> CambiarEstadoRolAsync(int idRol, bool estado)
        {
            if (idRol <= 0)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "ID de rol inválido." });
            }
            try
            {
                var rol = await Entity.FindAsync(idRol);
                if (rol == null)
                {
                    return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] };
                }

                rol.EstaActivo = estado;
                return await base.UpdateAsync(rol);
            }
            catch (Exception ex)
            {
                var action = estado ? "Activar" : "Desactivar";
                var errorMessage = _configuration[$"ErrorMessages:Roles:{action}Rol"] ?? $"Error al {action.ToLower()} el rol.";
                _logger.LogError(ex, "{ErrorMessage} para el ID: {RolID}", errorMessage, idRol);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }
        #endregion
    }
}