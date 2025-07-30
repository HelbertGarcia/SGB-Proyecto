using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Libro;

namespace SGB.Presentation.Services
{
    public interface ILibroHttpService
    {
        Task<List<LibroModel>> ObtenerTodos();
        Task<LibroModel> ObtenerPorId(int id);
        Task<ApiResponse<LibroDto>> Crear(AddLibroDto dto);
        Task<ApiResponse<object>> Actualizar(int id, UpdateLibroDto dto);
        Task<bool> Eliminar(int id);
    }
}
