using SGB.Application.Wrappers;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Api.Handlers
{
    public interface IConfiguracionHandler
    {  
        Task<ApiResponse<object>> DeleteAsync(int id);
        Task<ApiResponse<ConfiguracionDto>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<ConfiguracionDto>>> GetAllAsync();
        Task<ApiResponse<ConfiguracionDto>> GetByNameAsync(string name);
        Task<ApiResponse<ConfiguracionDto>> CreateAsync(AddConfiguracionDto dto);
        Task<ApiResponse<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto);
    }
}
