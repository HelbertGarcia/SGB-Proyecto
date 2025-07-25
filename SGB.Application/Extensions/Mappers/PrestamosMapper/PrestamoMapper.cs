using SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Entities.Prestamos;

namespace SGB.Application.Extensions.Mappers.PrestamosMapper
{
    public class PrestamoMapper : IPrestamoMapper
    {
        public void ApplyUpdateDto(Prestamo entity, UpdatePrestamoDto dto)
        {
            if (dto.FechaInicio.HasValue)
                entity.FechaInicio = dto.FechaInicio.Value;

            if (dto.FechaFin.HasValue)
                entity.FechaFin = dto.FechaFin.Value;

           


        }


        public Prestamo MapFromAddDto(AddPrestamoDto dto)
        {
            return new Prestamo(dto.UsuarioId, dto.ISBN, dto.FechaInicio, dto.FechaFin);
        }

        public PrestamoResponseDto MapToDto(Prestamo entity)
        {
            return new PrestamoResponseDto
            {
                Id = entity.Id,
                UsuarioId = entity.UsuarioId,
                ISBN = entity.ISBN,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin,
                FechaDevolucion = entity.FechaDevolucion,
                Estado = entity.Estado.ToString(),
                EstaActivo = entity.EstaActivo

            };
        }
    }
}
