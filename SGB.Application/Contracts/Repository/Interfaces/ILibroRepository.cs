using SGB.Api.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;

namespace SGB.Api.Contracts.Repository.Interfaces
{
    public interface ILibroRepository : IBaseRepository<Libro>
    {
        Task<OperationResult<IEnumerable<Libro>>> BuscarPorAutorAsync(string autor);
        Task<OperationResult<IEnumerable<Libro>>> BuscarPorTituloAsync(string titulo);
        Task<OperationResult<Libro>> BuscarPorIsbnAsync(string isbn);
        Task<Libro> ObtenerParaActualizacionAsync(int id);
        Task<OperationResult<LibroDto>> ObtenerDetallesDTOPorIdAsync(int id);
        Task<OperationResult<IEnumerable<LibroDto>>> ObtenerTodosConDetallesAsync();
    }
}