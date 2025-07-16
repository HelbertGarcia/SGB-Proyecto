using SGB.Application.Base;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPrestamosServices : IBaseService<AddPrestamoDto, UpdatePrestamoDto, DiseblePrestamoDto, PrestamoResponseDto>
{
    Task<OperationResult<string>> ActualizarEstadoPrestamoPorVencimientoAsync(int idPrestamo);
    Task<OperationResult<string>> RegistrarDevolucionAsync(RegistrarDevolucionDto dto);

    Task<OperationResult<bool>> PuedePrestarAsync(int usuarioId);
    Task<OperationResult<IList<PrestamoResponseDto>>> ObtenerPrestamosActivosPorUsuarioAsync(int usuarioId);
}
