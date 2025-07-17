using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
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

        #region "Implementación de IPersonaRepository"

        public async Task<OperationResult<Persona>> ObtenerPorEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return OperationResult<Persona>.Failure("El correo electrónico no puede estar vacío.");
            }

            try
            {
                var persona = await Entity.AsNoTracking()
                                          .FirstOrDefaultAsync(p => p.Email == email);

                // Devuelve la entidad encontrada (o null) dentro de un resultado exitoso.
                return OperationResult<Persona>.Success(persona);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Usuarios:GetByEmail"] ?? "Error al buscar usuario por email.";
                _logger.LogError(ex, "{ErrorMessage}: {Email}", errorMessage, email);
                return OperationResult<Persona>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<bool>> ExisteEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return OperationResult<bool>.Success(false, "El email proporcionado estaba vacío.");

            try
            {
                var existe = await Entity.AnyAsync(u => u.Email == email);
                return OperationResult<bool>.Success(existe);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Usuarios:GetByEmail"] ?? "Error al verificar la existencia del email.";
                _logger.LogError(ex, "{ErrorMessage}: {Email}", errorMessage, email);
                return OperationResult<bool>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<IEnumerable<Persona>>> BuscarPorRolAsync(int idRol)
        {
            if (idRol <= 0)
            {
                return OperationResult<IEnumerable<Persona>>.Failure("ID de rol inválido.");
            }
            // Reutilizamos el método genérico de la clase base.
            return await base.FindByConditionAsync(u => u.IdRol == idRol && u.EstaActivo);
        }

        public async Task<OperationResult<IEnumerable<Persona>>> ObtenerTodosActivosAsync()
        {
            // Reutilizamos el método genérico de la clase base.
            return await base.FindByConditionAsync(u => u.EstaActivo);
        }

        public async Task<OperationResult<bool>> ActivarCuentaAsync(int idUsuario)
        {
            return await CambiarEstadoCuentaAsync(idUsuario, true, "activar");
        }

        public async Task<OperationResult<bool>> DesactivarCuentaAsync(int idUsuario)
        {
            return await CambiarEstadoCuentaAsync(idUsuario, false, "desactivar");
        }

        // Método privado para no repetir código entre Activar y Desactivar.
        private async Task<OperationResult<bool>> CambiarEstadoCuentaAsync(int idUsuario, bool estado, string accion)
        {
            if (idUsuario <= 0)
                return OperationResult<bool>.Failure("ID de usuario inválido.");

            try
            {
                var usuario = await Entity.FindAsync(idUsuario);
                if (usuario == null)
                    return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:ResourceNotFound"]);

                usuario.EstaActivo = estado;
                var repoResult = await base.UpdateAsync(usuario);

                // Devolvemos el resultado booleano de la operación de actualización.
                return OperationResult<bool>.Success(repoResult.IsSuccess, repoResult.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration[$"ErrorMessages:Usuarios:{accion}Cuenta"] ?? $"Error al {accion} la cuenta.";
                _logger.LogError(ex, "{ErrorMessage} para el ID: {UsuarioID}", errorMessage, idUsuario);
                return OperationResult<bool>.Failure(errorMessage);
            }
        }
        #endregion
    }
}