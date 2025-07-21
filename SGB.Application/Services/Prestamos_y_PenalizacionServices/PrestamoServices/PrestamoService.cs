using FluentValidation;
using Microsoft.Extensions.Configuration;
using SGB.Application.Base.ValidatorServices.Prestamos;
using SGB.Application.Contracts.Interfaces.Mappers.PrestamoMappers;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Application.Loggers;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;

public sealed class PrestamoService : IPrestamosServices
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IAppLogger<PrestamoService> _logger; // Cambiado
    private readonly IConfiguration _configuration;
    private readonly IValidator<AddPrestamoDto> _addValidator;
    private readonly IValidator<UpdatePrestamoDto> _updateValidator;
    private readonly IValidator<DiseblePrestamoDto> _disableValidator;
    private readonly IValidator<RegistrarDevolucionDto> _registrarDevolucionValidator;
    private readonly IPrestamoBusinessValidator _businessValidator;
    private readonly IPrestamoMapper _mapper;
    private readonly IPenalizacionServices _penalizacionService;

    public PrestamoService(
        IPrestamoRepository prestamoRepository,
        IAppLogger<PrestamoService> logger, // Cambiado
        IConfiguration configuration,
        IValidator<AddPrestamoDto> addValidator,
        IValidator<UpdatePrestamoDto> updateValidator,
        IValidator<DiseblePrestamoDto> disableValidator,
        IValidator<RegistrarDevolucionDto> registrarDevolucionValidator,
        IPrestamoBusinessValidator businessValidator,
        IPrestamoMapper mapper,
        IPenalizacionServices penalizacionService
    )
    {
        _prestamoRepository = prestamoRepository;
        _logger = logger;
        _configuration = configuration;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _disableValidator = disableValidator;
        _registrarDevolucionValidator = registrarDevolucionValidator;
        _businessValidator = businessValidator;
        _mapper = mapper;
        _penalizacionService = penalizacionService;
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
            _logger.Info("Préstamo registrado: {@Dto}", responseDto);
            return OperationResult<PrestamoResponseDto>.Success(responseDto, "Préstamo registrado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al registrar préstamo");
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
            _logger.Info("Préstamo actualizado: {@Dto}", responseDto);
            return OperationResult<PrestamoResponseDto>.Success(responseDto, "Préstamo actualizado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al actualizar préstamo");
            return OperationResult<PrestamoResponseDto>.Failure("Error inesperado al actualizar préstamo.");
        }
    }

    public async Task<OperationResult<bool>> DeleteAsync(DiseblePrestamoDto dto)
    {
        var validation = await _disableValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return OperationResult<bool>.Failure(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var businessValidation = await _businessValidator.ValidateForDisableAsync(dto);
        if (!businessValidation.IsSuccess)
            return OperationResult<bool>.Failure(businessValidation.Message);

        try
        {
            var prestamoResult = await _prestamoRepository.GetByIdAsync(dto.IDPrestamo);
            if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
                return OperationResult<bool>.Failure("Préstamo no encontrado.");

            var prestamo = prestamoResult.Data;
            prestamo.Deshabilitar();

            var updateResult = await _prestamoRepository.UpdateAsync(prestamo);
            if (!updateResult.IsSuccess)
                return OperationResult<bool>.Failure(updateResult.Message);

            _logger.Info("Préstamo desactivado: {Id}", dto.IDPrestamo);
            return OperationResult<bool>.Success(true, "Préstamo desactivado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al desactivar préstamo");
            return OperationResult<bool>.Failure("Error inesperado al desactivar préstamo.");
        }
    }

    public async Task<OperationResult<string>> RegistrarDevolucionAsync(RegistrarDevolucionDto dto)
    {
        // 1. Validación básica del DTO (estructura, formatos)
        var validation = await _registrarDevolucionValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return OperationResult<string>.Failure(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        // 2. Validación de reglas de negocio
        var businessValidation = await _businessValidator.ValidateForRegistrarDevolucionAsync(dto);
        if (!businessValidation.IsSuccess)
            return OperationResult<string>.Failure(businessValidation.Message);

        // 3. Obtener el préstamo y registrar la devolución
        var prestamoResult = await _prestamoRepository.GetByIdAsync(dto.IdPrestamo);
        if (!prestamoResult.IsSuccess || prestamoResult.Data == null)
            return OperationResult<string>.Failure("Préstamo no encontrado.");

        var prestamo = prestamoResult.Data;

        try
        {
            prestamo.RegistrarDevolucion(dto.FechaDevolucion);
            var updateResult = await _prestamoRepository.UpdateAsync(prestamo);
            if (!updateResult.IsSuccess)
                return OperationResult<string>.Failure(updateResult.Message);

            string mensaje = "Devolución registrada correctamente.";

            if (prestamo.Estado == EstadoPrestamo.DevueltoConAtraso)
            {
                var penalizacionResult = await _penalizacionService.CalcularPenalizacionPorRetrasoAsync(dto.IdPrestamo);

                if (penalizacionResult.IsSuccess)
                {
                    mensaje += $" Penalización generada: {penalizacionResult.Message}";
                    _logger.Info("Penalización creada para préstamo {Id}", dto.IdPrestamo);
                }
                else
                {
                    mensaje += " Devolución tardía detectada, pero no se pudo generar penalización.";
                    _logger.Error("No se generó penalización para préstamo {Id}: {Error}", dto.IdPrestamo, penalizacionResult.Message);
                }
            }

            return OperationResult<string>.Success(mensaje);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al registrar devolución.");
            return OperationResult<string>.Failure("Ocurrió un error inesperado al registrar la devolución.");
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

            _logger.Info("Estado actualizado por vencimiento para préstamo {Id}", idPrestamo);
            return OperationResult<string>.Success("Estado actualizado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al actualizar estado por vencimiento");
            return OperationResult<string>.Failure("Error inesperado al actualizar estado.");
        }
    }

    public async Task<OperationResult<IList<PrestamoResponseDto>>> ObtenerPrestamosActivosPorUsuarioAsync(int usuarioId)
    {
        var result = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(usuarioId);
        if (!result.IsSuccess)
            return OperationResult<IList<PrestamoResponseDto>>.Failure(result.Message);

        var dtos = result.Data.Select(_mapper.MapToDto).ToList();
        _logger.Info("Prestamos activos para usuario {UsuarioId}: {Cantidad}", usuarioId, dtos.Count);
        return OperationResult<IList<PrestamoResponseDto>>.Success(dtos);
    }

    public async Task<OperationResult<IEnumerable<PrestamoResponseDto>>> GetAllAsync()
    {
        try
        {
            var result = await _prestamoRepository.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.Error("Error desde el repositorio: {Message}", result.Message);
                return OperationResult<IEnumerable<PrestamoResponseDto>>.Failure("Error al obtener préstamos.");
            }

            if (result.Data == null || !result.Data.Any())
            {
                _logger.Info("No hay préstamos registrados.");
                return OperationResult<IEnumerable<PrestamoResponseDto>>.Failure("No se encontraron préstamos registrados.");
            }

            var dtos = result.Data.Select(_mapper.MapToDto).ToList();
            _logger.Info("Cantidad de préstamos mapeados a DTO: {Count}", dtos.Count);

            return OperationResult<IEnumerable<PrestamoResponseDto>>.Success(dtos, "Préstamos obtenidos correctamente.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error inesperado al obtener préstamos.");
            return OperationResult<IEnumerable<PrestamoResponseDto>>.Failure("Ocurrió un error inesperado al obtener los préstamos.");
        }
    }

    public async Task<OperationResult<PrestamoResponseDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return OperationResult<PrestamoResponseDto>.Failure("ID inválido.");

        var result = await _prestamoRepository.GetByIdAsync(id);
        if (!result.IsSuccess || result.Data == null)
            return OperationResult<PrestamoResponseDto>.Failure("Préstamo no encontrado.");

        var dto = _mapper.MapToDto(result.Data);
        _logger.Info("Préstamo obtenido por ID {Id}", id);
        return OperationResult<PrestamoResponseDto>.Success(dto);
    }

    public async Task<OperationResult<bool>> PuedePrestarAsync(int usuarioId)
    {
        var result = await _prestamoRepository.GetPrestamosActivosPorUsuarioAsync(usuarioId);
        if (!result.IsSuccess)
            return OperationResult<bool>.Failure(result.Message);

        var puede = result.Data.All(p => p.Estado == EstadoPrestamo.Devuelto || p.Estado == EstadoPrestamo.DevueltoConAtraso);
        _logger.Info("Evaluación de posibilidad de préstamo para usuario {UsuarioId}: {Resultado}", usuarioId, puede);
        return OperationResult<bool>.Success(puede);
    }
}
