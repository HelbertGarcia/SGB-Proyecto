using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<OperationResult> ValidateForAddAsync(AddPrestamoDto dto)
        {
            if (dto == null)
                return new OperationResult { Success = false, Message = "Datos de préstamo inválidos." };

            // Validar que usuario no tenga préstamos activos pendientes de devolución
            var prestamosActivos = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(dto.UsuarioId);
            if (prestamosActivos.Any(p => p.Estado != Domain.Entities.Prestamos.EstadoPrestamo.Devuelto && p.Estado != Domain.Entities.Prestamos.EstadoPrestamo.DevueltoConAtraso))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El usuario tiene préstamos pendientes no devueltos."
                };
            }

            // Validar que usuario no tenga penalizaciones activas
            var penalizacionesActivas = await _penalizacionRepository.GetPenalizacionesActivasPorUsuarioAsync(dto.UsuarioId);
            if (penalizacionesActivas != null && penalizacionesActivas.Data is System.Collections.IEnumerable penList && penList.GetEnumerator().MoveNext())
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El usuario tiene penalizaciones activas."
                };
            }

            // Podrías agregar más validaciones de negocio como fechas, estado, etc.

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ValidateForUpdateAsync(UpdatePrestamoDto dto)
        {
            if (dto == null)
                return new OperationResult { Success = false, Message = "Datos de actualización inválidos." };

            var prestamo = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);
            if (prestamo == null)
                return new OperationResult { Success = false, Message = "Préstamo no encontrado." };

            // Validar que las fechas sean coherentes
            if (dto.FechaFin.HasValue && dto.FechaDevolucion.HasValue && dto.FechaDevolucion < dto.FechaFin)
            {
                return new OperationResult { Success = false, Message = "La fecha de devolución no puede ser anterior a la fecha fin del préstamo." };
            }

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ValidateForDisableAsync(DiseblePrestamoDto dto)
        {
            if (dto == null)
                return new OperationResult { Success = false, Message = "Datos inválidos para deshabilitar préstamo." };

            var prestamo = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);
            if (prestamo == null)
                return new OperationResult { Success = false, Message = "Préstamo no encontrado." };

            if (!prestamo.EstaActivo)
                return new OperationResult { Success = false, Message = "El préstamo ya está desactivado." };

            return new OperationResult { Success = true };
        }

        public async Task<OperationResult> ValidateForRegistrarDevolucionAsync(int idPrestamo)
        {
            var prestamo = await _prestamoRepository.GetByIdAsync(idPrestamo);
            if (prestamo == null)
                return new OperationResult { Success = false, Message = "Préstamo no encontrado." };

            if (prestamo.FechaDevolucion.HasValue)
                return new OperationResult { Success = false, Message = "El préstamo ya tiene fecha de devolución registrada." };

            return new OperationResult { Success = true };
        }
    }

}
