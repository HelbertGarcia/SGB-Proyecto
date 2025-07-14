using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Base.ValidatorServices.Prestamos;
using SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public sealed class PrestamoService : IPrestamosServices
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly ILogger<PrestamoService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IValidator<AddPrestamoDto> _addValidator;
    private readonly IValidator<UpdatePrestamoDto> _updateValidator;
    private readonly IValidator<DiseblePrestamoDto> _disableValidator;
    private readonly IPrestamoBusinessValidator _businessValidator;
    private readonly IPrestamoMapper _mapper;

    public PrestamoService(
        IPrestamoRepository prestamoRepository,
        ILogger<PrestamoService> logger,
        IConfiguration configuration,
        IValidator<AddPrestamoDto> addValidator,
        IValidator<UpdatePrestamoDto> updateValidator,
        IValidator<DiseblePrestamoDto> disableValidator,
        IPrestamoBusinessValidator businessValidator,
        IPrestamoMapper mapper
    )
    {
        _prestamoRepository = prestamoRepository;
        _logger = logger;
        _configuration = configuration;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _disableValidator = disableValidator;
        _businessValidator = businessValidator;
        _mapper = mapper;
    }



    public async Task<OperationResult<PrestamoResponseDto>> AddAsync(AddPrestamoDto dto)
    {
        var validation = await _addValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return OperationResult<PrestamoResponseDto>.Failure(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var businessValidation = await _businessValidator.ValidateForAddAsync(dto);
        if (!businessValidation.IsSuccess)
            return OperationResult<PrestamoResponseDto>.Failure(businessValidation.Message);

        try
        {
            var prestamo = _mapper.MapFromAddDto(dto);
            var result = await _prestamoRepository.AddAsync(prestamo);
            if (!result.IsSuccess)
                return OperationResult<PrestamoResponseDto>.Failure(result.Message);

            var responseDto = _mapper.MapToDto(prestamo);
            return OperationResult<PrestamoResponseDto>.Success(responseDto, "Préstamo registrado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar préstamo");
            return OperationResult<PrestamoResponseDto>.Failure("Error inesperado al registrar préstamo.");
        }
    }




    public async Task<OperationResult<PrestamoResponseDto>> UpdateAsync(UpdatePrestamoDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return OperationResult<PrestamoResponseDto>.Failure(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var businessValidation = await _businessValidator.ValidateForUpdateAsync(dto);
        if (!businessValidation.IsSuccess)
            return OperationResult<PrestamoResponseDto>.Failure(businessValidation.Message);

        try
        {
            var prestamoResult = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);
            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<PrestamoResponseDto>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;
            _mapper.ApplyUpdateDto(prestamo, dto);

            var updateResult = await _prestamoRepository.UpdateAsync(prestamo);
            if (!updateResult.IsSuccess)
                return OperationResult<PrestamoResponseDto>.Failure(updateResult.Message);

            var responseDto = _mapper.MapToDto(prestamo);
            return OperationResult<PrestamoResponseDto>.Success(responseDto, "Préstamo actualizado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar préstamo");
            return OperationResult<PrestamoResponseDto>.Failure("Error inesperado al actualizar préstamo.");
        }
    }




    public async Task<OperationResult<bool>> DeleteAsync(DiseblePrestamoDto dto)
    {
        // Validación del DTO (por ejemplo: ID válido)
        var validation = await _disableValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return OperationResult<bool>.Failure(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))
            );

        // Validación de negocio (por ejemplo: que el préstamo no esté ya devuelto, etc.)
        var businessValidation = await _businessValidator.ValidateForDisableAsync(dto);
        if (!businessValidation.IsSuccess)
            return OperationResult<bool>.Failure(businessValidation.Message);

        try
        {
            // Buscar préstamo por ID
            var prestamoResult = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);
            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<bool>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;

            // Deshabilitar préstamo (lógica dentro de la entidad)
            prestamo.Deshabilitar();

            // Guardar cambios en la base de datos
            var updateResult = await _prestamoRepository.UpdateAsync(prestamo);
            if (!updateResult.IsSuccess)
                return OperationResult<bool>.Failure(updateResult.Message);

            return OperationResult<bool>.Success(true, "Préstamo desactivado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al desactivar préstamo");
            return OperationResult<bool>.Failure("Error inesperado al desactivar préstamo.");
        }
    }




    public async Task<OperationResult<string>> RegistrarDevolucionAsync(int idPrestamo)
    {
        var businessValidation = await _businessValidator.ValidateForRegistrarDevolucionAsync(idPrestamo);
        if (!businessValidation.IsSuccess)
            return OperationResult<string>.Failure(businessValidation.Message);

        try
        {
            var prestamoResult = await _prestamoRepository.GetByIdAsync(idPrestamo);
            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<string>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;
            prestamo.RegistrarDevolucion();

            var updateResult = await _prestamoRepository.UpdateAsync(prestamo);
            if (!updateResult.IsSuccess)
                return OperationResult<string>.Failure(updateResult.Message);

            return OperationResult<string>.Success("Devolución registrada correctamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar devolución");
            return OperationResult<string>.Failure("Error inesperado al registrar devolución.");
        }
    }





    public async Task<OperationResult<string>> ActualizarEstadoPrestamoPorVencimientoAsync(int idPrestamo)
    {
        try
        {
            var prestamoResult = await _prestamoRepository.GetByIdAsync(idPrestamo);
            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<string>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;
            prestamo.ActualizarEstadoSiEstaAtrasado();

            var updateResult = await _prestamoRepository.UpdateAsync(prestamo);
            if (!updateResult.IsSuccess)
                return OperationResult<string>.Failure(updateResult.Message);

            return OperationResult<string>.Success("Estado actualizado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado por vencimiento");
            return OperationResult<string>.Failure("Error inesperado al actualizar estado.");
        }
    }





    public async Task<OperationResult<IList<PrestamoResponseDto>>> ObtenerPrestamosActivosPorUsuarioAsync(int usuarioId)
    {
        var result = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(usuarioId);
        if (!result.IsSuccess)
            return OperationResult<IList<PrestamoResponseDto>>.Failure(result.Message);

        var dtos = result.Data.Select(_mapper.MapToDto).ToList();
        return OperationResult<IList<PrestamoResponseDto>>.Success(dtos);
    }







    public async Task<OperationResult<IEnumerable<PrestamoResponseDto>>> GetAllAsync()
    {
        var result = await _prestamoRepository.GetAllAsync();
        if (!result.IsSuccess)
            return OperationResult<IEnumerable<PrestamoResponseDto>>.Failure(result.Message);

        var dtos = result.Data.Select(_mapper.MapToDto);
        return OperationResult<IEnumerable<PrestamoResponseDto>>.Success(dtos);
    }






    public async Task<OperationResult<PrestamoResponseDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return OperationResult<PrestamoResponseDto>.Failure("ID inválido.");

        var result = await _prestamoRepository.GetByIdAsync(id);
        if (!result.IsSuccess || result.Data == null)
            return OperationResult<PrestamoResponseDto>.Failure("Préstamo no encontrado.");

        var dto = _mapper.MapToDto(result.Data);
        return OperationResult<PrestamoResponseDto>.Success(dto);
    }


    public async Task<OperationResult<bool>> PuedePrestarAsync(int usuarioId)
    {
        var result = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(usuarioId);
        if (!result.IsSuccess)
            return OperationResult<bool>.Failure(result.Message);

        var puede = result.Data.All(p => p.Estado == EstadoPrestamo.Devuelto || p.Estado == EstadoPrestamo.DevueltoConAtraso);
        return OperationResult<bool>.Success(puede);
    }
}
