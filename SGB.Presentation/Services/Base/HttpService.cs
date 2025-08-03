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
                var response = await CreateClient().GetAsync(uri);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {content}"
                    };
                }

                var result = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
                return result ?? new ApiResponse<T> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir GET {Uri}", uri);
                return new ApiResponse<T> { IsSuccess = false, Message = $"Excepción: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var response = await CreateClient().PostAsJsonAsync(uri, data);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<TResponse>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {content}"
                    };
                }

                var result = JsonSerializer.Deserialize<ApiResponse<TResponse>>(content, _jsonOptions);
                return result ?? new ApiResponse<TResponse> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir POST {Uri}", uri);
                return new ApiResponse<TResponse> { IsSuccess = false, Message = $"Excepción: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var response = await CreateClient().PutAsJsonAsync(uri, data);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<TResponse>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {content}"
                    };
                }

                var result = JsonSerializer.Deserialize<ApiResponse<TResponse>>(content, _jsonOptions);
                return result ?? new ApiResponse<TResponse> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir PUT {Uri}", uri);
                return new ApiResponse<TResponse> { IsSuccess = false, Message = $"Excepción: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string uri)
        {
            try
            {
                var response = await CreateClient().DeleteAsync(uri);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        Message = $"Error {response.StatusCode}: {content}"
                    };
                }

                var result = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
                return result ?? new ApiResponse<T> { IsSuccess = false, Message = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir DELETE {Uri}", uri);
                return new ApiResponse<T> { IsSuccess = false, Message = $"Excepción: {ex.Message}" };
            }
        }
    }
}

