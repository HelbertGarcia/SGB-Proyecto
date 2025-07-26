using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Persistence.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;

namespace SGB.Persistence.Repositories
{
    public class PersonaRepository : BaseRepository<Domain.Base.Usuario>, IPersonaRepository
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

        public async Task<OperationResult<Domain.Base.Usuario>> ObtenerPorEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return OperationResult<Domain.Base.Usuario>.Failure("El correo electrónico no puede estar vacío.");
            }

            try
            {
                var persona = await Entity.AsNoTracking()
                                          .FirstOrDefaultAsync(p => p.Email == email);

              
                return OperationResult<Domain.Base.Usuario>.Success(persona);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Usuarios:GetByEmail"] ?? "Error al buscar usuario por email.";
                _logger.LogError(ex, "{ErrorMessage}: {Email}", errorMessage, email);
                return OperationResult<Domain.Base.Usuario>.Failure(errorMessage);
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

        public async Task<OperationResult<IEnumerable<Domain.Base.Usuario>>> BuscarPorRolAsync(int idRol)
        {
            if (idRol <= 0)
            {
                return OperationResult<IEnumerable<Domain.Base.Usuario>>.Failure("ID de rol inválido.");
            }
           
            return await base.FindByConditionAsync(u => u.IdRol == idRol && u.EstaActivo);
        }

        public async Task<OperationResult<IEnumerable<Domain.Base.Usuario>>> ObtenerTodosActivosAsync()
        {
            
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

                
                return OperationResult<bool>.Success(repoResult.IsSuccess, repoResult.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration[$"ErrorMessages:Usuarios:{accion}Cuenta"] ?? $"Error al {accion} la cuenta.";
                _logger.LogError(ex, "{ErrorMessage} para el ID: {UsuarioID}", errorMessage, idUsuario);
                return OperationResult<bool>.Failure(errorMessage);
            }
        }

      
      
        Task<OperationResult<bool>> IPersonaRepository.UpdateAsync(UsuarioDto usuario)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<UsuarioDto>>> SearchAsync(string termino)
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<Domain.Base.Usuario>> IPersonaRepository.AddAsync(UsuarioDto usuarioEntity)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<UsuarioDto>>> ObtenerTodosConDetallesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Base.Usuario> ObtenerParaActualizacionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task BuscarPorCorreoAsync(object correo)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}