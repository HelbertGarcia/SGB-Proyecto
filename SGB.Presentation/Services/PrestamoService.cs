using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Presentation.Models.PrestamoModels;
using SGB.Presentation.Models;
using SGB.Presentation.Services.Base;
using SGB.Application.Wrappers;

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

        public async Task<ApiResponse<PrestamoDto>> CreatePrestamoAsync(PrestamoCreateModel model)
        {
            var dto = new
            {
                UsuarioId = model.UsuarioId,
                ISBN = model.ISBN,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin
            };

            var response = await _httpService.PostAsJsonAsync<object, PrestamoDto>($"{CONTROLLER_NAME}/AddPrestamo", dto);
            return response;
        }

        public async Task<ApiResponse<object>> UpdatePrestamoAsync(PrestamoEditModel model)
        {
            var dto = new
            {
                IDPrestamo = model.IDPrestamo,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin
            };

            var response = await _httpService.PutAsJsonAsync<object, object>($"{CONTROLLER_NAME}/UpdatePrestamo?id={model.IDPrestamo}", dto);
            return response;
        }

        public async Task<ApiResponse<bool>> RegistrarDevolucionAsync(PrestamoDevolucionModel model)
        {
            var dto = new
            {
                idPrestamo = model.IdPrestamo,
                fechaDevolucion = model.FechaDevolucion
            };

            var response = await _httpService.PostAsJsonAsync<object, bool>($"{CONTROLLER_NAME}/Registrar-devolucion", dto);
            return response;
        }
    }
}
