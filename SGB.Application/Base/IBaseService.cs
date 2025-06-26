using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base
{
    public interface IBaseService
    {
        Task<OperationResult> AddAsync<T>(T entity) where T : class;

        Task<OperationResult> UpdateAsync<T>(T entity) where T : class;

        Task<OperationResult> DeleteAsync<T>(int id) where T : class;

        Task<T> GetByIdAsync<T>(T id) where T : class;
    }
}
