using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Entities.Prestamos;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Persistence.Base;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;



namespace SGB.Persistence.Repositories
{
     public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<PrestamoRepository> _logger;
        private readonly IConfiguration _configuration;


        public PrestamoRepository(SGBContext context, 
                                  ILogger<PrestamoRepository> logger,
                                  IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetFechaVencimientoByPrestamoIdAsync(int prestamoId)
        {
            OperationResult result = new OperationResult(); 
            try
            {
                var fechaVencimiento = await _context.Prestamos
                                                     .AsNoTracking() // No necesitamos rastrear cambios si solo leemos una propiedad
                                                     .Where(p => p.Id == prestamoId)
                                                     .Select(p => (DateTime?)p.FechaVencimiento) // Casteamos a nullable para el caso de no encontrado
                                                     .FirstOrDefaultAsync();

                if (fechaVencimiento.HasValue) 
                {
                    result.Data = fechaVencimiento.Value; 
                    result.Success = true; 
                   
                }
                else
                {
                    result.Success = false;
              
                    result.Message = _configuration["ErrorPrestamoRepository:PrestamoNotFound"] ?? "Préstamo no encontrado.";
                   
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                // Usa la configuración para el mensaje de error, similar a tu ejemplo
                result.Message = _configuration["ErrorPrestamoRepository:GetFechaVencimientoError"] ?? "Error al obtener la fecha de vencimiento del préstamo.";
                _logger.LogError(ex, result.Message + " ID de Préstamo: {PrestamoId}", prestamoId); // Log del error completo
            }

            return result;
        }



        public async Task<OperationResult> GetEstadosPrestamosPorUsuarioAsync(int usuarioId)
        {
            var result = new OperationResult();

            try
            {
                var estados = await _context.Prestamos
                    .AsNoTracking()
                    .Where(p => p.UsuarioId == usuarioId)
                    .Select(p => new
                    {
                        p.Id,
                        Estado = p.Estado.ToString()
                    })
                    .ToListAsync();

                result.Success = true;
                result.Data = estados; // Lista anónima con Id y Estado
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorPrestamoRepository:GetEstadosPorUsuarioError"]
                                 ?? "Error al obtener los estados de préstamos del usuario.";
                _logger.LogError(ex, result.Message + " UsuarioId: {UsuarioId}", usuarioId);
            }

            return result;
        }



        public override async Task<OperationResult> AddAsync(Prestamo entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Intento de agregar un préstamo nulo.");
                return new OperationResult { Success = false, Message = "El préstamo no puede ser nulo." };
            }

            try
            {
                var result = await base.AddAsync(entity);
                if (result.Success)
                {
                    _logger.LogInformation("Préstamo agregado exitosamente. ID: {Id}", entity.Id);
                }
                return result;
            }
            catch (Exception ex)
            {
                var message = _configuration["ErrorPrestamoRepository:AddError"] ?? "Error al agregar el préstamo.";
                _logger.LogError(ex, message);
                return new OperationResult { Success = false, Message = message };
            }
        }

        public override async Task<OperationResult> UpdateAsync(Prestamo entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Intento de actualizar un préstamo nulo.");
                return new OperationResult { Success = false, Message = "El préstamo no puede ser nulo." };
            }

            try
            {
                var result = await base.UpdateAsync(entity);
                if (result.Success)
                {
                    _logger.LogInformation("Préstamo actualizado exitosamente. ID: {Id}", entity.Id);
                }
                return result;
            }
            catch (Exception ex)
            {
                var message = _configuration["ErrorPrestamoRepository:UpdateError"] ?? "Error al actualizar el préstamo.";
                _logger.LogError(ex, message + " Préstamo ID: {Id}", entity?.Id);
                return new OperationResult { Success = false, Message = message };
            }
        }

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Intento de eliminar préstamo con ID inválido: {Id}", id);
                return new OperationResult { Success = false, Message = "El ID del préstamo no es válido." };
            }

            try
            {
                var result = await base.DeleteAsync(id);
                if (result.Success)
                {
                    _logger.LogInformation("Préstamo eliminado correctamente. ID: {Id}", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                var message = _configuration["ErrorPrestamoRepository:DeleteError"] ?? "Error al eliminar el préstamo.";
                _logger.LogError(ex, message + " ID: {Id}", id);
                return new OperationResult { Success = false, Message = message };
            }
        }

        public override async Task<Prestamo> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID inválido para GetByIdAsync en préstamo: {Id}", id);
                return null;
            }

            try
            {
                var entity = await base.GetByIdAsync(id);
                if (entity != null)
                {
                    _logger.LogInformation("Préstamo encontrado. ID: {Id}", id);
                }
                else
                {
                    _logger.LogInformation("No se encontró préstamo con ID: {Id}", id);
                }
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el préstamo con ID: {Id}", id);
                return null;
            }
        }

        public override async Task<OperationResult> GetAllAsync(Expression<Func<Prestamo, bool>> filter = null)
        {
            var result = new OperationResult();

            try
            {
                result = await base.GetAllAsync(filter);
                _logger.LogInformation("Se obtuvieron préstamos filtrados. ¿Filtro aplicado?: {Filtro}", filter != null);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorPrestamoRepository:GetAllFilteredError"]
                                 ?? "Error al obtener préstamos con filtro.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        public override async Task<IEnumerable<Prestamo>> GetAllAsync()
        {
            try
            {
                var prestamos = await base.GetAllAsync();
                _logger.LogInformation("Se recuperaron todos los préstamos. Total: {Count}", prestamos.Count());
                return prestamos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los préstamos.");
                return Enumerable.Empty<Prestamo>();
            }
        }




















    }
}
