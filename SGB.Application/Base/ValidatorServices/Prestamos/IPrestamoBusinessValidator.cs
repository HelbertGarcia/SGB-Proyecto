using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Base.ValidatorServices.Prestamos
{
    public interface IPrestamoBusinessValidator
    {
        Task<OperationResult<string>> ValidateForAddAsync(AddPrestamoDto dto);
        Task<OperationResult<string>> ValidateForUpdateAsync(UpdatePrestamoDto dto);
        Task<OperationResult<string>> ValidateForDisableAsync(DiseblePrestamoDto dto);
        Task<OperationResult<string>> ValidateForRegistrarDevolucionAsync(int idPrestamo);
    }
}
