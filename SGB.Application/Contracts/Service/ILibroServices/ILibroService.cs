using SGB.Application.Base;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.ILibroServices
{
    public interface ILibroService : IBaseService<AddLibroDto, UpdateLibroDto>
    {
        Task<OperationResult> BuscarPorIsbnAsync(string isbn);
        Task<OperationResult> BuscarLibrosAsync(string terminoBusqueda);
    }
}
