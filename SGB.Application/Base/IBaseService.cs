using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base
{
    public interface IBaseService<TDtoAdd,TDtoUpdate,TDtoDelete>

    {
         Task<OperationResult> AddAsync(TDtoAdd dto);
        Task<OperationResult> UpdateAsync(TDtoUpdate dto);
        Task<OperationResult> DeleteAsync(TDtoDelete dto);

        Task<OperationResult> GetAllAsync();

        Task<OperationResult> GetByIdAsync(int id);
       


    }
}
