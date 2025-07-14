
using SGB.Domain.Base;


using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base
{
    public interface IBaseService<TAddDto, TUpdateDto, TDtoDelete , TDto>
    {
        Task<OperationResult<TDto>> AddAsync(TAddDto dto);
        Task<OperationResult<TDto>> UpdateAsync( TUpdateDto dto);
        Task<OperationResult<bool>> DeleteAsync(TDtoDelete dto);

        Task<OperationResult<IEnumerable<TDto>>> GetAllAsync();
        Task<OperationResult<TDto>> GetByIdAsync(int id);
    }
}



