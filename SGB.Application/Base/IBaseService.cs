using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base
{
    public interface IBaseService<TAddDto, TUpdateDto, TDto>
    {
        Task<OperationResult<TDto>> AddAsync(TAddDto dto);
        Task<OperationResult<TDto>> UpdateAsync(int id, TUpdateDto dto);
        Task<OperationResult<bool>> DeleteAsync(int id);
        Task<OperationResult<IEnumerable<TDto>>> GetAllAsync();
        Task<OperationResult<TDto>> GetByIdAsync(int id);
    }
}
