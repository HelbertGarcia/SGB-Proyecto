using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Domain.Entities.Prestamos;
using SGB.Domain.Repository;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<PenalizacionRepository> _logger;
        private readonly IConfiguration _configuration;

        public PenalizacionRepository(SGBContext context, 
                                  ILogger<PenalizacionRepository> logger,
                                  IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }



        public async Task<OperationResult> GetActivePenalizacionesAsync()
        {
            var result = new OperationResult();

            try
            {
                var now = DateTime.Now;

                var penalizacionesActivas = await _context.Penalizacion
                    .AsNoTracking()
                    .Where(p => p.EstaActivo && p.FechaInicio <= now && p.FechaFin >= now)
                    .ToListAsync();

                result.Success = true;
                result.Data = penalizacionesActivas; 
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorPenalizacionRepository:GetActivePenalizacionesError"]
                                 ?? "Error al obtener penalizaciones activas.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }



        public async Task<OperationResult> GetMotivosPenalizacionesPorUsuarioAsync(int usuarioId)
        {
            var result = new OperationResult();

            try
            {
                var motivos = await _context.Penalizacion
                    .AsNoTracking()
                    .Where(p => p.Id == usuarioId)
                    .Select(p => new
                    {
                        p.Id,
                        p.Motivo 
                    })
                    .ToListAsync();

                result.Success = true;
                result.Data = motivos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorPenalizacionRepository:GetMotivosPorUsuarioError"]
                                 ?? "Error al obtener los motivos de penalización del usuario.";
                _logger.LogError(ex, result.Message + " UsuarioId: {UsuarioId}", usuarioId);
            }

            return result;
        }




        public override async Task<OperationResult> AddAsync(Penalizacion entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Intento de agregar una penalización nula.");
                return new OperationResult { Success = false, Message = "La penalización no puede ser nula." };
            }

            try
            {
                var result = await base.AddAsync(entity);
                if (result.Success)
                {
                    _logger.LogInformation("Penalización agregada exitosamente con ID: {Id}", entity.Id);
                }
                return result;
            }
            catch (Exception ex)
            {
                var message = _configuration["ErrorPenalizacionRepository:AddError"] ?? "Error al agregar la penalización.";
                _logger.LogError(ex, message);
                return new OperationResult { Success = false, Message = message };
            }
        }

        public override async Task<OperationResult> UpdateAsync(Penalizacion entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Intento de actualizar una penalización nula.");
                return new OperationResult { Success = false, Message = "La penalización no puede ser nula." };
            }

            try
            {
                var result = await base.UpdateAsync(entity);
                if (result.Success)
                {
                    _logger.LogInformation("Penalización actualizada correctamente con ID: {Id}", entity.Id);
                }
                return result;
            }
            catch (Exception ex)
            {
                var message = _configuration["ErrorPenalizacionRepository:UpdateError"] ?? "Error al actualizar la penalización.";
                _logger.LogError(ex, message + " Penalización ID: {Id}", entity?.Id);
                return new OperationResult { Success = false, Message = message };
            }
        }

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Intento de eliminar penalización con ID inválido: {Id}", id);
                return new OperationResult { Success = false, Message = "El ID no es válido." };
            }

            try
            {
                var result = await base.DeleteAsync(id);
                if (result.Success)
                {
                    _logger.LogInformation("Penalización eliminada correctamente. ID: {Id}", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                var message = _configuration["ErrorPenalizacionRepository:DeleteError"] ?? "Error al eliminar la penalización.";
                _logger.LogError(ex, message + " ID: {Id}", id);
                return new OperationResult { Success = false, Message = message };
            }
        }

        public override async Task<Penalizacion> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID inválido para GetByIdAsync en penalización: {Id}", id);
                return null;
            }

            try
            {
                var entity = await base.GetByIdAsync(id);
                if (entity != null)
                {
                    _logger.LogInformation("Penalización encontrada. ID: {Id}", id);
                }
                else
                {
                    _logger.LogInformation("No se encontró penalización con ID: {Id}", id);
                }
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener penalización con ID: {Id}", id);
                return null;
            }
        }

        public override async Task<OperationResult> GetAllAsync(Expression<Func<Penalizacion, bool>> filter = null)
        {
            var result = new OperationResult();

            try
            {
                result = await base.GetAllAsync(filter);
                _logger.LogInformation("Se recuperaron penalizaciones con filtro. ¿Filtro aplicado?: {Filtro}", filter != null);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorPenalizacionRepository:GetAllFilteredError"]
                                 ?? "Error al obtener penalizaciones con filtro.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        public override async Task<IEnumerable<Penalizacion>> GetAllAsync()
        {
            try
            {
                var lista = await base.GetAllAsync();
                _logger.LogInformation("Se recuperaron todas las penalizaciones. Total: {Count}", lista.Count());
                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las penalizaciones.");
                return Enumerable.Empty<Penalizacion>();
            }
        }


    }
}
