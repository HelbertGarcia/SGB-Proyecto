using SGB.Application.Base;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;

public interface IPenalizacionServices : IBaseService<AddPenalizacionDto, UpdatePenalizacionDto, DisablePenalizacionDto, PenalizacionResponseDto>
{
    Task<OperationResult<PenalizacionResponseDto>> CalcularPenalizacionPorRetrasoAsync(int idPrestamo);
    Task<OperationResult<List<PenalizacionResponseDto>>> ObtenerPenalizacionesActivasPorUsuarioAsync(int usuarioId);
}
