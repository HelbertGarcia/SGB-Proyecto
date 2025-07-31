using SGB.Presentation.Models;
using SGB.Application.Wrappers;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Presentation.Handlers
{ 
    public interface IConfiguracionAppHandler
    {
        Task<List<ConfiguracionModel>> GetAllAsync();
        Task<ConfiguracionModel?> GetByIdAsync(int id);
        Task<ApiResponse<object>> CreateAsync(AddConfiguracionDto dto);
        Task<ApiResponse<object>> UpdateAsync(UpdateConfiguracionDto dto);
    }
}
