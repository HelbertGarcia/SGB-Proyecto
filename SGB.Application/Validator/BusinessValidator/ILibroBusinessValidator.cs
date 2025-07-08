using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Validators.BusinessValidators
{
    public interface ILibroBusinessValidator
    {
        Task<OperationResult> ValidateForAddAsync(AddLibroDto dto);
        Task<OperationResult> ValidateForUpdateAsync(int id, UpdateLibroDto dto);
        Task<OperationResult> ValidateForDeleteAsync(int id);
    }
}