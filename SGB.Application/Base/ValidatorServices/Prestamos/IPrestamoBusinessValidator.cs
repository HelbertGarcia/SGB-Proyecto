using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base.ValidatorServices.Prestamos
{
    public interface IPrestamoBusinessValidator
    {
        Task<OperationResult> ValidateForAddAsync(AddPrestamoDto dto);
        Task<OperationResult> ValidateForUpdateAsync(UpdatePrestamoDto dto);
        Task<OperationResult> ValidateForDisableAsync(DiseblePrestamoDto dto);
        Task<OperationResult> ValidateForRegistrarDevolucionAsync(int idPrestamo);
    }
}
