using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Sobrescribe el borrado por defecto para que sea un borrado lógico.
        /// </summary>
        public override async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            return await DesactivarRolAsync(id);
        }

        #endregion

        #region "Implementación de IRolRepository"

        public async Task<OperationResult<Rol>> ObtenerPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return OperationResult<Rol>.Failure("El nombre del rol no puede estar vacío.");

            try
            {
                var rol = await Entity.AsNoTracking().FirstOrDefaultAsync(r => r.Nombre == nombre);
                return OperationResult<Rol>.Success(rol);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Roles:GetByNombre"];
                _logger.LogError(ex, "{ErrorMessage}: {Nombre}", errorMessage, nombre);
                return OperationResult<Rol>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<IEnumerable<Rol>>> ObtenerTodosActivosAsync()
        {
            // Reutiliza la lógica de la clase base, que ya es robusta.
            return await base.FindByConditionAsync(r => r.EstaActivo);
        }

        public async Task<OperationResult<bool>> ActivarRolAsync(int idRol)
        {
            return await CambiarEstadoRolAsync(idRol, true);
        }

        public async Task<OperationResult<bool>> DesactivarRolAsync(int idRol)
        {
            return await CambiarEstadoRolAsync(idRol, false);
        }

        // Método privado para no repetir código entre Activar y Desactivar.
        private async Task<OperationResult<bool>> CambiarEstadoRolAsync(int idRol, bool estado)
        {
            if (idRol <= 0)
                return OperationResult<bool>.Failure("ID de rol inválido.");

            try
            {
                var rol = await Entity.FindAsync(idRol);
                if (rol == null)
                    return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);

                rol.EstaActivo = estado;
                var repoResult = await base.UpdateAsync(rol);

                return OperationResult<bool>.Success(repoResult.IsSuccess, repoResult.Message);
            }
            catch (Exception ex)
            {
                var action = estado ? "Activar" : "Desactivar";
                var errorMessage = _configuration[$"ErrorMessages:Roles:{action}Rol"] ?? $"Error al {action.ToLower()} el rol.";
                _logger.LogError(ex, "{ErrorMessage} para el ID: {RolID}", errorMessage, idRol);
                return OperationResult<bool>.Failure(errorMessage);
            }
        }

        #endregion
    }
}