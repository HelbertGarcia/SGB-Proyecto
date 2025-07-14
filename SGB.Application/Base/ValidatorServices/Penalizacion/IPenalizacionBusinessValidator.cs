using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Base.ValidatorServices.Penalizacion
{
    public interface IPenalizacionBusinessValidator
    {
        Task<OperationResult<string>> ValidateForAddAsync(AddPenalizacionDto dto);
        Task<OperationResult<string>> ValidateForUpdateAsync(UpdatePenalizacionDto dto);
        Task<OperationResult<string>> ValidateForDisableAsync(DisablePenalizacionDto dto);
        Task<OperationResult<string>> ValidateForCalcularPenalizacionAsync(int idPrestamo);
    }
}
