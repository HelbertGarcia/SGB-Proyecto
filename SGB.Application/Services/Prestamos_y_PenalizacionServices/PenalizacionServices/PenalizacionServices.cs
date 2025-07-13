using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Base.ValidatorServices.Penalizacion;
using SGB.Application.Contracts.Interfaces.Mappers.PenalizacionMappers;
using SGB.Application.Contracts.Interfaces.Service.IPrestamos_PenalizacionServices.Penalizacion;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using System;
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

        public async Task<OperationResult> AddAsync(AddPenalizacionDto dto)
        {
            var dtoValidation = await _addValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
                return new OperationResult { Success = false, Message = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)) };

            var businessValidation = await _businessValidator.ValidateForAddAsync(dto);
            if (!businessValidation.Success)
                return businessValidation;

            try
            {
                var penalizacion = _mapper.MapFromDto(dto);

                var result = await _penalizacionRepository.AddAsync(penalizacion);

                if (!result.Success)
                    return result;

                return new OperationResult
                {
                    Success = true,
                    Message = "Penalización registrada correctamente.",
                    Data = _mapper.MapToDto(penalizacion)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar penalización");
                return new OperationResult { Success = false, Message = "Error inesperado al registrar penalización." };
            }
        }

        public async Task<OperationResult> UpdateAsync(UpdatePenalizacionDto dto)
        {
            var dtoValidation = await _updateValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
                return new OperationResult { Success = false, Message = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)) };

            var businessValidation = await _businessValidator.ValidateForUpdateAsync(dto);
            if (!businessValidation.Success)
                return businessValidation;

            try
            {
                var penalizacion = await _penalizacionRepository.GetByIdAsync(dto.IDPenalizacion);
                _mapper.ApplyUpdateDto(penalizacion, dto);

                var result = await _penalizacionRepository.UpdateAsync(penalizacion);
                if (!result.Success)
                    return result;

                return new OperationResult
                {
                    Success = true,
                    Message = "Penalización actualizada correctamente.",
                    Data = _mapper.MapToDto(penalizacion)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar penalización");
                return new OperationResult { Success = false, Message = "Error inesperado al actualizar penalización." };
            }
        }

        public async Task<OperationResult> DeleteAsync(DisablePenalizacionDto dto)
        {
            var dtoValidation = await _disableValidator.ValidateAsync(dto);
            if (!dtoValidation.IsValid)
                return new OperationResult { Success = false, Message = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)) };

            var businessValidation = await _businessValidator.ValidateForDisableAsync(dto);
            if (!businessValidation.Success)
                return businessValidation;

            try
            {
                var result = await _penalizacionRepository.DisableAsync(dto.IDPenalizacion);
                return result.Success
                    ? new OperationResult { Success = true, Message = "Penalización desactivada correctamente." }
                    : result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar penalización");
                return new OperationResult { Success = false, Message = "Error inesperado al desactivar penalización." };
            }
        }

        public async Task<OperationResult> CalcularPenalizacionPorRetrasoAsync(int idPrestamo)
        {
            var businessValidation = await _businessValidator.ValidateForCalcularPenalizacionAsync(idPrestamo);
            if (!businessValidation.Success)
                return businessValidation;

            try
            {
                var prestamo = await _prestamoRepository.GetByIdAsync(idPrestamo);
                var diasRetraso = (prestamo.FechaDevolucion.Value - prestamo.FechaFin).Days;
                var fechaInicio = prestamo.FechaFin.AddDays(1);
                var fechaFin = fechaInicio.AddDays(diasRetraso);

                var penalizacion = new Penalizacion(
                    prestamo.UsuarioId,
                    $"Retraso de {diasRetraso} día(s) en devolución del libro ISBN {prestamo.ISBN}",
                    fechaInicio,
                    fechaFin
                );

                var result = await _penalizacionRepository.AddAsync(penalizacion);
                if (!result.Success)
                    return result;

                return new OperationResult
                {
                    Success = true,
                    Message = "Penalización generada correctamente.",
                    Data = _mapper.MapToDto(penalizacion)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular penalización");
                return new OperationResult { Success = false, Message = "Error inesperado al calcular penalización." };
            }
        }

        public async Task<OperationResult> GetAllAsync()
        {
            try
            {
                var penalizaciones = await _penalizacionRepository.GetAllAsync();
                var data = penalizaciones.Select(_mapper.MapToDto).ToList();

                return new OperationResult { Success = true, Data = data, Message = "Penalizaciones obtenidas correctamente." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener penalizaciones");
                return new OperationResult { Success = false, Message = "Error inesperado al obtener penalizaciones." };
            }
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            if (id <= 0)
                return new OperationResult { Success = false, Message = "ID inválido." };

            try
            {
                var penalizacion = await _penalizacionRepository.GetByIdAsync(id);
                if (penalizacion == null)
                    return new OperationResult { Success = false, Message = "Penalización no encontrada." };

                return new OperationResult
                {
                    Success = true,
                    Message = "Penalización obtenida correctamente.",
                    Data = _mapper.MapToDto(penalizacion)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener penalización por ID");
                return new OperationResult { Success = false, Message = "Error inesperado al buscar penalización." };
            }
        }

        public async Task<OperationResult> ObtenerPenalizacionesActivasPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                return new OperationResult { Success = false, Message = "ID inválido." };

            try
            {
                var result = await _penalizacionRepository.GetPenalizacionesActivasPorUsuarioAsync(usuarioId);
                return new OperationResult { Success = true, Message = result.Message, Data = result.Data };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener penalizaciones activas");
                return new OperationResult { Success = false, Message = "Error inesperado al obtener penalizaciones activas." };
            }
        }
    }
}
