using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers
{
    public interface IPrestamoMapper
    {

        Prestamo MapFromAddDto(AddPrestamoDto dto);
        object MapToDto(Prestamo entity);
        void ApplyUpdateDto(Prestamo entity, UpdatePrestamoDto dto);
    }
}
