using SGB.Domain.Base;
using System.Threading.Tasks;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;

namespace SGB.Application.Validators.BusinessValidators
{
    public interface ICategoriaBusinessValidator
    {
        Task<OperationResult> ValidateForAddAsync(AddCategoriaDto dto);
        Task<OperationResult> ValidateForDeleteAsync(int id);
    }
}