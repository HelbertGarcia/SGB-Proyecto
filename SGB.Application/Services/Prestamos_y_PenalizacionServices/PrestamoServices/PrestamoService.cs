using SGB.Domain.Base;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Prestamos;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SGB.Domain.Entities.Prestamos;
using SGB.Application.Contracts.Repository.Interfaces;

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
                return new OperationResult { Success = false, Message = "El objeto AddPrestamoDto no puede ser nulo." };

            if (addPrestamoDto.EjemplarId <= 0)
                return new OperationResult { Success = false, Message = "El ID del ejemplar no es válido." };

            if (addPrestamoDto.UsuarioId <= 0)
                return new OperationResult { Success = false, Message = "El ID del usuario no es válido." };

            if (addPrestamoDto.DiasDePrestamo <= 0)
                return new OperationResult { Success = false, Message = "Los días de préstamo deben ser mayores que cero." };

            if (string.IsNullOrWhiteSpace(addPrestamoDto.ISBN) || addPrestamoDto.ISBN.Length > 13)
                return new OperationResult { Success = false, Message = "El ISBN es inválido." };

            try
            {
                var prestamo = new Prestamo(
                    addPrestamoDto.EjemplarId,
                    addPrestamoDto.UsuarioId,
                    addPrestamoDto.DiasDePrestamo,
                    addPrestamoDto.ISBN
                );

                prestamo.EstaActivo = true; // Asegura que se guarde correctamente

                var result = await _PrestamoRepository.AddAsync(prestamo);

                if (!result.Success)
                {
                    _logger.LogError("Error al agregar el préstamo: {Mensaje}", result.Message);
                    return new OperationResult { Success = false, Message = result.Message };
                }

                _logger.LogInformation("Préstamo para usuario ID: {UsuarioId} agregado correctamente.", addPrestamoDto.UsuarioId);

                return new OperationResult
                {
                    Success = true,
                    Data = new
                    {
                        Id = prestamo.Id,
                        ISBN = prestamo.ISBN,
                        EjemplarId = prestamo.EjemplarId,
                        UsuarioId = prestamo.UsuarioId,
                        FechaInicio = prestamo.FechaInicio,
                        FechaFin = prestamo.FechaFin,
                        FechaDevolucion = prestamo.FechaDevolucion,
                        Estado = prestamo.Estado.ToString(),
                        EstaActivo = prestamo.EstaActivo
                    },
                    Message = "Préstamo agregado correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al agregar préstamo.");
                return new OperationResult { Success = false, Message = $"Error inesperado: {ex.Message}" };
            }
        }

        public async Task<OperationResult> DisablePrestamoAsync(int idPrestamo)
        {
            if (idPrestamo <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El ID del préstamo debe ser mayor a cero."
                };
            }

            try
            {
                _logger.LogInformation("Desactivando préstamo con ID: {Id}", idPrestamo);

                var result = await _PrestamoRepository.DisableAsync(idPrestamo);

                if (!result.Success)
                {
                    _logger.LogWarning("Error al desactivar préstamo: {Mensaje}", result.Message);
                    return result;
                }

                _logger.LogInformation("Préstamo con ID {Id} desactivado correctamente.", idPrestamo);

                return new OperationResult
                {
                    Success = true,
                    Message = "Préstamo desactivado correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al desactivar préstamo.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }


        public async Task<OperationResult> GetAllPrestamosAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los préstamos.");

                var prestamosEntidades = await _PrestamoRepository.GetAllAsync();

                var prestamosDto = prestamosEntidades.Select(p => new
                {
                    Id = p.Id,
                    ISBN = p.ISBN,
                    EjemplarId = p.EjemplarId,
                    UsuarioId = p.UsuarioId,
                    FechaInicio = p.FechaInicio,
                    FechaFin = p.FechaFin,
                    FechaDevolucion = p.FechaDevolucion,
                    Estado = p.Estado.ToString(),
                    EstaActivo = p.EstaActivo
                }).ToList();

                _logger.LogInformation("Préstamos obtenidos correctamente.");

                return new OperationResult
                {
                    Success = true,
                    Data = prestamosDto,
                    Message = "Lista de préstamos obtenida correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al obtener los préstamos.");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}",
                    Data = ex.StackTrace
                };
            }
        }

        public async Task<OperationResult> GetPrestamoByIdAsync(int idPrestamo)
        {
            if (idPrestamo <= 0)
                return new OperationResult { Success = false, Message = "El ID del préstamo debe ser mayor a 0." };

            try
            {
                _logger.LogInformation("Buscando préstamo con ID: {Id}", idPrestamo);

                var result = await _PrestamoRepository.GetByIdAsync(idPrestamo);

                if (result == null)
                {
                    _logger.LogWarning("Préstamo con ID {Id} no encontrado.", idPrestamo);
                    return new OperationResult { Success = false, Message = "Préstamo no encontrado." };
                }

                _logger.LogInformation("Préstamo con ID: {Id} encontrado exitosamente.", idPrestamo);

                return new OperationResult
                {
                    Success = true,
                    Data = new
                    {
                        Id = result.Id,
                        ISBN = result.ISBN,
                        EjemplarId = result.EjemplarId,
                        UsuarioId = result.UsuarioId,
                        FechaInicio = result.FechaInicio,
                        FechaFin = result.FechaFin,
                        FechaDevolucion = result.FechaDevolucion,
                        Estado = result.Estado.ToString(),
                        EstaActivo = result.EstaActivo
                    },
                    Message = "Detalle del préstamo obtenido correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al obtener préstamo por ID.");
                return new OperationResult { Success = false, Message = $"Error inesperado: {ex.Message}" };
            }
        }

        public async Task<OperationResult> UpdatePrestamoAsync(UpdatePrestamoDto updatePrestamoDto)
        {
            if (updatePrestamoDto == null)
                return new OperationResult { Success = false, Message = "El objeto UpdatePrestamoDto no puede ser nulo." };

            try
            {
                _logger.LogInformation("Actualizando préstamo con ID: {Id}", updatePrestamoDto.IDPrestamo);

                var prestamoExistente = await _PrestamoRepository.GetByIdAsync(updatePrestamoDto.IDPrestamo);

                if (prestamoExistente == null)
                    return new OperationResult { Success = false, Message = "El préstamo que desea actualizar no existe." };

                prestamoExistente.FechaFin = updatePrestamoDto.FechaFin;
                prestamoExistente.FechaDevolucion = updatePrestamoDto.FechaDevolucion;
                prestamoExistente.Estado = (EstadoPrestamo)Enum.Parse(typeof(EstadoPrestamo), updatePrestamoDto.Estado);

                var result = await _PrestamoRepository.UpdateAsync(prestamoExistente);

                if (!result.Success)
                {
                    _logger.LogWarning("Error al actualizar préstamo: {Mensaje}", result.Message);
                    return new OperationResult { Success = false, Message = result.Message };
                }

                _logger.LogInformation("Préstamo con ID: {Id} actualizado correctamente.", updatePrestamoDto.IDPrestamo);

                return new OperationResult
                {
                    Success = true,
                    Data = new
                    {
                        Id = prestamoExistente.Id,
                        ISBN = prestamoExistente.ISBN,
                        EjemplarId = prestamoExistente.EjemplarId,
                        UsuarioId = prestamoExistente.UsuarioId,
                        FechaInicio = prestamoExistente.FechaInicio,
                        FechaFin = prestamoExistente.FechaFin,
                        FechaDevolucion = prestamoExistente.FechaDevolucion,
                        Estado = prestamoExistente.Estado.ToString(),
                        EstaActivo = prestamoExistente.EstaActivo
                    },
                    Message = "Préstamo actualizado correctamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al actualizar préstamo.");
                return new OperationResult { Success = false, Message = $"Error inesperado al actualizar el préstamo: {ex.Message}" };
            }
        }
    }
}
