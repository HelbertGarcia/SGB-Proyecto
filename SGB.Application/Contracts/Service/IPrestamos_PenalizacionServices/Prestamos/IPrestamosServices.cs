using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Prestamos
{
     public interface IPrestamosServices
    {

        Task<OperationResult> AddPrestamoAsync(AddPrestamoDto addPrestamoDto);
        Task<OperationResult> UpdatePrestamoAsync(UpdatePrestamoDto updatePrestamoDto);
        Task<OperationResult> DeletePrestamoAsync(DeletePrestamoDto deletePrestamoDto);
        Task<OperationResult> GetAllPrestamosAsync(GetPrestamoDto getPrestamoDto);
       Task<OperationResult> GetPrestamoByIdAsync(int idPrestamo);

       

    }
}
