using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Penalizacion
{
    public interface IPenalizacionServices  
    {
        Task<OperationResult> AddPenalizacionAsync(AddPenalizacionDto addPenalizacionDto);
        Task<OperationResult> UpdatePenalizacionAsync(UpdatePenalizacionDto updatePenalizacionDto);
        Task<OperationResult> DisablePenalizacionAsync(DisablePenalizacionDto disablePenalizacionDto);
        Task<OperationResult> GetAllPenalizacionesAsync(GetPenalizacionDto getPenalizacionDto);
        Task<OperationResult> GetPenalizacionByIdAsync(int idPenalizacion);
        

    }
}
