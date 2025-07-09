using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Base.ValidatorServices.Prestamos;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Prestamos;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using System;
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

    public PrestamoService(
        IPrestamoRepository prestamoRepository,
        ILogger<PrestamoService> logger,
        IConfiguration configuration,
        IValidator<AddPrestamoDto> addValidator,
        IValidator<UpdatePrestamoDto> updateValidator,
        IValidator<DiseblePrestamoDto> disableValidator,
        IPrestamoBusinessValidator businessValidator
    )
    {
        _prestamoRepository = prestamoRepository;
        _logger = logger;
        _configuration = configuration;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _disableValidator = disableValidator;
        _businessValidator = businessValidator;
    }

    public async Task<OperationResult> AddAsync(AddPrestamoDto dto)
    {
        var dtoValidation = await _addValidator.ValidateAsync(dto);
        if (!dtoValidation.IsValid)
            return new OperationResult { Success = false, Message = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)) };

        var businessValidation = await _businessValidator.ValidateForAddAsync(dto);
        if (!businessValidation.Success)
            return businessValidation;

        try
        {
            var prestamo = new Prestamo(dto.UsuarioId, dto.ISBN, dto.FechaInicio, dto.FechaFin);
            var result = await _prestamoRepository.AddAsync(prestamo);
            if (!result.Success) return result;

            return new OperationResult
            {
                Success = true,
                Message = "Préstamo registrado correctamente.",
                Data = new
                {
                    prestamo.Id,
                    prestamo.UsuarioId,
                    prestamo.ISBN,
                    prestamo.FechaInicio,
                    prestamo.FechaFin,
                    Estado = prestamo.Estado.ToString(),
                    prestamo.EstaActivo
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar préstamo");
            return new OperationResult { Success = false, Message = "Error inesperado al registrar préstamo." };
        }
    }

    public async Task<OperationResult> UpdateAsync(UpdatePrestamoDto dto)
    {
        var dtoValidation = await _updateValidator.ValidateAsync(dto);
        if (!dtoValidation.IsValid)
            return new OperationResult { Success = false, Message = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)) };

        var businessValidation = await _businessValidator.ValidateForUpdateAsync(dto);
        if (!businessValidation.Success)
            return businessValidation;

        try
        {
            var prestamo = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);
            if (dto.FechaFin.HasValue) prestamo.FechaFin = dto.FechaFin.Value;
            if (dto.FechaDevolucion.HasValue) prestamo.FechaDevolucion = dto.FechaDevolucion.Value;
            if (!string.IsNullOrWhiteSpace(dto.Estado))
                prestamo.Estado = Enum.Parse<EstadoPrestamo>(dto.Estado);

            return await _prestamoRepository.UpdateAsync(prestamo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar préstamo");
            return new OperationResult { Success = false, Message = "Error inesperado al actualizar préstamo." };
        }
    }

    public async Task<OperationResult> DeleteAsync(DiseblePrestamoDto dto)
    {
        var dtoValidation = await _disableValidator.ValidateAsync(dto);
        if (!dtoValidation.IsValid)
            return new OperationResult { Success = false, Message = string.Join("; ", dtoValidation.Errors.Select(e => e.ErrorMessage)) };

        var businessValidation = await _businessValidator.ValidateForDisableAsync(dto);
        if (!businessValidation.Success)
            return businessValidation;

        try
        {
            return await _prestamoRepository.DisableAsync(dto.IDPrestamo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al desactivar préstamo");
            return new OperationResult { Success = false, Message = "Error inesperado al desactivar préstamo." };
        }
    }

    public async Task<OperationResult> RegistrarDevolucionAsync(int idPrestamo)
    {
        var businessValidation = await _businessValidator.ValidateForRegistrarDevolucionAsync(idPrestamo);
        if (!businessValidation.Success)
            return businessValidation;

        try
        {
            var prestamo = await _prestamoRepository.GetByIdAsync(idPrestamo);
            prestamo.RegistrarDevolucion();
            return await _prestamoRepository.UpdateAsync(prestamo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar devolución");
            return new OperationResult { Success = false, Message = "Error inesperado al registrar devolución." };
        }
    }

    public async Task<OperationResult> ActualizarEstadoPrestamoPorVencimientoAsync(int idPrestamo)
    {
        try
        {
            var prestamo = await _prestamoRepository.GetByIdAsync(idPrestamo);
            if (prestamo == null)
                return new OperationResult { Success = false, Message = "Préstamo no encontrado." };

            prestamo.ActualizarEstadoSiEstaAtrasado();
            return await _prestamoRepository.UpdateAsync(prestamo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado por vencimiento");
            return new OperationResult { Success = false, Message = "Error inesperado al actualizar estado." };
        }
    }

    public async Task<OperationResult> ObtenerPrestamosActivosPorUsuarioAsync(int usuarioId)
    {
        var prestamos = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(usuarioId);
        var data = prestamos.Select(p => new
        {
            p.Id,
            p.UsuarioId,
            p.ISBN,
            p.FechaInicio,
            p.FechaFin,
            p.FechaDevolucion,
            Estado = p.Estado.ToString(),
            p.EstaActivo
        });

        return new OperationResult { Success = true, Data = data };
    }

    public async Task<OperationResult> GetAllAsync()
    {
        var prestamos = await _prestamoRepository.GetAllAsync();
        var data = prestamos.Select(p => new
        {
            p.Id,
            p.UsuarioId,
            p.ISBN,
            p.FechaInicio,
            p.FechaFin,
            p.FechaDevolucion,
            Estado = p.Estado.ToString(),
            p.EstaActivo
        });

        return new OperationResult { Success = true, Data = data };
    }

    public async Task<OperationResult> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new OperationResult { Success = false, Message = "ID inválido." };

        var prestamo = await _prestamoRepository.GetByIdAsync(id);
        if (prestamo == null)
            return new OperationResult { Success = false, Message = "Préstamo no encontrado." };

        return new OperationResult
        {
            Success = true,
            Data = new
            {
                prestamo.Id,
                prestamo.UsuarioId,
                prestamo.ISBN,
                prestamo.FechaInicio,
                prestamo.FechaFin,
                prestamo.FechaDevolucion,
                Estado = prestamo.Estado.ToString(),
                prestamo.EstaActivo
            }
        };
    }

    public async Task<bool> PuedePrestarAsync(int usuarioId)
    {
        var prestamos = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(usuarioId);
        return prestamos.All(p => p.Estado == EstadoPrestamo.Devuelto || p.Estado == EstadoPrestamo.DevueltoConAtraso);
    }
}
