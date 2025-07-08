using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base
{
    public interface IBaseService<TAddDto, TUpdateDto>
    {
        Task<OperationResult> AddAsync(TAddDto dto);

        Task<OperationResult> UpdateAsync(int id, TUpdateDto dto);

        Task<OperationResult> DeleteAsync(int id);

        Task<OperationResult> GetAllAsync();

        Task<OperationResult> GetByIdAsync(int id);
    }
}