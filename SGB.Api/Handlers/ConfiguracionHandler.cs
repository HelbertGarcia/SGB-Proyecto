using SGB.Api.Contracts.Service.IConfiguracionService;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Application.Wrappers;
using SGB.Domain.Base;

namespace SGB.Api.Handlers
{
    public class ConfiguracionHandler : IConfiguracionHandler
    {
        private readonly IConfiguracionService _configuracionService;

        public ConfiguracionHandler(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        private ApiResponse<T> Respuesta<T>(OperationResult<T> result, string mensajeExito)
        {
            return result.IsSuccess && result.Data != null
                ? ApiResponseFactory.Success(result.Data, mensajeExito)
                : ApiResponseFactory.Fail<T>(result.Message ?? "Ocurrió un error.");
        }

        public async Task<ApiResponse<IEnumerable<ConfiguracionDto>>> GetAllAsync()
        {
            var result = await _configuracionService.GetAllAsync();
            return Respuesta(result, "Configuraciones obtenidas correctamente.");
        }

        public async Task<ApiResponse<ConfiguracionDto>> GetByIdAsync(int id)
        {
            var result = await _configuracionService.GetByIdAsync(id);
            return Respuesta(result, "Configuración obtenida correctamente.");
        }

        public async Task<ApiResponse<ConfiguracionDto>> GetByNameAsync(string name)
        {
            var result = await _configuracionService.ObtenerPorNombreAsync(name);
            return Respuesta(result, "Configuración obtenida correctamente.");
        }

        public async Task<ApiResponse<ConfiguracionDto>> CreateAsync(AddConfiguracionDto dto)
        {
            var result = await _configuracionService.AddAsync(dto);
            return Respuesta(result, "Configuración creada exitosamente.");
        }

        public async Task<ApiResponse<ConfiguracionDto>> UpdateAsync(int id, UpdateConfiguracionDto dto)
        {
            var result = await _configuracionService.UpdateAsync(id, dto);
            return Respuesta(result, "Configuración actualizada correctamente.");
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var result = await _configuracionService.DeleteAsync(id);
            return result.IsSuccess
                ? ApiResponseFactory.Success<object>(null, "Configuración deshabilitada correctamente.")
                : ApiResponseFactory.Fail<object>(result.Message ?? "No se pudo deshabilitada la configuración.");
        }
    }
}
