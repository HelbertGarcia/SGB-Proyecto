
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Presentation.Models.PrestamoModels;
using SGB.Presentation.Models;
using SGB.Presentation.Services.Base;
using SGB.Application.Wrappers;

using SGB.Presentation.Services.Mappers.SGB.Presentation.Mappers;

namespace SGB.Presentation.Services
{
    public class PrestamoHttpService : IPrestamoHttpService
    {
        private readonly IHttpService _httpService;
        private const string CONTROLLER_NAME = "Prestamo";

        public PrestamoHttpService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<ApiResponse<List<PrestamoModel>>> GetPrestamosAsync()
        {
            var response = await _httpService.GetAsync<List<PrestamoModel>>($"{CONTROLLER_NAME}/GetPrestamos");
            return response;
        }

        public async Task<ApiResponse<PrestamoModel>> GetPrestamoByIdAsync(int id)
        {
            var response = await _httpService.GetAsync<PrestamoModel>($"{CONTROLLER_NAME}/GetPrestamosById?id={id}");
            return response;
        }

        public async Task<ApiResponse<PrestamoResponseDto>> CreatePrestamoAsync(PrestamoCreateModel model)
        {
            var dto = PrestamoMapper.ToAddPrestamoDto(model);
            var response = await _httpService.PostAsJsonAsync<AddPrestamoDto, PrestamoResponseDto>($"{CONTROLLER_NAME}/AddPrestamo", dto);
            return response;
        }

        public async Task<ApiResponse<PrestamoResponseDto>> UpdatePrestamoAsync(PrestamoEditModel model)
        {
            var dto = PrestamoMapper.ToUpdatePrestamoDto(model);
            var response = await _httpService.PutAsJsonAsync<UpdatePrestamoDto, PrestamoResponseDto>($"{CONTROLLER_NAME}/UpdatePrestamo?id={model.IDPrestamo}", dto);
            return response;
        }

        public async Task<ApiResponse<bool>> RegistrarDevolucionAsync(PrestamoDevolucionModel model)
        {
            var dto = PrestamoMapper.ToRegistrarDevolucionDto(model);
            var response = await _httpService.PostAsJsonAsync<RegistrarDevolucionDto, bool>($"{CONTROLLER_NAME}/Registrar-devolucion", dto);
            return response;
        }


        public async Task<ApiResponse<bool>> DeletePrestamoAsync(int id)
        {
            return await _httpService.DeleteAsync<bool>($"{CONTROLLER_NAME}/DisablePrestamo?id={id}");
        }




    }
}
