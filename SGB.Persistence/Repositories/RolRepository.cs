using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;

namespace SGB.Persistence.Repositories
{
    public class RolRepository : BaseRepository<Rol>, IRolRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<RolRepository> _logger;
        private readonly IConfiguration _configuration;

        public RolRepository(SGBContext context,
                             ILogger<RolRepository> logger,
                             IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        #region "Métodos Sobrescritos"

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "ID de rol inválido."
                    };
                }

                var rol = await Entity.FindAsync(id);

                if (rol == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = _configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Rol no encontrado."
                    };
                }

                rol.EstaActivo = false;
               

                return await base.UpdateAsync(rol);
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Roles:Deactivate"] ?? "Error al desactivar el rol.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        #endregion

        #region "Métodos Propios"

        public async Task<OperationResult> ObtenerPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El nombre del rol no puede estar vacío."
                };
            }

            try
            {
                var rol = await Entity.AsNoTracking()
                                      .FirstOrDefaultAsync(r => r.Nombre == nombre);

                return new OperationResult
                {
                    Success = true,
                    Data = rol
                };
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Roles:GetByNombre"] ?? "Error al buscar rol por nombre.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        public async Task<OperationResult> ObtenerTodosActivosAsync()
        {
            try
            {
                var roles = await Entity
                    .Where(r => r.EstaActivo)
                    .AsNoTracking()
                    .ToListAsync();

                return new OperationResult
                {
                    Success = true,
                    Data = roles
                };
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Roles:GetActivos"] ?? "Error al obtener roles activos.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        public async Task<OperationResult> ActivarRolAsync(int idRol)
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
                var rol = await Entity.FindAsync(idRol);

                if (rol == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = _configuration["ErrorMessages:Global:ResourceNotFound"] ?? "Rol no encontrado."
                    };
                }

                rol.EstaActivo = true;
               

                return await base.UpdateAsync(rol);
            }
            catch (Exception ex)
            {
                var msg = _configuration["ErrorMessages:Roles:Activate"] ?? "Error al activar el rol.";
                _logger.LogError(ex, msg);
                return new OperationResult { Success = false, Message = msg };
            }
        }

        #endregion
    }
}


















