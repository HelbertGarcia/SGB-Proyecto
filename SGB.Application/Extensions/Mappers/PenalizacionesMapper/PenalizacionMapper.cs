using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Entities.Penalizaciones;
using System;

namespace SGB.Application.Extensions.Mappers.PenalizacionesMapper
{
    public class PenalizacionMapper : IPenalizacionMapper
    {
        public Penalizacion MapFromDto(AddPenalizacionDto dto)
        {
            return new Penalizacion(
                dto.UsuarioId,
                dto.Motivo,
                dto.FechaInicio,
                dto.FechaFin,
                dto.IDPrestamo,
                dto.Monto  
            );
        }

        public void ApplyUpdateDto(Penalizacion entity, UpdatePenalizacionDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Motivo))
                entity.CambiarMotivo(dto.Motivo);

            if (dto.FechaInicio.HasValue)
                entity.FechaInicio = dto.FechaInicio.Value;

            if (dto.FechaFin.HasValue)
                entity.ExtenderPenalizacion(dto.FechaFin.Value);

           
        }

        public PenalizacionResponseDto MapToDto(Penalizacion entity)
        {
            return new PenalizacionResponseDto
            {
                IDPenalizacion = entity.Id,
                UsuarioId = entity.IDUsuario,
                Motivo = entity.Motivo,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin,
                Monto = entity.Monto,
                EstaActivo = entity.EstaActivo,
                IDPrestamo = entity.IDPrestamo // ✅ CORRECTO
            };
        }


    }
}
