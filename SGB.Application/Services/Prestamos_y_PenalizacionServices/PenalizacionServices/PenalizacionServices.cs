using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Penalizacion;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PrestamoServices;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Services.Prestamos_y_PenalizacionServices.PenalizacionServices
{


    public sealed class PenalizacionServices : IPenalizacionServices
    {
        private readonly IPenalizacionRepository _PenalizacionRepository;

        private readonly ILogger<PenalizacionServices> _logger;
        private readonly IConfiguration _configuration;
        public PenalizacionServices(IPenalizacionRepository PenalizacionRepository, ILogger<PenalizacionServices> logger, IConfiguration configuration)
        {
            _PenalizacionRepository = PenalizacionRepository;
            _logger = logger;
            _configuration = configuration;


        }

        public async Task<OperationResult> AddPenalizacionAsync(AddPenalizacionDto addPenalizacionDto)
        {
            if (addPenalizacionDto == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El objeto AddPenalizacionDto no puede ser nulo."
                };
            }

            _logger.LogInformation("Iniciando proceso para agregar penalización al usuario ID: {UsuarioId}", addPenalizacionDto.UsuarioId);

            try
            {
                var penalizacion = new Penalizacion(
                    addPenalizacionDto.UsuarioId,
                    addPenalizacionDto.Motivo,
                    addPenalizacionDto.FechaInicio,
                    addPenalizacionDto.FechaFin,
                    addPenalizacionDto.EstaActiva
                );

                var result = await _PenalizacionRepository.AddAsync(penalizacion);

                if (!result.Success)
                {
                    _logger.LogError("Error al agregar la penalización: {Mensaje}", result.Message);
                    return new OperationResult
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                _logger.LogInformation("Penalización para usuario ID: {UsuarioId} agregada correctamente.", addPenalizacionDto.UsuarioId);

                return new OperationResult
                {
                    Success = true,
                    Data = penalizacion,
                    Message = "Penalización agregada correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al agregar penalización.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> DeletePenalizacionAsync(DeletePenalizacionDto deletePenalizacionDto)
        {
            if (deletePenalizacionDto == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El objeto DeletePenalizacionDto no puede ser nulo."
                };
            }

            _logger.LogInformation("Iniciando eliminación de penalización con ID: {Id}", deletePenalizacionDto.IDPenalizacion);

            try
            {
                var result = await _PenalizacionRepository.DeleteAsync(deletePenalizacionDto.IDPenalizacion);

                if (!result.Success)
                {
                    _logger.LogError("Error al eliminar la penalización: {Mensaje}", result.Message);
                    return new OperationResult
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                _logger.LogInformation("Penalización con ID: {Id} eliminada correctamente.", deletePenalizacionDto.IDPenalizacion);

                return new OperationResult
                {
                    Success = true,
                    Message = "Penalización eliminada correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al eliminar penalización.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> GetAllPenalizacionesAsync(GetPenalizacionDto getPenalizacionDto)
        {
            if (getPenalizacionDto == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El objeto GetPenalizacionDto no puede ser nulo."
                };
            }

            _logger.LogInformation("Obteniendo penalizaciones según los criterios proporcionados.");

            try
            {
                var result = await _PenalizacionRepository.GetAllAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron penalizaciones.");
                    return new OperationResult
                    {
                        Success = false,
                        Message = "No se encontraron penalizaciones."
                    };
                }

                _logger.LogInformation("Listado de penalizaciones obtenido correctamente.");

                return new OperationResult
                {
                    Success = true,
                    Data = result,
                    Message = "Penalizaciones obtenidas correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al obtener penalizaciones.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> GetPenalizacionByIdAsync(int idPenalizacion)
        {
            if (idPenalizacion <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El ID de la penalización debe ser mayor a 0."
                };
            }

            _logger.LogInformation("Buscando penalización con ID: {Id}", idPenalizacion);

            try
            {
                var result = await _PenalizacionRepository.GetByIdAsync(idPenalizacion);

                if (result == null)
                {
                    _logger.LogWarning("Penalización con ID {Id} no encontrada.", idPenalizacion);
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Penalización no encontrada."
                    };
                }

                _logger.LogInformation("Penalización con ID: {Id} encontrada correctamente.", idPenalizacion);

                return new OperationResult
                {
                    Success = true,
                    Data = result,
                    Message = "Detalle de penalización obtenido correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al obtener penalización por ID.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> UpdatePenalizacionAsync(UpdatePenalizacionDto updatePenalizacionDto)
        {
            if (updatePenalizacionDto == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El objeto UpdatePenalizacionDto no puede ser nulo."
                };
            }

            try
            {
                _logger.LogInformation("Actualizando penalización con ID: {Id}", updatePenalizacionDto.IDPenalizacion);

                // Obtienes directamente la entidad penalización o null
                var penalizacionExistente = await _PenalizacionRepository.GetByIdAsync(updatePenalizacionDto.IDPenalizacion);

                if (penalizacionExistente == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "La penalización que desea actualizar no existe."
                    };
                }

                // Actualizas los campos directamente
                penalizacionExistente.FechaVencimiento = updatePenalizacionDto.FechaVencimiento;
                penalizacionExistente.FechaDevolucion = updatePenalizacionDto.FechaDevolucion;
                penalizacionExistente.Estado = updatePenalizacionDto.Estado;

                var result = await _PenalizacionRepository.UpdateAsync(penalizacionExistente);

                if (!result.Success)
                {
                    _logger.LogWarning("Error al actualizar penalización: {Mensaje}", result.Message);
                    return new OperationResult
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                _logger.LogInformation("Penalización con ID: {Id} actualizada correctamente.", updatePenalizacionDto.IDPenalizacion);

                return new OperationResult
                {
                    Success = true,
                    Data = penalizacionExistente,
                    Message = "Penalización actualizada correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al actualizar penalización.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado al actualizar la penalización: {ex.Message}"
                };
            }
        }
    }
}
