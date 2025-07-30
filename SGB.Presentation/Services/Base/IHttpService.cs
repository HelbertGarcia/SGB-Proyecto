using SGB.Application.Wrappers;

namespace SGB.Presentation.Services.Base
{
    public interface IHttpService
    {
        Task<ApiResponse<T>> GetAsync<T>(string uri);
        Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string uri, TRequest data);
        Task<ApiResponse<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string uri, TRequest data);
        Task<ApiResponse<bool>> DeleteAsync(string uri);
    }
}
