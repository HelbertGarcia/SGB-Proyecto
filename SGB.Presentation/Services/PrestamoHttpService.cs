using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Presentation.Models.PrestamoModels;
using SGB.Presentation.Models;
using SGB.Presentation.Services.Base;
using SGB.Application.Wrappers;
using SGB.Presentation.Mappers;
using SGB.Presentation.Endpoints;
using SGB.Presentation.Endpoints.EndpointsPrestamo;

namespace SGB.Presentation.Services
{
    public class PrestamoHttpService : IPrestamoHttpService
    {
        private readonly IHttpService _httpService;
        private readonly IPrestamoEndpoints _endpoints;

        public PrestamoHttpService(IHttpService httpService, IPrestamoEndpoints endpoints)
        {
            _httpService = httpService;
            _endpoints = endpoints;
        }

        public async Task<ApiResponse<List<PrestamoModel>>> GetPrestamosAsync()
        {
            return await _httpService.GetAsync<List<PrestamoModel>>(_endpoints.GetAll);
        }

        public async Task<ApiResponse<PrestamoModel>> GetPrestamoByIdAsync(int id)
        {
            return await _httpService.GetAsync<PrestamoModel>($"{_endpoints.GetById}?id={id}");
        }

        public async Task<ApiResponse<PrestamoResponseDto>> CreatePrestamoAsync(PrestamoCreateModel model)
        {
            var dto = PrestamoMapper.ToAddPrestamoDto(model);
            return await _httpService.PostAsJsonAsync<AddPrestamoDto, PrestamoResponseDto>(_endpoints.Create, dto);
        }

        public async Task<ApiResponse<PrestamoResponseDto>> UpdatePrestamoAsync(PrestamoEditModel model)
        {
            var dto = PrestamoMapper.ToUpdatePrestamoDto(model);
            return await _httpService.PutAsJsonAsync<UpdatePrestamoDto, PrestamoResponseDto>($"{_endpoints.Update}?id={model.IDPrestamo}", dto);
        }

        public async Task<ApiResponse<bool>> RegistrarDevolucionAsync(PrestamoDevolucionModel model)
        {
            var dto = PrestamoMapper.ToRegistrarDevolucionDto(model);
            return await _httpService.PostAsJsonAsync<RegistrarDevolucionDto, bool>(_endpoints.RegistrarDevolucion, dto);
        }

        public async Task<ApiResponse<bool>> DeletePrestamoAsync(int id)
        {
            return await _httpService.DeleteAsync<bool>($"{_endpoints.Delete}?id={id}");
        }
    }
}
