using SGB.Application.Base;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.ILibroServices
{

    public interface ICategoriaService : IBaseService<AddCategoriaDto, UpdateCategoriaDto, CategoriaDto>
    {
    }
}