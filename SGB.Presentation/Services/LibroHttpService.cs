using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Libro;

namespace SGB.Presentation.Services
{
    public class LibroHttpService: ILibroHttpService
    {
        private readonly string _baseUrl = "https://localhost:7299/api/";

        private HttpClient CreateClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            return client;
        }

        public async Task<List<LibroModel>> ObtenerTodos()
        {
            using var client = CreateClient();
            var response = await client.GetAsync("Libro/GetAllLibros");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<LibroModel>>>();
                if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
            }
            return new List<LibroModel>();
        }

        public async Task<LibroModel> ObtenerPorId(int id)
        {
            using var client = CreateClient();
            var response = await client.GetAsync($"Libro/GetLibroById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LibroModel>>();
                return apiResponse?.Data;
            }
            return null;
        }

        public async Task<ApiResponse<LibroDto>> Crear(AddLibroDto dto)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync("Libro/AddLibro", dto);
            // Devuelve la respuesta completa de la API para que el controlador pueda manejar el éxito o el error.
            return await response.Content.ReadFromJsonAsync<ApiResponse<LibroDto>>();
        }

        public async Task<ApiResponse<object>> Actualizar(int id, UpdateLibroDto dto)
        {
            using var client = CreateClient();
            var response = await client.PutAsJsonAsync($"Libro/UpdateLibro/{id}", dto);
            return await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
        }

        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            using var client = CreateClient();
            var response = await client.DeleteAsync($"Libro/DisableLibro/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<bool> { IsSuccess = true };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        }
    }
}
