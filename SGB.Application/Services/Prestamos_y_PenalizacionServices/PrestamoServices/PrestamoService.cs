using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGB.Domain.Base;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Prestamos;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SGB.Domain.Entities.Prestamos;
using SGB.Application.Contracts.Repository.Interfaces;

//por ahora




namespace SGB.Application.Services.Prestamos_y_PenalizacionServices.PrestamoServices
{

    public sealed class PrestamoService : IPrestamosServices

    {
        private readonly IPrestamoRepository _PrestamoRepository;
        private readonly ILogger<PrestamoService> _logger;
        private readonly IConfiguration _configuration;

        public PrestamoService(IPrestamoRepository prestamoRepository, ILogger<PrestamoService> logger, IConfiguration configuration)
        {
            _PrestamoRepository = prestamoRepository;
            _logger = logger;
            _configuration = configuration;


        }

        public async Task<OperationResult> AddPrestamoAsync(AddPrestamoDto addPrestamoDto)
        {
            if (addPrestamoDto == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = _configuration["ErrorMessages:Global:InvalidInput"] ?? "El objeto de entrada no puede ser nulo."
                };
            }

            if (addPrestamoDto.EjemplarId <= 0 || addPrestamoDto.UsuarioId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Los IDs de ejemplar y usuario deben ser mayores que cero."
                };
            }

            if (addPrestamoDto.DiasDePrestamo <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Los días de préstamo deben ser mayores que cero."
                };
            }

            try
            {
                var prestamo = new Prestamo(
                    addPrestamoDto.EjemplarId,
                    addPrestamoDto.UsuarioId,
                    addPrestamoDto.DiasDePrestamo
                );

                var result = await _PrestamoRepository.AddAsync(prestamo);

                if (!result.Success)
                {
                    _logger.LogWarning("No se pudo agregar el préstamo: {Mensaje}", result.Message);
                    return new OperationResult
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                _logger.LogInformation("Préstamo registrado: UsuarioId={UsuarioId}, EjemplarId={EjemplarId}",
                    prestamo.UsuarioId, prestamo.EjemplarId);

                return new OperationResult
                {
                    Success = true,
                    Data = prestamo,
                    Message = "Préstamo agregado correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el préstamo.");
                return new OperationResult
                {
                    Success = false,
                    Message = _configuration["ErrorMessages:Global:UnexpectedError"] ?? "Ocurrió un error al registrar el préstamo."
                };
            }
        }

        public async Task<OperationResult> DeletePrestamoAsync(DeletePrestamoDto deletePrestamoDto)
        {
            if (deletePrestamoDto == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El objeto DeletePrestamoDto no puede ser nulo."
                };
            }

            try
            {
                _logger.LogInformation("Iniciando eliminación del préstamo con ID: {Id}", deletePrestamoDto.IdPrestamo);

                var result = await _PrestamoRepository.DeleteAsync(deletePrestamoDto.IdPrestamo);

                if (!result.Success)
                {
                    _logger.LogError("Error al eliminar el préstamo: {Mensaje}", result.Message);
                    return new OperationResult
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                _logger.LogInformation("Préstamo con ID: {Id} eliminado correctamente.", deletePrestamoDto.IdPrestamo);

                return new OperationResult
                {
                    Success = true,
                    Message = "Préstamo eliminado correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al eliminar préstamo.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> GetAllPrestamosAsync(GetPrestamoDto getPrestamoDto)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los préstamos.");

                var result = await _PrestamoRepository.GetAllAsync();

                // Cambiar la verificación de éxito
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron préstamos.");
                    return new OperationResult
                    {
                        Success = false,
                        Message = "No se encontraron préstamos."
                    };
                }

                _logger.LogInformation("Préstamos obtenidos correctamente.");

                return new OperationResult
                {
                    Success = true,
                    Data = result,
                    Message = "Lista de préstamos obtenida correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al obtener los préstamos.");
                return new OperationResult
                {
                    Success = false,
                    Message = "Error inesperado al obtener los préstamos."
                };
            }
        }

        public async Task<OperationResult> GetPrestamoByIdAsync(int idPrestamo)
        {
            if (idPrestamo <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El ID del préstamo debe ser mayor a 0."
                };
            }

            try
            {
                _logger.LogInformation("Buscando préstamo con ID: {Id}", idPrestamo);

                var result = await _PrestamoRepository.GetByIdAsync(idPrestamo);

                // Cambiar la verificación de éxito
                if (result == null)
                {
                    _logger.LogWarning("Préstamo con ID {Id} no encontrado.", idPrestamo);
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Préstamo no encontrado."
                    };
                }

                _logger.LogInformation("Préstamo con ID: {Id} encontrado exitosamente.", idPrestamo);

                return new OperationResult
                {
                    Success = true,
                    Data = result,
                    Message = "Detalle del préstamo obtenido correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al obtener préstamo por ID.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> UpdatePrestamoAsync(UpdatePrestamoDto updatePrestamoDto)
        {
            if (updatePrestamoDto == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El objeto UpdatePrestamoDto no puede ser nulo."
                };
            }

            try
            {
                _logger.LogInformation("Actualizando préstamo con ID: {Id}", updatePrestamoDto.IDPrestamo);

                // Aquí obtienes directamente el objeto Prestamo o null
                var prestamoExistente = await _PrestamoRepository.GetByIdAsync(updatePrestamoDto.IDPrestamo);

                if (prestamoExistente == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "El préstamo que desea actualizar no existe."
                    };
                }

                // Actualizas los campos directamente
                prestamoExistente.FechaVencimiento = updatePrestamoDto.FechaVencimiento;
                prestamoExistente.FechaDevolucion = updatePrestamoDto.FechaDevolucion;
                prestamoExistente.Estado = (EstadoPrestamo)Enum.Parse(typeof(EstadoPrestamo), updatePrestamoDto.Estado);

                var result = await _PrestamoRepository.UpdateAsync(prestamoExistente);

                if (!result.Success)
                {
                    _logger.LogWarning("Error al actualizar préstamo: {Mensaje}", result.Message);
                    return new OperationResult
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                _logger.LogInformation("Préstamo con ID: {Id} actualizado correctamente.", updatePrestamoDto.IDPrestamo);

                return new OperationResult
                {
                    Success = true,
                    Data = prestamoExistente,
                    Message = "Préstamo actualizado correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al actualizar préstamo.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado al actualizar el préstamo: {ex.Message}"
                };
            }
        }

    }

}

 
