
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Entities.Penalizaciones;
using System;

namespace SGB.Application.Extensions.Mappers.PenalizacionesMapper
{
    public class PenalizacionMapper : IPenalizacionMapper
    {
        public Penalizacion MapFromDto(AddPenalizacionDto dto)
        {
            // Asumiendo que el constructor de Penalizacion es:
            // Penalizacion(int usuarioId, string motivo, DateTime fechaInicio, DateTime fechaFin)
            return new Penalizacion(dto.UsuarioId, dto.Motivo, dto.FechaInicio, dto.FechaFin);
        }

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

        public PenalizacionResponseDto MapToDto(Penalizacion entity)
        {
            return new PenalizacionResponseDto
            {
                IDPenalizacion = entity.Id,
                UsuarioId = entity.IDUsuario,
                Motivo = entity.Motivo,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin,
                FechaDevolucion = entity.FechaDevolucion,
                EstaActivo = entity.EstaActivo,
                Monto = entity.Monto // si existe en la entidad
            };
        }
    }
}
