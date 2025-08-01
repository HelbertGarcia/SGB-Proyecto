using System.Text.Json;
using SGB.Application.Wrappers;
using SGB.Presentation.Models;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Presentation.Service
{
    public class ConfiguracionHttpService : IConfiguracionHttpService
    {
        private readonly HttpClient _httpClient;

        public ConfiguracionHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ConfiguracionModel>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("admin/GetAllConfigurations");
            if (!response.IsSuccessStatusCode)
                return new List<ConfiguracionModel>();
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<ConfiguracionDto>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var dtos = apiResponse?.Data ?? new List<ConfiguracionDto>();
            return dtos.Select(dto => new ConfiguracionModel
            {
                IDConfiguracion = dto.IDConfiguracion,
                Nombre = dto.Nombre,
                Valor = dto.Valor,
                Descripcion = dto.Descripcion,
                FechaCreacion = dto.FechaCreacion,
                EstaActivo = dto.EstaActivo
            }).ToList();
        }

        public async Task<ConfiguracionModel?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"admin/GetConfigurationById?id={id}");
            if (!response.IsSuccessStatusCode)
                return null;
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<ConfiguracionDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var dto = apiResponse?.Data;
            if (dto == null)
                return null;

            return new ConfiguracionModel
            {
                IDConfiguracion = dto.IDConfiguracion,
                Nombre = dto.Nombre,
                Valor = dto.Valor,
                Descripcion = dto.Descripcion,
                FechaCreacion = dto.FechaCreacion,
                EstaActivo = dto.EstaActivo
            };
        }

        public async Task<ApiResponse<object>> CreateAsync(AddConfiguracionDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("admin/AddConfiguration", dto);
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
                return new ApiResponse<object> { IsSuccess = false, Message = "Respuesta vacía de la API" };
            return JsonSerializer.Deserialize<ApiResponse<object>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new ApiResponse<object> { IsSuccess = false, Message = "Error desconocido" };
        }

        public async Task<ApiResponse<object>> UpdateAsync(UpdateConfiguracionDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"admin/UpdateConfiguration?id={dto.IDConfiguracion}", dto);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new ApiResponse<object> { IsSuccess = false, Message = $"Error: {response.StatusCode}" };
            if (string.IsNullOrWhiteSpace(content))
                return new ApiResponse<object> { IsSuccess = false, Message = "Respuesta vacía de la API" };
            var deserialized = JsonSerializer.Deserialize<ApiResponse<ConfiguracionDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (deserialized?.Data == null)
                return new ApiResponse<object> { IsSuccess = false, Message = "No se pudo deserializar la respuesta." };
            return new ApiResponse<object>
            {
                IsSuccess = deserialized.IsSuccess,
                Message = deserialized.Message,
                Data = deserialized.Data
            };
        }

        
        
    }
}
