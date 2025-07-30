using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Categoria;

namespace SGB.Presentation.Services
{
    public interface ICategoriaHttpService
    {
        Task<List<CategoriaModel>> ObtenerTodas();
        Task<CategoriaModel> ObtenerPorId(int id);
        Task<ApiResponse<CategoriaDto>> Crear(AddCategoriaDto dto);
        Task<ApiResponse<object>> Actualizar(int id, UpdateCategoriaDto dto);
        Task<ApiResponse<bool>> Eliminar(int id);
    }
}
