using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        //-- CAMBIO: Ahora devuelve un OperationResult que contiene la entidad T.
        Task<OperationResult<T>> GetByIdAsync(int id);

        //-- CAMBIO: Ahora devuelve un OperationResult que contiene una lista de entidades.
        Task<OperationResult<IEnumerable<T>>> GetAllAsync();

        //-- CAMBIO: Devuelve un OperationResult que contiene la entidad recién creada.
        Task<OperationResult<T>> AddAsync(T entity);

        //-- CAMBIO: Devuelve un OperationResult que contiene la entidad actualizada.
        Task<OperationResult<T>> UpdateAsync(T entity);

        //-- CAMBIO: Devuelve un OperationResult que contiene un booleano (true si se eliminó).
        Task<OperationResult<bool>> DeleteAsync(int id);

        //-- CAMBIO: Ahora devuelve un OperationResult que contiene una lista de entidades.
        Task<OperationResult<IEnumerable<T>>> FindByConditionAsync(Expression<Func<T, bool>> filter);
    }
}