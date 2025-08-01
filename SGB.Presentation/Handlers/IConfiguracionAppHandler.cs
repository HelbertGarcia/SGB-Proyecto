using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Application.Dtos.DashboardDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models;

namespace SGB.Presentation.Handlers
{ 
    public interface IConfiguracionAppHandler
    {
        Task<List<ConfiguracionModel>> GetAllAsync();
        Task<ConfiguracionModel?> GetByIdAsync(int id);
        Task<ApiResponse<object>> CreateAsync(AddConfiguracionDto dto);
        Task<ApiResponse<object>> UpdateAsync(UpdateConfiguracionDto dto);
        Task<DashboardDto?> GetDashboardAsync();
    }
}
