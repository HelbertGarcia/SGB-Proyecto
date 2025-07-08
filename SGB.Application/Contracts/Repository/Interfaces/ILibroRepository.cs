using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Application.Dtos;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface ILibroRepository : IBaseRepository<Libro> 
    {
        Task<OperationResult> BuscarPorIsbnAsync(string isbn);
        Task<OperationResult> BuscarPorAutorAsync(string autor);
        Task<OperationResult> BuscarPorTituloAsync(string titulo);
        Task<Libro> ObtenerParaActualizacionAsync(int id);
        Task<OperationResult> ObtenerDetallesDTOPorIdAsync(int id);
        Task<OperationResult> ObtenerTodosConDetallesAsync();
    }
}