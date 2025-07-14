using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Application.Contracts.Repository;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGB.Persistence.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly SGBContext _context;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        protected readonly DbSet<T> Entity;

        public BaseRepository(SGBContext context, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger($"SGB.Persistence.Base.BaseRepository<{typeof(T).Name}>");
            Entity = _context.Set<T>();
        }

        public virtual async Task<OperationResult<T>> AddAsync(T entity)
        {
            try
            {
                await Entity.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity, "Entidad agregada exitosamente.");
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:AddError"];
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return OperationResult<T>.Failure(errorMessage);
            }
        }

        public virtual async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            try
            {
                Entity.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity, "Entidad actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:UpdateError"];
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return OperationResult<T>.Failure(errorMessage);
            }
        }

        public virtual async Task<OperationResult<T>> DisableAsync(int id)
        {
            try
            {
                var entity = await Entity.FindAsync(id);
                if (entity == null)
                    return OperationResult<T>.Failure("Entidad no encontrada para desactivar.");

                if (entity is IEstaActivo estaActivoEntity)
                {
                    estaActivoEntity.Deshabilitar(); // Se ejecuta el método de dominio
                }
                else
                {
                    return OperationResult<T>.Failure("La entidad no admite desactivación lógica (no implementa IEstaActivo).");
                }

                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity, "Entidad desactivada correctamente.");
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:DisableError"] ?? "Error al desactivar la entidad.";
                _logger.LogError(ex, "{ErrorMessage} - ID: {Id}", errorMessage, id);
                return OperationResult<T>.Failure(errorMessage);
            }
        }


        public virtual async Task<OperationResult<IEnumerable<T>>> FindByConditionAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                var data = await Entity.Where(filter).AsNoTracking().ToListAsync();
                return OperationResult<IEnumerable<T>>.Success(data);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:GetError"];
                _logger.LogError(ex, errorMessage);
                return OperationResult<IEnumerable<T>>.Failure(errorMessage);
            }
        }

        public virtual async Task<OperationResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var data = await Entity.FindAsync(id);
                if (data == null)
                    return OperationResult<T>.Failure("Entidad no encontrada.");

                return OperationResult<T>.Success(data);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:GetError"];
                _logger.LogError(ex, "{ErrorMessage} - ID: {Id}", errorMessage, id);
                return OperationResult<T>.Failure(errorMessage);
            }
        }

        public virtual async Task<OperationResult<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var data = await Entity.AsNoTracking().ToListAsync();
                return OperationResult<IEnumerable<T>>.Success(data);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:GetError"];
                _logger.LogError(ex, errorMessage);
                return OperationResult<IEnumerable<T>>.Failure(errorMessage);
            }
        }
    }
}