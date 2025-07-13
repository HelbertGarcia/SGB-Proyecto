using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Entities.Penalizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Interfaces.Mappers.PenalizacionMappers
{
    public interface IPenalizacionMapper
    {
        Penalizacion MapFromDto(AddPenalizacionDto dto);
        void ApplyUpdateDto(Penalizacion entity, UpdatePenalizacionDto dto);
        object MapToDto(Penalizacion entity);
    }
}
