using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using System.Collections;


namespace SGB.Application.Base.ValidatorServices.Prestamos
{
    public class PrestamoBusinessValidator : IPrestamoBusinessValidator
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IPenalizacionRepository _penalizacionRepository;
        private readonly ILibroRepository _libroRepository; // Inyéctalo


        public PrestamoBusinessValidator(
            IPrestamoRepository prestamoRepository,
            IPenalizacionRepository penalizacionRepository,
            ILibroRepository libroRepository)
        {
            _prestamoRepository = prestamoRepository;
            _penalizacionRepository = penalizacionRepository;
            _libroRepository = libroRepository;
        }


        public async Task<OperationResult<string>> ValidateForAddAsync(AddPrestamoDto dto)
        {
            if (dto == null)
                return OperationResult<string>.Failure("Datos de préstamo inválidos.");

          
            // Validar préstamos activos del usuario
            var prestamosActivosResult = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(dto.UsuarioId);
            if (!prestamosActivosResult.IsSuccess)
                return OperationResult<string>.Failure(prestamosActivosResult.Message);

            var prestamosActivos = prestamosActivosResult.Data;
           

            // Validar penalizaciones activas
            var penalizacionesResult = await _penalizacionRepository.GetPenalizacionesActivasPorUsuarioAsync(dto.UsuarioId);
            if (!penalizacionesResult.IsSuccess)
                return OperationResult<string>.Failure(penalizacionesResult.Message);

            var penalizaciones = penalizacionesResult.Data;
            if (penalizaciones != null && penalizaciones.Any())
                return OperationResult<string>.Failure("El usuario tiene penalizaciones activas.");

            return OperationResult<string>.Success("Validación de negocio exitosa.");
        }


        public async Task<OperationResult<string>> ValidateForUpdateAsync(UpdatePrestamoDto dto)
        {
            if (dto == null)
                return OperationResult<string>.Failure("Datos de actualización inválidos.");

            var prestamoResult = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);

            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<string>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;

            if (prestamo.FechaDevolucion.HasValue)
                return OperationResult<string>.Failure("No se puede modificar un préstamo que ya fue devuelto.");

            if (!prestamo.EstaActivo)
                return OperationResult<string>.Failure("No se puede modificar un préstamo inactivo.");

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

            if (prestamo.FechaDevolucion.HasValue)
                return OperationResult<string>.Failure("No se puede deshabilitar un préstamo que ya fue devuelto.");

            return OperationResult<string>.Success("Validación exitosa.");
        }







        public async Task<OperationResult<string>> ValidateForRegistrarDevolucionAsync(RegistrarDevolucionDto dto)
        {
            if (dto.IdPrestamo <= 0)
                return OperationResult<string>.Failure("ID del préstamo inválido.");

            var prestamoResult = await _prestamoRepository.GetByIdAsync(dto.IdPrestamo);
            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<string>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;

            if (!prestamo.EstaActivo)
                return OperationResult<string>.Failure("No se puede registrar devolución de un préstamo inactivo.");


            if (prestamo.Estado == EstadoPrestamo.Devuelto || prestamo.FechaDevolucion.HasValue)
                return OperationResult<string>.Failure("El préstamo ya fue devuelto.");

            if (dto.FechaDevolucion < prestamo.FechaInicio)
                return OperationResult<string>.Failure("La fecha de devolución no puede ser anterior a la fecha de inicio.");

            return OperationResult<string>.Success("Validación exitosa.");
        }



    }
}
