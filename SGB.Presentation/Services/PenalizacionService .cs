using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models;
using SGB.Presentation.Models.PenalizacionModels;
using SGB.Presentation.Services.Base;


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

        public async Task<ApiResponse<PenalizacionDto>> CreatePenalizacionAsync(PenalizacionCreateModel model)
        {
            var dto = new
            {
                UsuarioId = model.UsuarioId,
                IDPrestamo = model.IDPrestamo,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                Monto = model.Monto,
                Motivo = model.Motivo
            };

            return await _httpService.PostAsJsonAsync<object, PenalizacionDto>($"{CONTROLLER_NAME}/AddPenalizacion", dto);
        }

        public async Task<ApiResponse<object>> UpdatePenalizacionAsync(PenalizacionEditModel model)
        {
            var dto = new
            {
                IDPenalizacion = model.IDPenalizacion,
                FechaFin = model.FechaFin,
                Monto = model.Monto,
                Motivo = model.Motivo
            };

            return await _httpService.PutAsJsonAsync<object, object>($"{CONTROLLER_NAME}/UpdatePenalizacion?idPenalizacion={model.IDPenalizacion}", dto);
        }

        /*public async Task<ApiResponse<object>> DisablePenalizacionAsync(int id)
        {
            return await _httpService.PutAsJsonAsync<object, object>($"{CONTROLLER_NAME}/DisablePenalizacion?idPenalizacion={id}", new { });
        }
        */
    }
}
