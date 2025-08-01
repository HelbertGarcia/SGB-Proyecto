using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models;
using SGB.Presentation.Models.PenalizacionModels;
using SGB.Presentation.Services.Base;
using SGB.Presentation.Services.Mappers;


namespace SGB.Presentation.Services
{
    public class PenalizacionHttpService : IPenalizacionHttpService
    {
        private readonly IHttpService _httpService;
        private const string CONTROLLER_NAME = "Penalizacion";

        public PenalizacionHttpService(IHttpService httpService)
        {
            _httpService = httpService;
        }


        public async Task<ApiResponse<List<PenalizacionModel>>> GetPenalizacionesAsync()
        {
            return await _httpService.GetAsync<List<PenalizacionModel>>($"{CONTROLLER_NAME}/GetPenalizaciones");
        }

        public async Task<ApiResponse<PenalizacionModel>> GetPenalizacionByIdAsync(int id)
        {
            return await _httpService.GetAsync<PenalizacionModel>($"{CONTROLLER_NAME}/GetPenalizacionById?idPenalizacion={id}");
        }

        public async Task<ApiResponse<PenalizacionResponseDto>> CreatePenalizacionAsync(PenalizacionCreateModel model)
        {
            var dto = PenalizacionMapper.ToAddPenalizacionDto(model);
            return await _httpService.PostAsJsonAsync<AddPenalizacionDto, PenalizacionResponseDto>($"{CONTROLLER_NAME}/AddPenalizacion", dto);
        }

        public async Task<ApiResponse<PenalizacionResponseDto>> UpdatePenalizacionAsync(PenalizacionEditModel model)
        {
            var dto = PenalizacionMapper.ToUpdatePenalizacionDto(model);
            return await _httpService.PutAsJsonAsync<UpdatePenalizacionDto, PenalizacionResponseDto>($"{CONTROLLER_NAME}/UpdatePenalizacion?idPenalizacion={model.IDPenalizacion}", dto);
        }

        public async Task<ApiResponse<bool>> DeletePenalizacionAsync(int id)
        {
            return await _httpService.DeleteAsync<bool>($"{CONTROLLER_NAME}/DisablePenalizacion?id={id}");
        }



    }


}

