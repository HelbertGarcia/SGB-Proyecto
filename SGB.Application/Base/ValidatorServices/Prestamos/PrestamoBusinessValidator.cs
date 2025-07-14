using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System.Collections;

namespace SGB.Application.Base.ValidatorServices.Prestamos
{
    public class PrestamoBusinessValidator : IPrestamoBusinessValidator
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IPenalizacionRepository _penalizacionRepository;

        public PrestamoBusinessValidator(
            IPrestamoRepository prestamoRepository,
            IPenalizacionRepository penalizacionRepository)
        {
            _prestamoRepository = prestamoRepository;
            _penalizacionRepository = penalizacionRepository;
        }

        public async Task<OperationResult<string>> ValidateForAddAsync(AddPrestamoDto dto)
        {
            if (dto == null)
                return OperationResult<string>.Failure("Datos de préstamo inválidos.");

            // Obtener préstamos activos con validación de éxito
            var prestamosActivosResult = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(dto.UsuarioId);
            if (!prestamosActivosResult.IsSuccess)
                return OperationResult<string>.Failure(prestamosActivosResult.Message);

            var prestamosActivos = prestamosActivosResult.Data;

            // Validar que existan préstamos activos no devueltos
            if (prestamosActivos != null && prestamosActivos.Any(p => p.Estado != Domain.Entities.Prestamos.EstadoPrestamo.Devuelto
                                                                        && p.Estado != Domain.Entities.Prestamos.EstadoPrestamo.DevueltoConAtraso))
            {
                return OperationResult<string>.Failure("El usuario tiene préstamos pendientes no devueltos.");
            }

            // Obtener penalizaciones activas
            var penalizacionesActivasResult = await _penalizacionRepository.GetPenalizacionesActivasPorUsuarioAsync(dto.UsuarioId);
            if (!penalizacionesActivasResult.IsSuccess)
                return OperationResult<string>.Failure(penalizacionesActivasResult.Message);

            var penalizacionesActivas = penalizacionesActivasResult.Data;

            // Validar si hay penalizaciones activas
            if (penalizacionesActivas != null && penalizacionesActivas is IEnumerable penList && penList.GetEnumerator().MoveNext())
            {
                return OperationResult<string>.Failure("El usuario tiene penalizaciones activas.");
            }

            return OperationResult<string>.Success("Validación exitosa.");
        }

        public async Task<OperationResult<string>> ValidateForUpdateAsync(UpdatePrestamoDto dto)
        {
            if (dto == null)
                return OperationResult<string>.Failure("Datos de actualización inválidos.");

            var prestamo = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);
            if (prestamo == null)
                return OperationResult<string>.Failure("Préstamo no encontrado.");

            if (dto.FechaFin.HasValue && dto.FechaDevolucion.HasValue && dto.FechaDevolucion < dto.FechaFin)
            {
                return OperationResult<string>.Failure("La fecha de devolución no puede ser anterior a la fecha fin.");
            }

            return OperationResult<string>.Success("Validación exitosa.");
        }

        public async Task<OperationResult<string>> ValidateForDisableAsync(DiseblePrestamoDto dto)
        {
            if (dto == null)
                return OperationResult<string>.Failure("Datos inválidos para deshabilitar préstamo.");

            var prestamoResult = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);

            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<string>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;

            if (!prestamo.EstaActivo)
                return OperationResult<string>.Failure("El préstamo ya está desactivado.");

            return OperationResult<string>.Success("Validación exitosa.");
        }


        public async Task<OperationResult<string>> ValidateForRegistrarDevolucionAsync(int idPrestamo)
        {
            var prestamoResult = await _prestamoRepository.GetByIdAsync(idPrestamo);

            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<string>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;

            if (prestamo.FechaDevolucion.HasValue)
                return OperationResult<string>.Failure("El préstamo ya tiene fecha de devolución registrada.");

            return OperationResult<string>.Success("Validación exitosa.");
        }


    }
}
