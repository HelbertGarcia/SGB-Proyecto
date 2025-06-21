using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Notificaciones;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;

namespace SGB.Persistence.Repositories
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<NotificacionRepository> _logger;
        private readonly IConfiguration _configuration;

        public NotificacionRepository(SGBContext context,ILogger<NotificacionRepository> logger, IConfiguration configuration) : base(context)
        {
            _logger = logger; 
            _context = context;           
            _configuration = configuration;
        }

        public override async Task<OperationResult> AddAsync(Notificacion entity)
        {
            if (entity.IDUsuario <= 0 || string.IsNullOrWhiteSpace(entity.Mensaje) || string.IsNullOrWhiteSpace(entity.TipoNotificacion))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Datos incompletos o inválidos para la notificación."
                };
            }
            entity.FechaCreacion = (entity.FechaCreacion == default) ? DateTime.Now : entity.FechaCreacion;
            entity.FechaEnvio = entity.FechaCreacion;
            var result = await base.AddAsync(entity);
            result.Message = result.Success ? "Notificación registrada correctamente." : "Error al registrar la notificación.";
            return result;
        }



        public override async Task<OperationResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "ID inválido para eliminar la notificación."
                };
            }
            var result = await base.DeleteAsync(id);
            if (!result.Success)
            {
                var entityCheck = await _context.Notificacion.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
                result.Message = (entityCheck == null) ? "No se encontró la notificación con el ID proporcionado para eliminar."  : "Error al eliminar la notificación.";
            }
            else
            {
                result.Message = "Notificación eliminada correctamente.";
            }
            return result;
        }

        public override async Task<OperationResult> UpdateAsync(Notificacion entity)
        {
            if (entity.Id <= 0 || entity.IDUsuario <= 0 ||
                string.IsNullOrWhiteSpace(entity.Mensaje) ||
                string.IsNullOrWhiteSpace(entity.TipoNotificacion))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Datos inválidos o incompletos para actualizar la notificación."
                };
            }
            var existingEntity = await _context.Notificacion.FindAsync(entity.Id);
            if (existingEntity == null)
            {             
                return new OperationResult
                {
                    Success = false,
                    Message = "No se encontró la notificación que se desea actualizar."
                };
            }
            existingEntity.IDUsuario = entity.IDUsuario;
            existingEntity.Mensaje = entity.Mensaje;
            existingEntity.TipoNotificacion = entity.TipoNotificacion;
            existingEntity.FechaEnvio = (entity.FechaEnvio != default) ? entity.FechaEnvio : DateTime.Now;

            try
            {
                _context.Notificacion.Update(existingEntity);
                await _context.SaveChangesAsync();
                return new OperationResult
                {
                    Success = true,
                    Message = "Notificación actualizada correctamente."
                };
            }
            catch
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error al actualizar la notificación."
                };
            }
        }

        public override async Task<OperationResult> FindByConditionAsync(Expression<Func<Notificacion, bool>> filter = null)
        {
            var result = await base.FindByConditionAsync(filter);
            if (!result.Success)
            {
                _logger.LogError("Fallo al obtener notificaciones. Mensaje del repositorio base: {BaseMessage}", result.Message);
            }
            else
            {
                _logger.LogInformation("Notificaciones obtenidas correctamente.");
            }
            return result;
        }


        public override async Task<Notificacion> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return await _context.Notificacion.FindAsync(id);
        }

        public async Task<Dictionary<string, int>> ContarPorTipoAsync(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return new Dictionary<string, int>();
            }
            return await _context.Notificacion.Where(n => n.IDUsuario == idUsuario) .GroupBy(n => n.TipoNotificacion) .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
}

