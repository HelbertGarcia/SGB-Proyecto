using SGB.Application.Wrappers;
using System.Text.Json;

namespace SGB.Presentation.Services.Base
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpService> _logger;


        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };



        public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }


        private HttpClient CreateClient() => _httpClientFactory.CreateClient("ApiSGB");

        public async Task<ApiResponse<T>> GetAsync<T>(string uri)
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {error}",
                        Data = default
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonOptions);
                return result ?? new ApiResponse<T> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir GET {Uri}", uri);
                return new ApiResponse<T> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var client = CreateClient();
                var response = await client.PostAsJsonAsync(uri, data);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<TResponse>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {error}"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>(_jsonOptions);
                return result ?? new ApiResponse<TResponse> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir POST {Uri}", uri);
                return new ApiResponse<TResponse> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var client = CreateClient();
                var response = await client.PutAsJsonAsync(uri, data);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<TResponse>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {error}"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>(_jsonOptions);
                return result ?? new ApiResponse<TResponse> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir PUT {Uri}", uri);
                return new ApiResponse<TResponse> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string uri)
        {
            try
            {
                var client = CreateClient();
                var response = await client.DeleteAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {error}",
                        Data = default
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonOptions);
                return result ?? new ApiResponse<T> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir DELETE {Uri}", uri);
                return new ApiResponse<T> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }




    }
}
