using SGB.Application.Wrappers;

using System.Threading.Tasks;

namespace SGB.Presentation.Services.Base
{
    public interface IHttpService
    {
       
        /// Realiza una petición GET a la URI especificada.
       
        Task<ApiResponse<T>> GetAsync<T>(string uri);

      
        /// Realiza una petición POST con datos JSON y recibe una respuesta tipada.
       
        Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string uri, TRequest data);

        
        /// Realiza una petición PUT con datos JSON y recibe una respuesta tipada.
      
        Task<ApiResponse<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string uri, TRequest data);

     
        /// Realiza una petición DELETE a la URI especificada.
        
       // Task<ApiResponse<bool>> DeleteAsync(string uri);
    }
}
