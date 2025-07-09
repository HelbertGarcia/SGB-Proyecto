using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base.ValidatorServices.Penalizacion
{
    public interface IPenalizacionBusinessValidator
    {
        Task<OperationResult> ValidateForAddAsync(AddPenalizacionDto dto);
        Task<OperationResult> ValidateForUpdateAsync(UpdatePenalizacionDto dto);
        Task<OperationResult> ValidateForDisableAsync(DisablePenalizacionDto dto);
        Task<OperationResult> ValidateForCalcularPenalizacionAsync(int idPrestamo);
    }

}
