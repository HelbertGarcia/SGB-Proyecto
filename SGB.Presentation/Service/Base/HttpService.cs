using SGB.Application.Wrappers;

namespace SGB.Presentation.Services.Base
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("ApiSGB");

        public async Task<ApiResponse<T>> GetAsync<T>(string uri)
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetFromJsonAsync<ApiResponse<T>>(uri);
                return response ?? new ApiResponse<T> { IsSuccess = false, Message = "Respuesta nula del servidor." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T> { IsSuccess = false, Message = $"Excepción de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var client = CreateClient();
                var response = await client.PostAsJsonAsync(uri, data);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
                return result ?? new ApiResponse<TResponse> { IsSuccess = false, Message = "Respuesta nula al enviar POST." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TResponse> { IsSuccess = false, Message = $"Error en POST: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var client = CreateClient();
                var response = await client.PutAsJsonAsync(uri, data);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
                return result ?? new ApiResponse<TResponse> { IsSuccess = false, Message = "Respuesta nula al enviar PUT." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TResponse> { IsSuccess = false, Message = $"Error en PUT: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string uri)
        {
            try
            {
                var client = CreateClient();
                var response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool> { IsSuccess = true, Data = true, Message = "Eliminado correctamente." };
                }
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                return result ?? new ApiResponse<bool> { IsSuccess = false, Message = $"Error al eliminar: {response.ReasonPhrase}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = $"Excepción en DELETE: {ex.Message}" };
            }
        }
    }
}
