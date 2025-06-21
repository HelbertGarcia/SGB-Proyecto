using Microsoft.EntityFrameworkCore;
using SGB.Domain.Base;
using SGB.Domain.Repository;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly SGBContext _context;
        protected DbSet<T> Entity { get; set; }

        public BaseRepository(SGBContext context)
        {
            _context = context;
            Entity = _context.Set<T>();
        }

        public virtual async Task<OperationResult> AddAsync(T entity)
        {
            OperationResult result = new OperationResult();
            try 
            {
                await Entity.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex) {
            result.Success = false;
            result.Message = "Error al agregar la entidad.";
            }
            return result;
        }

        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            OperationResult result = new OperationResult();

            try {
                var entityToDelete = await Entity.FindAsync(id);
                if (entityToDelete != null)
                {
                    Entity.Remove(entityToDelete);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al eliminar la entidad.";
            }
            return result;
        }
        

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entity.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await Entity.FindAsync(id);
        }

        public virtual async Task<OperationResult> UpdateAsync(T entity)
        {
            OperationResult result = new OperationResult();
            try { 
                Entity.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Error al actualizar la entidad."
                };
            }

            return result;
        }

        public virtual async Task<OperationResult> FindByConditionAsync(Expression<Func<T, bool>> filter)
        {
            OperationResult result = new OperationResult();

            try
            {
                var datos = Entity.Where(filter).ToListAsync();

                result.Data = datos;
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Ocurrio un error obteniendo los datos.";
            }

            return result;
        }
    }
}
