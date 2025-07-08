using SGB.Application.Base;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.ILibroServices
{
    /// <summary>
    /// Defines the contract for category-related business operations.
    /// Inherits all standard CRUD operations from IBaseService.
    /// </summary>
    public interface ICategoriaService : IBaseService<AddCategoriaDto, UpdateCategoriaDto>
    {
    }
}