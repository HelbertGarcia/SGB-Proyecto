using FluentValidation;
using Microsoft.Extensions.Configuration;
using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Loggers; 
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Application.Services.Prestamos_y_PenalizacionServices.PenalizacionServices
{
    public sealed class PenalizacionService : IPenalizacionServices
    {
        private readonly IPenalizacionRepository _penalizacionRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IAppLogger<PenalizacionService> _logger; 
        private readonly IConfiguration _configuration;
        private readonly IValidator<AddPenalizacionDto> _addValidator;
        private readonly IValidator<UpdatePenalizacionDto> _updateValidator;
        private readonly IValidator<DisablePenalizacionDto> _disableValidator;
        private readonly IPenalizacionBusinessValidator _businessValidator;
        private readonly IPenalizacionMapper _mapper;

        public PenalizacionService(
            IPenalizacionRepository penalizacionRepository,
            IPrestamoRepository prestamoRepository,
            IAppLogger<PenalizacionService> logger, // Cambio aquí
            IConfiguration configuration,
            IValidator<AddPenalizacionDto> addValidator,
            IValidator<UpdatePenalizacionDto> updateValidator,
            IValidator<DisablePenalizacionDto> disableValidator,
            IPenalizacionBusinessValidator businessValidator,
            IPenalizacionMapper mapper)
        {
            _penalizacionRepository = penalizacionRepository;
            _prestamoRepository = prestamoRepository;
            _logger = logger;
            _configuration = configuration;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _disableValidator = disableValidator;
            _businessValidator = businessValidator;
            _mapper = mapper;
        }

        public async Task<OperationResult<PenalizacionResponseDto>> AddAsync(AddPenalizacionDto dto)
        {
            _logger.Info("Iniciando registro de penalización para usuario {0}", dto?.UsuarioId);

            var dtoValidation = await _addValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
            {
                var errors = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Validación de DTO falló: {0}", errors);
                return OperationResult<PenalizacionResponseDto>.Failure(errors);
            }

            var businessValidation = await _businessValidator.ValidateForAddAsync(dto);
            if (!businessValidation.IsSuccess)
            {
                _logger.Error("Validación de negocio falló: {0}", businessValidation.Message);
                return OperationResult<PenalizacionResponseDto>.Failure(businessValidation.Message);
            }

            try
            {
                var penalizacion = _mapper.MapFromDto(dto);
                _logger.Info("Penalización mapeada correctamente: {0}", penalizacion);

                var result = await _penalizacionRepository.AddAsync(penalizacion);
                if (!result.IsSuccess)
                {
                    _logger.Error("Error en repositorio al agregar penalización: {0}", result.Message);
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message);
                }

                _logger.Info("Penalización registrada exitosamente con ID {0}", penalizacion.Id);
                return OperationResult<PenalizacionResponseDto>.Success(_mapper.MapToDto(penalizacion), "Penalización registrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al registrar penalización para usuario {0}", dto?.UsuarioId);
                return OperationResult<PenalizacionResponseDto>.Failure("Error inesperado al registrar penalización.");
            }
        }

        public async Task<OperationResult<PenalizacionResponseDto>> UpdateAsync(UpdatePenalizacionDto dto)
        {
            _logger.Info("Iniciando actualización de penalización ID {0}", dto?.IDPenalizacion);

            var dtoValidation = await _updateValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
            {
                var errors = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Validación de DTO falló para actualización: {0}", errors);
                return OperationResult<PenalizacionResponseDto>.Failure(errors);
            }

            var businessValidation = await _businessValidator.ValidateForUpdateAsync(dto);
            if (!businessValidation.IsSuccess)
            {
                _logger.Error("Validación de negocio falló para actualización: {0}", businessValidation.Message);
                return OperationResult<PenalizacionResponseDto>.Failure(businessValidation.Message);
            }

            try
            {
                var penalizacionResult = await _penalizacionRepository.GetByIdAsync(dto.IDPenalizacion);

                if (!penalizacionResult.IsSuccess || penalizacionResult.Data == null)
                {
                    _logger.Error("Penalización no encontrada con ID {0}", dto.IDPenalizacion);
                    return OperationResult<PenalizacionResponseDto>.Failure("Penalización no encontrada.");
                }

                var penalizacion = penalizacionResult.Data;
                _logger.Info("Penalización encontrada: {0}", penalizacion);

                _mapper.ApplyUpdateDto(penalizacion, dto);

                var result = await _penalizacionRepository.UpdateAsync(penalizacion);
                if (!result.IsSuccess)
                {
                    _logger.Error("Error en repositorio al actualizar penalización: {0}", result.Message);
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message);
                }

                _logger.Info("Penalización actualizada exitosamente ID {0}", dto.IDPenalizacion);
                return OperationResult<PenalizacionResponseDto>.Success(_mapper.MapToDto(penalizacion), "Penalización actualizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al actualizar penalización ID {0}", dto?.IDPenalizacion);
                return OperationResult<PenalizacionResponseDto>.Failure("Error inesperado al actualizar penalización.");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(DisablePenalizacionDto dto)
        {
            _logger.Info("Iniciando desactivación de penalización ID {0}", dto?.IDPenalizacion);

            var dtoValidation = await _disableValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
            {
                var errors = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Validación de DTO falló para desactivación: {0}", errors);
                return OperationResult<bool>.Failure(errors);
            }

            var businessValidation = await _businessValidator.ValidateForDisableAsync(dto);
            if (!businessValidation.IsSuccess)
            {
                _logger.Error("Validación de negocio falló para desactivación: {0}", businessValidation.Message);
                return OperationResult<bool>.Failure(businessValidation.Message);
            }

            try
            {
                var result = await _penalizacionRepository.DisableAsync(dto.IDPenalizacion);
                if (!result.IsSuccess)
                {
                    _logger.Error("Error en repositorio al desactivar penalización: {0}", result.Message);
                    return OperationResult<bool>.Failure(result.Message);
                }

                _logger.Info("Penalización desactivada exitosamente ID {0}", dto.IDPenalizacion);
                return OperationResult<bool>.Success(true, "Penalización desactivada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al desactivar penalización ID {0}", dto?.IDPenalizacion);
                return OperationResult<bool>.Failure("Error inesperado al desactivar penalización.");
            }
        }

        public async Task<OperationResult<PenalizacionResponseDto>> CalcularPenalizacionPorRetrasoAsync(int idPrestamo)
        {
            _logger.Info("Iniciando cálculo de penalización por retraso para préstamo ID {0}", idPrestamo);

            // 1. Validación de negocio
            var validation = await _businessValidator.ValidateForCalcularPenalizacionAsync(idPrestamo);
            if (!validation.IsSuccess)
            {
                _logger.Error("Validación de negocio falló para cálculo de penalización: {0}", validation.Message);
                return OperationResult<PenalizacionResponseDto>.Failure(validation.Message);
            }

            try
            {
                // 2. Obtener el préstamo
                var prestamoResult = await _prestamoRepository.GetByIdAsync(idPrestamo);
                if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                {
                    _logger.Error("Préstamo no encontrado con ID {0}", idPrestamo);
                    return OperationResult<PenalizacionResponseDto>.Failure("Préstamo no encontrado.");
                }

                var prestamo = prestamoResult.Data;
                _logger.Info("Préstamo encontrado: Usuario {0}, Fecha fin {1}, Fecha devolución {2}",
                    prestamo.UsuarioId, prestamo.FechaFin, prestamo.FechaDevolucion);

                // 3. Calcular días de retraso
                var diasRetrasados = (prestamo.FechaDevolucion.Value - prestamo.FechaFin).Days;
                diasRetrasados = diasRetrasados <= 0 ? 1 : diasRetrasados;

                // 4. Calcular monto
                var montoPorDia = decimal.Parse(_configuration["Penalizacion:MontoPorDiaRetraso"] ?? "10");
                var montoFinal = diasRetrasados * montoPorDia;

                _logger.Info("Cálculo de penalización: {0} días x ${1} = ${2}",
                    diasRetrasados, montoPorDia, montoFinal);

                // 5. Crear entidad penalización
                var penalizacion = new Penalizacion(
                    idUsuario: prestamo.UsuarioId,
                    motivo: "Retraso en la devolución del préstamo.",
                    fechaInicio: prestamo.FechaDevolucion.Value,
                    fechaFin: prestamo.FechaDevolucion.Value.AddDays(30),
                    idPrestamo: prestamo.Id,
                    monto: montoFinal
                );

                // 6. Guardar
                var result = await _penalizacionRepository.AddAsync(penalizacion);
                if (!result.IsSuccess)
                {
                    _logger.Error("Error al guardar penalización calculada: {0}", result.Message);
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message);
                }

                var dto = _mapper.MapToDto(penalizacion);
                _logger.Info("Penalización por retraso creada exitosamente: ID {0}, Monto ${1}",
                    penalizacion.Id, montoFinal);

                return OperationResult<PenalizacionResponseDto>.Success(dto, "Penalización creada correctamente por retraso.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al calcular penalización para préstamo ID {0}", idPrestamo);
                return OperationResult<PenalizacionResponseDto>.Failure("Ocurrió un error al generar la penalización.");
            }
        }

        public async Task<OperationResult<IEnumerable<PenalizacionResponseDto>>> GetAllAsync()
        {
            _logger.Info("Iniciando obtención de todas las penalizaciones");

            try
            {
                var result = await _penalizacionRepository.GetAllAsync();

                if (!result.IsSuccess)
                {
                    _logger.Error("Error desde el repositorio al obtener penalizaciones: {0}", result.Message);
                    return OperationResult<IEnumerable<PenalizacionResponseDto>>.Failure("Error al obtener penalizaciones.");
                }

                if (result.Data == null || !result.Data.Any())
                {
                    _logger.Info("No hay penalizaciones registradas en la base de datos");
                    return OperationResult<IEnumerable<PenalizacionResponseDto>>.Failure("No se encontraron penalizaciones registradas.");
                }

                var data = result.Data.Select(_mapper.MapToDto).ToList();
                _logger.Info("Se obtuvieron {0} penalizaciones correctamente", data.Count);

                return OperationResult<IEnumerable<PenalizacionResponseDto>>.Success(data, "Penalizaciones obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al obtener todas las penalizaciones");
                return OperationResult<IEnumerable<PenalizacionResponseDto>>.Failure("Ocurrió un error inesperado al obtener los datos.");
            }
        }

        public async Task<OperationResult<PenalizacionResponseDto>> GetByIdAsync(int id)
        {
            _logger.Info("Buscando penalización por ID {0}", id);

            if (id <= 0)
            {
                _logger.Error("ID inválido proporcionado: {0}", id);
                return OperationResult<PenalizacionResponseDto>.Failure("ID inválido.");
            }

            try
            {
                var result = await _penalizacionRepository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                {
                    _logger.Error("Penalización no encontrada con ID {0}: {1}", id, result.Message ?? "Sin mensaje");
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message ?? "Penalización no encontrada.");
                }

                _logger.Info("Penalización encontrada exitosamente ID {0}", id);
                return OperationResult<PenalizacionResponseDto>.Success(_mapper.MapToDto(result.Data), "Penalización obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al buscar penalización por ID {0}", id);
                return OperationResult<PenalizacionResponseDto>.Failure("Error inesperado al buscar penalización.");
            }
        }

        public async Task<OperationResult<List<PenalizacionResponseDto>>> ObtenerPenalizacionesActivasPorUsuarioAsync(int usuarioId)
        {
            _logger.Info("Obteniendo penalizaciones activas para usuario ID {0}", usuarioId);

            if (usuarioId <= 0)
            {
                _logger.Error("ID de usuario inválido: {0}", usuarioId);
                return OperationResult<List<PenalizacionResponseDto>>.Failure("ID inválido.");
            }

            try
            {
                var result = await _penalizacionRepository.GetPenalizacionesActivasPorUsuarioAsync(usuarioId);
                if (!result.IsSuccess || result.Data == null)
                {
                    _logger.Error("Error al obtener penalizaciones activas para usuario {0}: {1}",
                        usuarioId, result.Message ?? "Sin mensaje");
                    return OperationResult<List<PenalizacionResponseDto>>.Failure(result.Message ?? "Error al obtener penalizaciones activas.");
                }

                var data = result.Data.Select(_mapper.MapToDto).ToList();
                _logger.Info("Se encontraron {0} penalizaciones activas para usuario {1}", data.Count, usuarioId);

                return OperationResult<List<PenalizacionResponseDto>>.Success(data, result.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al obtener penalizaciones activas para usuario {0}", usuarioId);
                return OperationResult<List<PenalizacionResponseDto>>.Failure("Error inesperado al obtener penalizaciones activas.");
            }
        }
    }
}