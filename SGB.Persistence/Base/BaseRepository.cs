using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Repository;
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


        public virtual async Task<OperationResult> AddAsync(T entity)
        {
            try
            {
                await Entity.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:AddError"];
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        public virtual async Task<OperationResult> UpdateAsync(T entity)
        {
            try
            {
                Entity.Update(entity);
                await _context.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:UpdateError"];
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var entityToDelete = await Entity.FindAsync(id);
                if (entityToDelete == null)
                    return new OperationResult { Success = false, Message = "Entidad no encontrada para eliminar." };

                Entity.Remove(entityToDelete);
                await _context.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:DeleteError"];
                _logger.LogError(ex, "{ErrorMessage} - ID: {Id}", errorMessage, id);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        public virtual async Task<OperationResult> FindByConditionAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                var data = await Entity.Where(filter).AsNoTracking().ToListAsync();
                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:BaseRepository:GetError"];
                _logger.LogError(ex, errorMessage);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await Entity.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entity.AsNoTracking().ToListAsync();
        }
    }
}