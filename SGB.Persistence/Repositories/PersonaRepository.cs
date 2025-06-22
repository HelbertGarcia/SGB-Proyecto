using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;
using SGB.Domain.Repository;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;

namespace SGB.Persistence.Repositories
{
    public class PersonaRepository : BaseRepository<Persona>, IPersonaRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<PersonaRepository> _logger;
        private readonly IConfiguration _configuration;

        public PersonaRepository(SGBContext context,
                                 ILogger<PersonaRepository> logger,
                                 IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }


        #region "Métodos Heredados Sobrescritos"

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return new OperationResult { Success = false, Message = "ID inválido." };

                var usuario = await Entity.FindAsync(id);

                if (usuario == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = _configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Usuario no encontrado."
                    };
                }

                usuario.EstaActivo = false;


                return await base.UpdateAsync(usuario);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Usuarios:Deactivate"] ?? "Ocurrió un error al desactivar el usuario.";
                _logger.LogError(ex, "{ErrorMessage} para el ID de usuario: {UsuarioID}", errorMessage, id);

                return new OperationResult
                {
                    Success = false,
                    Message = errorMessage
                };
            }
        }

        #endregion

        #region "Métodos Propios"

        public async Task<OperationResult> ObtenerPorEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El correo electrónico no puede estar vacío."
                };
            }

            try
            {
                var usuario = await Entity.AsNoTracking()
                                          .FirstOrDefaultAsync(u => u.Email == email);

                return new OperationResult
                {
                    Success = true,
                    Data = usuario
                };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Usuarios:GetByEmail"] ?? "Ocurrió un error al buscar el usuario por email.";
                _logger.LogError(ex, "{ErrorMessage} para el email: {Email}", errorMessage, email);

                return new OperationResult
                {
                    Success = false,
                    Message = errorMessage
                };
            }
        }

        public async Task<OperationResult> BuscarPorIdAsync(int id)
        {
            if (id <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El ID del usuario no puede ser menor o igual a cero."
                };
            }

            try
            {
                var usuario = await Entity.AsNoTracking()
                                          .FirstOrDefaultAsync(u => u.IdRol == id);

                return new OperationResult
                {
                    Success = true,
                    Data = usuario
                };
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Usuarios:GetById"] ?? "Error al obtener usuario por ID.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return await Entity.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del email: {Email}", email);
                return false;
            }
        }

        public async Task<OperationResult> BuscarPorRolAsync(int idRol)
        {
            if (idRol <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "ID de rol inválido."
                };
            }

            try
            {
                var usuarios = await Entity
                    .Where(u => u.IdRol == idRol && u.EstaActivo)
                    .AsNoTracking()
                    .ToListAsync();

                return new OperationResult { Success = true, Data = usuarios };
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Usuarios:GetByRol"] ?? "Error al buscar usuarios por rol.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        public async Task<OperationResult> ObtenerTodosActivosAsync()
        {
            try
            {
                var usuarios = await Entity
                    .Where(u => u.EstaActivo)
                    .AsNoTracking()
                    .ToListAsync();

                return new OperationResult { Success = true, Data = usuarios };
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Usuarios:GetActivos"] ?? "Error al obtener usuarios activos.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        public async Task<OperationResult> ActivarCuentaAsync(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "ID de usuario inválido."
                };
            }

            try
            {
                var usuario = await Entity.FindAsync(idUsuario);

                if (usuario == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = _configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Usuario no encontrado."
                    };
                }

                usuario.EstaActivo = true;


                return await base.UpdateAsync(usuario);
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Usuarios:ActivarCuenta"] ?? "Error al activar cuenta de usuario.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        #endregion
    }


}

