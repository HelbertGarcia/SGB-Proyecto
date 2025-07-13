using SGB.Application.Contracts.Interfaces.Mappers.PenalizacionMappers;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Entities.Penalizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Extensions.Mappers.PenalizacionesMapper
{
    public class PenalizacionMapper : IPenalizacionMapper
    {
        public void ApplyUpdateDto(Penalizacion entity, UpdatePenalizacionDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Motivo))
                entity.CambiarMotivo(dto.Motivo);

            if (dto.FechaInicio.HasValue)
                entity.FechaInicio = dto.FechaInicio.Value;

            if (dto.FechaFin.HasValue)
                entity.ExtenderPenalizacion(dto.FechaFin.Value);

            if (dto.FechaDevolucion.HasValue)
                entity.FechaDevolucion = dto.FechaDevolucion.Value;
        }

        public Penalizacion MapFromDto(AddPenalizacionDto dto)
        {
            return new Penalizacion(dto.UsuarioId, dto.Motivo, dto.FechaInicio, dto.FechaFin); ;
        }

        public object MapToDto(Penalizacion entity)
        {
            return new
            {
                entity.Id,
                IDUsuario = entity.IDUsuario,
                entity.Motivo,
                entity.FechaInicio,
                entity.FechaFin,
                entity.FechaDevolucion,
                entity.EstaActivo
            };
        }
    }
}
