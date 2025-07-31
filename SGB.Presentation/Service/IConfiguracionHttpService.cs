using SGB.Presentation.Models;
using SGB.Application.Wrappers;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Presentation.Service
{
    public interface IConfiguracionHttpService
    {
        Task<List<ConfiguracionModel>> GetAllAsync();
        Task<ConfiguracionModel?> GetByIdAsync(int id);
        Task<ApiResponse<object>> CreateAsync(AddConfiguracionDto dto);
        Task<ApiResponse<object>> UpdateAsync(int id, UpdateConfiguracionDto dto);
    }
}
