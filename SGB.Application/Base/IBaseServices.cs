using SGB.Domain.Base;

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