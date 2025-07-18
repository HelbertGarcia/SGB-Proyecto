using SGB.Domain.Base;
using System.Linq.Expressions;

namespace SGB.Application.Contracts.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<OperationResult<T>> GetByIdAsync(int id);
        Task<OperationResult<IEnumerable<T>>> GetAllAsync();
        Task<OperationResult<T>> AddAsync(T entity);
        Task<OperationResult<T>> UpdateAsync(T entity);
        Task<OperationResult<bool>> DeleteAsync(int id);
        Task<OperationResult<IEnumerable<T>>> FindByConditionAsync(Expression<Func<T, bool>> filter);
    }
}
