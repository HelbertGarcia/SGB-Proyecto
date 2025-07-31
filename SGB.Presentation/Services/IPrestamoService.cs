using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Presentation.Models.PrestamoModels;
using SGB.Presentation.Models;

using SGB.Application.Wrappers;

namespace SGB.Presentation.Services
{
    public interface IPrestamoHttpService
    {
        Task<ApiResponse<List<PrestamoModel>>> GetPrestamosAsync();
        Task<ApiResponse<PrestamoModel>> GetPrestamoByIdAsync(int id);
        Task<ApiResponse<PrestamoDto>> CreatePrestamoAsync(PrestamoCreateModel model);
        Task<ApiResponse<object>> UpdatePrestamoAsync(PrestamoEditModel model);
        Task<ApiResponse<bool>> RegistrarDevolucionAsync(PrestamoDevolucionModel model);
    }
}
