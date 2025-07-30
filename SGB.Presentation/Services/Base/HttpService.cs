using SGB.Application.Wrappers;

namespace SGB.Presentation.Services.Base
{
    public class HttpService: IHttpService
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
                return await client.GetFromJsonAsync<ApiResponse<T>>(uri);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T> { IsSuccess = false, Message = $"Excepción de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync(uri, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
        }

        public async Task<ApiResponse<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync(uri, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string uri)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                // For DELETE, 204 No Content is a success but has no body, so we build our own response
                return new ApiResponse<bool> { IsSuccess = true };
            }
            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        }
    }
}
