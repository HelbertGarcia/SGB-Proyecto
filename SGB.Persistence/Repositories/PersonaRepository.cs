using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class PersonaRepository : BaseRepository<Persona>, IPersonaRepository
    {
        private readonly ILogger<PersonaRepository> _logger;
        private readonly IConfiguration _configuration;

        public PersonaRepository(SGBContext context,
                                 ILoggerFactory loggerFactory,
                                 IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PersonaRepository>();
        }

        #region "Métodos Heredados Sobrescritos"

        #endregion

        #region "Métodos Propios de IPersonaRepository"

        public async Task<OperationResult> ObtenerPorEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El correo electrónico no puede estar vacío." });
            }
            return await base.FindByConditionAsync(p => p.Email == email);
        }

        public async Task<OperationResult> ExisteEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return await Task.FromResult(new OperationResult { Data = false });

            try
            {
                var existe = await Entity.AnyAsync(u => u.Email == email);
                return new OperationResult { Data = existe };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Usuarios:GetByEmail"] ?? "Error al verificar existencia del email.";
                _logger.LogError(ex, "{ErrorMessage}: {Email}", errorMessage, email);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        public async Task<OperationResult> BuscarPorRolAsync(int idRol)
        {
            if (idRol <= 0)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "ID de rol inválido." });
            }
            return await base.FindByConditionAsync(u => u.IdRol == idRol && u.EstaActivo);
        }

        public async Task<OperationResult> ObtenerTodosActivosAsync()
        {
            return await base.FindByConditionAsync(u => u.EstaActivo);
        }

        public async Task<OperationResult> ActivarCuentaAsync(int idUsuario)
        {
            return await CambiarEstadoCuentaAsync(idUsuario, true);
        }

        public async Task<OperationResult> DesactivarCuentaAsync(int idUsuario)
        {
            return await CambiarEstadoCuentaAsync(idUsuario, false);
        }

        private async Task<OperationResult> CambiarEstadoCuentaAsync(int idUsuario, bool estado)
        {
            if (idUsuario <= 0)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "ID de usuario inválido." });
            }
            try
            {
                var usuario = await Entity.FindAsync(idUsuario);
                if (usuario == null)
                {
                    return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] };
                }

                usuario.EstaActivo = estado;
                return await base.UpdateAsync(usuario);
            }
            catch (Exception ex)
            {
                var action = estado ? "activar" : "desactivar";
                var errorMessage = _configuration[$"ErrorMessages:Usuarios:{action}Cuenta"] ?? $"Error al {action} la cuenta.";
                _logger.LogError(ex, "{ErrorMessage} para el ID: {UsuarioID}", errorMessage, idUsuario);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }
        #endregion
    }
}