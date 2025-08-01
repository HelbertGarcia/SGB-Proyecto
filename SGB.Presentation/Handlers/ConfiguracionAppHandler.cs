using SGB.Presentation.Models;
using SGB.Application.Wrappers;
using SGB.Presentation.Service;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Presentation.Handlers
{
    public class ConfiguracionAppHandler : IConfiguracionAppHandler
    {
        private readonly IConfiguracionHttpService _httpService;

        public ConfiguracionAppHandler(IConfiguracionHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<ConfiguracionModel>> GetAllAsync()
        {
            return await _httpService.GetAllAsync();
        }

        public async Task<ConfiguracionModel?> GetByIdAsync(int id)
        {
            return await _httpService.GetByIdAsync(id);
        }

        public async Task<ApiResponse<object>> CreateAsync(AddConfiguracionDto dto)
        {
            return await _httpService.CreateAsync(dto);
        }

        public async Task<ApiResponse<object>> UpdateAsync(UpdateConfiguracionDto dto)
        {
            return await _httpService.UpdateAsync(dto);
        }
    }
}
