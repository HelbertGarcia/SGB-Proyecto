using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Entities.Prestamos;

namespace SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers
{
    public interface IPrestamoMapper
    {
        Prestamo MapFromAddDto(AddPrestamoDto dto);
        PrestamoResponseDto MapToDto(Prestamo entity);
        void ApplyUpdateDto(Prestamo entity, UpdatePrestamoDto dto);
    }
}