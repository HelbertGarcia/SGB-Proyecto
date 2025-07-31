using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;

public class PenalizacionBusinessValidator : IPenalizacionBusinessValidator
{
    private readonly IPenalizacionRepository _penalizacionRepository;
    private readonly IPrestamoRepository _prestamoRepository;

    public PenalizacionBusinessValidator(
        IPenalizacionRepository penalizacionRepository,
        IPrestamoRepository prestamoRepository)
    {
        _penalizacionRepository = penalizacionRepository;
        _prestamoRepository = prestamoRepository;
    }

    // Valida la creación de una penalización
    public async Task<OperationResult<string>> ValidateForAddAsync(AddPenalizacionDto dto)
    {

        if (dto == null)
            return OperationResult<string>.Failure("Datos inválidos para penalización.");

        if (dto.FechaInicio > dto.FechaFin)
            return OperationResult<string>.Failure("La fecha de inicio no puede ser mayor que la fecha fin.");

        // Consulta penalizaciones activas para evitar solapamientos
        var penalizacionesActivasResult = await _penalizacionRepository.GetPenalizacionesActivasPorUsuarioAsync(dto.UsuarioId);
        if (!penalizacionesActivasResult.IsSuccess)
            return OperationResult<string>.Failure(penalizacionesActivasResult.Message);

        var penalizacionesActivas = penalizacionesActivasResult.Data;
        if (penalizacionesActivas != null && penalizacionesActivas.Any())
        {
            // Retorna error si hay penalizaciones activas que podrían superponerse
            return OperationResult<string>.Failure("El usuario tiene penalizaciones activas que podrían solaparse.");
        }

        return OperationResult<string>.Success("Validación exitosa.");
    }

    // Valida la actualización de una penalización
    public async Task<OperationResult<string>> ValidateForUpdateAsync(UpdatePenalizacionDto dto)
    {
        if (dto == null)
            return OperationResult<string>.Failure("Datos inválidos para actualizar penalización.");

        var penalizacionResult = await _penalizacionRepository.GetByIdAsync(dto.IDPenalizacion);
        if (!penalizacionResult.IsSuccess || penalizacionResult.Data == null)
            return OperationResult<string>.Failure("Penalización no encontrada.");

        return OperationResult<string>.Success("Validación exitosa.");
    }

    // Valida la desactivación de una penalización
    public async Task<OperationResult<string>> ValidateForDisableAsync(DisablePenalizacionDto dto)
    {
        if (dto == null)
            return OperationResult<string>.Failure("Datos inválidos para desactivar penalización.");

        var penalizacionResult = await _penalizacionRepository.GetByIdAsync(dto.IDPenalizacion);
        if (!penalizacionResult.IsSuccess || penalizacionResult.Data == null)
            return OperationResult<string>.Failure("Penalización no encontrada.");

        var penalizacion = penalizacionResult.Data;
        if (!penalizacion.EstaActivo)
            return OperationResult<string>.Failure("La penalización ya está desactivada.");

        return OperationResult<string>.Success("Validación exitosa.");
    }

    // Valida si se puede calcular penalización por retraso en un préstamo

    public async Task<OperationResult<string>> ValidateForCalcularPenalizacionAsync(int idPrestamo)
    {
        var prestamoResult = await _prestamoRepository.GetByIdAsync(idPrestamo);
        if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
            return OperationResult<string>.Failure("Préstamo no encontrado.");

        var prestamo = prestamoResult.Data;

        if (!prestamo.FechaDevolucion.HasValue)
            return OperationResult<string>.Failure("El préstamo aún no ha sido devuelto.");

        if (prestamo.FechaDevolucion <= prestamo.FechaFin)
            return OperationResult<string>.Failure("No hay retraso en la devolución.");

     

        return OperationResult<string>.Success("Validación exitosa.");
    }


}
