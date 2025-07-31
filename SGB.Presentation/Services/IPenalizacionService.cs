using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Presentation.Models.PenalizacionModels;
using SGB.Presentation.Models;

using SGB.Application.Wrappers;

namespace SGB.Presentation.Services
{
    public interface IPenalizacionHttpService
    {
        Task<ApiResponse<List<PenalizacionModel>>> GetPenalizacionesAsync();
        Task<ApiResponse<PenalizacionModel>> GetPenalizacionByIdAsync(int id);
        Task<ApiResponse<PenalizacionDto>> CreatePenalizacionAsync(PenalizacionCreateModel model);
        Task<ApiResponse<object>> UpdatePenalizacionAsync(PenalizacionEditModel model);
      //  Task<ApiResponse<object>> DisablePenalizacionAsync(int id);
    }
}
