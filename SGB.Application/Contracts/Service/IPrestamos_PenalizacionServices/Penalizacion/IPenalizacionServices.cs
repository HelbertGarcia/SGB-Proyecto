using SGB.Application.Base;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Penalizacion
{
    public interface IPenalizacionServices  : IBaseService<AddPenalizacionDto, UpdatePenalizacionDto, DisablePenalizacionDto>
    {
       
        

    }
}
