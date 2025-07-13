using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Libro;
using System.Threading.Tasks;

namespace SGB.Application.Validators.BusinessValidators
{
    public interface ILibroBusinessValidator
    {
        Task<OperationResult<Categoria>> ValidateForAddAsync(AddLibroDto dto);
        Task<OperationResult<Libro>> ValidateForUpdateAsync(int id, UpdateLibroDto dto);
        Task<OperationResult<bool>> ValidateForDeleteAsync(int id);
    }
}