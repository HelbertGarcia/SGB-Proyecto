using SGB.Domain.Base;

namespace SGB.Api.Base
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