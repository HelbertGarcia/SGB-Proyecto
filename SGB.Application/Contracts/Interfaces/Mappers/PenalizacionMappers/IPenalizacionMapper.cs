using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Entities.Penalizaciones;

public interface IPenalizacionMapper
{
    Penalizacion MapFromDto(AddPenalizacionDto dto);
    void ApplyUpdateDto(Penalizacion entity, UpdatePenalizacionDto dto);
    PenalizacionResponseDto MapToDto(Penalizacion entity);
}
