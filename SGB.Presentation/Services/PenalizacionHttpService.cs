using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models;
using SGB.Presentation.Models.PenalizacionModels;
using SGB.Presentation.Services.Base;
using SGB.Presentation.Services.Mappers;
using SGB.Presentation.Endpoints;
using SGB.Presentation.Endpoints.EndpointsPenalizacion;

namespace SGB.Presentation.Services
{
    public class PenalizacionHttpService : IPenalizacionHttpService
    {
        private readonly IHttpService _httpService;
        private readonly IPenalizacionEndpoints _endpoints;

        public PenalizacionHttpService(IHttpService httpService, IPenalizacionEndpoints endpoints)
        {
            _httpService = httpService;
            _endpoints = endpoints;
        }

        public async Task<ApiResponse<List<PenalizacionModel>>> GetPenalizacionesAsync()
        {
            return await _httpService.GetAsync<List<PenalizacionModel>>(_endpoints.GetAll);
        }

        public async Task<ApiResponse<PenalizacionModel>> GetPenalizacionByIdAsync(int id)
        {
            return await _httpService.GetAsync<PenalizacionModel>($"{_endpoints.GetById}?idPenalizacion={id}");
        }

        public async Task<ApiResponse<PenalizacionResponseDto>> CreatePenalizacionAsync(PenalizacionCreateModel model)
        {
            var dto = PenalizacionMapper.ToAddPenalizacionDto(model);
            return await _httpService.PostAsJsonAsync<AddPenalizacionDto, PenalizacionResponseDto>(_endpoints.Create, dto);
        }

        public async Task<ApiResponse<PenalizacionResponseDto>> UpdatePenalizacionAsync(PenalizacionEditModel model)
        {
            var dto = PenalizacionMapper.ToUpdatePenalizacionDto(model);
            return await _httpService.PutAsJsonAsync<UpdatePenalizacionDto, PenalizacionResponseDto>($"{_endpoints.Update}?idPenalizacion={model.IDPenalizacion}", dto);
        }

        public async Task<ApiResponse<bool>> DeletePenalizacionAsync(int id)
        {
            return await _httpService.DeleteAsync<bool>($"{_endpoints.Delete}?id={id}");
        }
    }
}
