using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Categoria;
using SGB.Presentation.Services.Base;

namespace SGB.Presentation.Services
{
    public class CategoriaHttpService: ICategoriaHttpService
    {
        private readonly string _baseUrl = "https://localhost:7299/api/";

        private HttpClient CreateClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            return client;
        }

        public async Task<List<CategoriaModel>> ObtenerTodas()
        {
            using var client = CreateClient();

            var response = await client.GetAsync("Categoria/GetAllCategorias");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaModel>>>();
                if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
            }
            return new List<CategoriaModel>();
        }

        public async Task<CategoriaModel> ObtenerPorId(int id)
        {
            using var client = CreateClient();

            var response = await client.GetAsync($"Categoria/GetCategoriaById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CategoriaModel>>();
                return apiResponse?.Data;
            }
            return null;
        }

        public async Task<ApiResponse<CategoriaDto>> Crear(AddCategoriaDto dto)
        {
            using var client = CreateClient();

            var response = await client.PostAsJsonAsync("Categoria/AddCategoria", dto);

            return await response.Content.ReadFromJsonAsync<ApiResponse<CategoriaDto>>();
        }

        public async Task<ApiResponse<object>> Actualizar(int id, UpdateCategoriaDto dto)
        {
            using var client = CreateClient();

            var response = await client.PutAsJsonAsync($"Categoria/UpdateCategoria/{id}", dto);

            return await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
        }

        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            using var client = CreateClient();

            var response = await client.DeleteAsync($"Categoria/DisableCategoria/{id}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new ApiResponse<bool> { IsSuccess = true };
                }
                return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
            }
            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        }
    }
}
