using SGB.Application.Wrappers;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Api.Contracts.Service.IConfiguracionService;

namespace SGB.Api.Handlers
{
    public class ConfiguracionHandler : IConfiguracionHandler
    {
        private readonly IConfiguracionService _configuracionService;

        public ConfiguracionHandler(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        public async Task<ApiResponse<IEnumerable<ConfiguracionDto>>> GetAllAsync()
        {
            var result = await _configuracionService.GetAllAsync();
            return result.IsSuccess
                ? ApiResponseFactory.Success(result.Data, "Configuraciones obtenidas correctamente.")
                : ApiResponseFactory.Fail<IEnumerable<ConfiguracionDto>>(result.Message);
        }

        public async Task<ApiResponse<ConfiguracionDto>> GetByIdAsync(int id)
        {
            var result = await _configuracionService.GetByIdAsync(id);
            return result.IsSuccess && result.Data != null
                ? ApiResponseFactory.Success(result.Data, "Configuración obtenida correctamente.")
                : ApiResponseFactory.Fail<ConfiguracionDto>(result.Message ?? "Configuración no encontrada.");
        }

        public async Task<ApiResponse<ConfiguracionDto>> GetByNameAsync(string name)
        {
            var result = await _configuracionService.ObtenerPorNombreAsync(name);
            return result.IsSuccess && result.Data != null
                ? ApiResponseFactory.Success(result.Data, "Configuración obtenida correctamente.")
                : ApiResponseFactory.Fail<ConfiguracionDto>("Configuración no encontrada por nombre.");
        }

        public async Task<ApiResponse<ConfiguracionDto>> CreateAsync(AddConfiguracionDto dto)
        {
            var result = await _configuracionService.AddAsync(dto);
            return result.IsSuccess
                ? ApiResponseFactory.Success(result.Data, "Configuración creada exitosamente.")
                : ApiResponseFactory.Fail<ConfiguracionDto>(result.Message);
        }

        public async Task<ApiResponse<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto)
        {
            var result = await _configuracionService.UpdateAsync(id, dto);
            return result.IsSuccess
                ? ApiResponseFactory.Success(result.Data, "Configuración actualizada correctamente.")
                : ApiResponseFactory.Fail<ConfiguracionDto>(result.Message);
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var result = await _configuracionService.DeleteAsync(id);
            return result.IsSuccess
                ? ApiResponseFactory.Success<object>(null, "Configuración eliminada correctamente.")
                : ApiResponseFactory.Fail<object>(result.Message);
        }
    }
}
