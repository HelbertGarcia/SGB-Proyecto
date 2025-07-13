using SGB.Application.Base;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.ILibroServices
{
    public interface ILibroService : IBaseService<AddLibroDto, UpdateLibroDto, LibroDto>
    {
        Task<OperationResult<LibroDto>> BuscarPorIsbnAsync(string isbn);
        Task<OperationResult<IEnumerable<LibroDto>>> BuscarLibrosAsync(string terminoBusqueda);
    }
}
