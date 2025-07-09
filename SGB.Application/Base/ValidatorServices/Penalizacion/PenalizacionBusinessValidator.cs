using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Base.ValidatorServices.Penalizacion
{
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

        public async Task<OperationResult> ValidateForAddAsync(AddPenalizacionDto dto)
        {
            if (dto == null)
                return new OperationResult { Success = false, Message = "Datos inválidos para penalización." };

            if (dto.FechaInicio > dto.FechaFin)
                return new OperationResult { Success = false, Message = "La fecha de inicio no puede ser mayor que la fecha fin." };

            // Podrías validar que el usuario no tenga penalizaciones activas que se crucen, etc.

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ValidateForUpdateAsync(UpdatePenalizacionDto dto)
        {
            if (dto == null)
                return new OperationResult { Success = false, Message = "Datos inválidos para actualizar penalización." };

            var penalizacion = await _penalizacionRepository.GetByIdAsync(dto.IDPenalizacion);
            if (penalizacion == null)
                return new OperationResult { Success = false, Message = "Penalización no encontrada." };

            if (dto.FechaInicio.HasValue && dto.FechaFin.HasValue && dto.FechaInicio > dto.FechaFin)
                return new OperationResult { Success = false, Message = "La fecha de inicio no puede ser mayor que la fecha fin." };

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ValidateForDisableAsync(DisablePenalizacionDto dto)
        {
            if (dto == null)
                return new OperationResult { Success = false, Message = "Datos inválidos para desactivar penalización." };

            var penalizacion = await _penalizacionRepository.GetByIdAsync(dto.IDPenalizacion);
            if (penalizacion == null)
                return new OperationResult { Success = false, Message = "Penalización no encontrada." };

            if (!penalizacion.EstaActivo)
                return new OperationResult { Success = false, Message = "La penalización ya está desactivada." };

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ValidateForCalcularPenalizacionAsync(int idPrestamo)
        {
            var prestamo = await _prestamoRepository.GetByIdAsync(idPrestamo);
            if (prestamo == null)
                return new OperationResult { Success = false, Message = "Préstamo no encontrado." };

            if (!prestamo.FechaDevolucion.HasValue)
                return new OperationResult { Success = false, Message = "El préstamo no tiene fecha de devolución registrada." };

            if (prestamo.FechaDevolucion <= prestamo.FechaFin)
                return new OperationResult { Success = false, Message = "No hay retraso en la devolución." };

            return new OperationResult { Success = true };
        }
    }
}
