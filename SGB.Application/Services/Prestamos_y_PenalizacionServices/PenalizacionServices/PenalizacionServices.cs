using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Base.ValidatorServices.Penalizacion;

using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
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
        private readonly ILogger<PenalizacionService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IValidator<AddPenalizacionDto> _addValidator;
        private readonly IValidator<UpdatePenalizacionDto> _updateValidator;
        private readonly IValidator<DisablePenalizacionDto> _disableValidator;
        private readonly IPenalizacionBusinessValidator _businessValidator;
        private readonly IPenalizacionMapper _mapper;

        public PenalizacionService(
            IPenalizacionRepository penalizacionRepository,
            IPrestamoRepository prestamoRepository,
            ILogger<PenalizacionService> logger,
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
            var dtoValidation = await _addValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
                return OperationResult<PenalizacionResponseDto>.Failure(string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)));

            var businessValidation = await _businessValidator.ValidateForAddAsync(dto);
            if (!businessValidation.IsSuccess)
                return OperationResult<PenalizacionResponseDto>.Failure(businessValidation.Message);

            try
            {
                var penalizacion = _mapper.MapFromDto(dto);

                var result = await _penalizacionRepository.AddAsync(penalizacion);
                if (!result.IsSuccess)
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message);

                return OperationResult<PenalizacionResponseDto>.Success(_mapper.MapToDto(penalizacion), "Penalización registrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar penalización");
                return OperationResult<PenalizacionResponseDto>.Failure("Error inesperado al registrar penalización.");
            }
        }

        //camios
        public async Task<OperationResult<PenalizacionResponseDto>> UpdateAsync(UpdatePenalizacionDto dto)
        {
            var dtoValidation = await _updateValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
                return OperationResult<PenalizacionResponseDto>.Failure(string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)));

            var businessValidation = await _businessValidator.ValidateForUpdateAsync(dto);
            if (!businessValidation.IsSuccess)
                return OperationResult<PenalizacionResponseDto>.Failure(businessValidation.Message);

            try
            {
                var penalizacionResult = await _penalizacionRepository.GetByIdAsync(dto.IDPenalizacion);

                if (!penalizacionResult.IsSuccess || penalizacionResult.Data == null)
                    return OperationResult<PenalizacionResponseDto>.Failure("Penalización no encontrada.");

                var penalizacion = penalizacionResult.Data;

                _mapper.ApplyUpdateDto(penalizacion, dto);

                var result = await _penalizacionRepository.UpdateAsync(penalizacion);
                if (!result.IsSuccess)
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message);

                return OperationResult<PenalizacionResponseDto>.Success(_mapper.MapToDto(penalizacion), "Penalización actualizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar penalización");
                return OperationResult<PenalizacionResponseDto>.Failure("Error inesperado al actualizar penalización.");
            }
        }


        public async Task<OperationResult<bool>> DeleteAsync(DisablePenalizacionDto dto)
        {
            var dtoValidation = await _disableValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
                return OperationResult<bool>.Failure(string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)));

            var businessValidation = await _businessValidator.ValidateForDisableAsync(dto);
            if (!businessValidation.IsSuccess)
                return OperationResult<bool>.Failure(businessValidation.Message);

            try
            {
                var result = await _penalizacionRepository.DisableAsync(dto.IDPenalizacion);
                if (!result.IsSuccess)
                    return OperationResult<bool>.Failure(result.Message);

                return OperationResult<bool>.Success(true, "Penalización desactivada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar penalización");
                return OperationResult<bool>.Failure("Error inesperado al desactivar penalización.");
            }
        }


        //cambios
        public async Task<OperationResult<PenalizacionResponseDto>> CalcularPenalizacionPorRetrasoAsync(int idPrestamo)
        {
            // 1. Validación de negocio
            var validation = await _businessValidator.ValidateForCalcularPenalizacionAsync(idPrestamo);
            if (!validation.IsSuccess)
                return OperationResult<PenalizacionResponseDto>.Failure(validation.Message);

            try
            {
                // 2. Obtener el préstamo
                var prestamoResult = await _prestamoRepository.GetByIdAsync(idPrestamo);
                if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                    return OperationResult<PenalizacionResponseDto>.Failure("Préstamo no encontrado.");

                var prestamo = prestamoResult.Data;

                // 3. Calcular días de retraso
                var diasRetrasados = (prestamo.FechaDevolucion.Value - prestamo.FechaFin).Days;
                diasRetrasados = diasRetrasados <= 0 ? 1 : diasRetrasados;

                // 4. Calcular monto
                var montoPorDia = decimal.Parse(_configuration["Penalizacion:MontoPorDiaRetraso"] ?? "10");
                var montoFinal = diasRetrasados * montoPorDia;

                // 5. Crear entidad penalización
                var penalizacion = new Penalizacion(
                    idUsuario: prestamo.UsuarioId,
                    motivo: "Retraso en la devolución del préstamo.",
                    fechaInicio: prestamo.FechaDevolucion.Value,  // fecha real de entrega
                    fechaFin: prestamo.FechaDevolucion.Value.AddDays(30), // sanción por 30 días
                    idPrestamo: prestamo.Id,
                    monto: montoFinal
                );

                // 6. Guardar
                var result = await _penalizacionRepository.AddAsync(penalizacion);
                if (!result.IsSuccess)
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message);

                var dto = _mapper.MapToDto(penalizacion);
                return OperationResult<PenalizacionResponseDto>.Success(dto, "Penalización creada correctamente por retraso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular penalización.");
                return OperationResult<PenalizacionResponseDto>.Failure("Ocurrió un error al generar la penalización.");
            }
        }




        public async Task<OperationResult<IEnumerable<PenalizacionResponseDto>>> GetAllAsync()
        {
            try
            {
                var result = await _penalizacionRepository.GetAllAsync();

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error desde el repositorio: {Message}", result.Message);
                    return OperationResult<IEnumerable<PenalizacionResponseDto>>.Failure("Error al obtener penalizaciones.");
                }

                if (result.Data == null || !result.Data.Any())
                {
                    _logger.LogInformation("No hay penalizaciones registradas.");
                    return OperationResult<IEnumerable<PenalizacionResponseDto>>.Failure("No se encontraron penalizaciones registradas.");
                }

                var data = result.Data.Select(_mapper.MapToDto).ToList();
                _logger.LogInformation("Cantidad de penalizaciones mapeadas a DTO: {Count}", data.Count);

                return OperationResult<IEnumerable<PenalizacionResponseDto>>.Success(data, "Penalizaciones obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en GetAllAsync");
                return OperationResult<IEnumerable<PenalizacionResponseDto>>.Failure("Ocurrió un error inesperado al obtener los datos.");
            }
        }




        public async Task<OperationResult<PenalizacionResponseDto>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return OperationResult<PenalizacionResponseDto>.Failure("ID inválido.");

            try
            {
                var result = await _penalizacionRepository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<PenalizacionResponseDto>.Failure(result.Message ?? "Penalización no encontrada.");

                return OperationResult<PenalizacionResponseDto>.Success(_mapper.MapToDto(result.Data), "Penalización obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener penalización por ID");
                return OperationResult<PenalizacionResponseDto>.Failure("Error inesperado al buscar penalización.");
            }
        }




        public async Task<OperationResult<List<PenalizacionResponseDto>>> ObtenerPenalizacionesActivasPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                return OperationResult<List<PenalizacionResponseDto>>.Failure("ID inválido.");

            try
            {
                var result = await _penalizacionRepository.GetPenalizacionesActivasPorUsuarioAsync(usuarioId);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<List<PenalizacionResponseDto>>.Failure(result.Message ?? "Error al obtener penalizaciones activas.");

                var data = result.Data.Select(_mapper.MapToDto).ToList();

                return OperationResult<List<PenalizacionResponseDto>>.Success(data, result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener penalizaciones activas");
                return OperationResult<List<PenalizacionResponseDto>>.Failure("Error inesperado al obtener penalizaciones activas.");
            }
        }
    }
}
