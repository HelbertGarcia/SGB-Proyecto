using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Categoria;
using SGB.Presentation.Services.Base;

namespace SGB.Presentation.Services
{
    public class CategoriaHttpService : ICategoriaHttpService
    {
        private readonly IHttpService _httpService;

        public CategoriaHttpService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<CategoriaModel>> ObtenerTodas()
        {
            var response = await _httpService.GetAsync<List<CategoriaModel>>("api/Categoria/GetAllCategorias");
            return (response != null && response.IsSuccess) ? response.Data : new List<CategoriaModel>();
        }

        public async Task<CategoriaModel> ObtenerPorId(int id)
        {
            var response = await _httpService.GetAsync<CategoriaModel>($"api/Categoria/GetCategoriaById/{id}");
            return (response != null && response.IsSuccess) ? response.Data : null;
        }

        public async Task<ApiResponse<CategoriaDto>> Crear(AddCategoriaDto dto)
        {
            return await _httpService.PostAsJsonAsync<AddCategoriaDto, CategoriaDto>("api/Categoria/AddCategoria", dto);
        }

        public async Task<ApiResponse<object>> Actualizar(int id, UpdateCategoriaDto dto)
        {
            return await _httpService.PutAsJsonAsync<UpdateCategoriaDto, object>($"api/Categoria/UpdateCategoria/{id}", dto);
        }

        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            return await _httpService.DeleteAsync($"api/Categoria/DisableCategoria/{id}");
        }
    }
}
