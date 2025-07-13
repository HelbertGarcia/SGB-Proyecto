using SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Extensions.Mappers.PrestamosMapper
{
    public class PrestamoMapper : IPrestamoMapper
    {
        public void ApplyUpdateDto(Prestamo entity, UpdatePrestamoDto dto)
        {
            if (dto.FechaFin.HasValue)
                entity.FechaFin = dto.FechaFin.Value;

            if (dto.FechaDevolucion.HasValue)
                entity.FechaDevolucion = dto.FechaDevolucion.Value;

            if (!string.IsNullOrWhiteSpace(dto.Estado))
                entity.Estado = Enum.Parse<EstadoPrestamo>(dto.Estado, true);
        }

        public Prestamo MapFromAddDto(AddPrestamoDto dto)
        {
            return new Prestamo(dto.UsuarioId, dto.ISBN, dto.FechaInicio, dto.FechaFin);
        }

        public object MapToDto(Prestamo entity)
        {
            return new
            {
                entity.Id,
                entity.UsuarioId,
                entity.ISBN,
                entity.FechaInicio,
                entity.FechaFin,
                entity.FechaDevolucion,
                Estado = entity.Estado.ToString(),
                entity.EstaActivo
            }; ;
        }
    }
}
