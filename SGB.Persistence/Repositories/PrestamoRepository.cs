using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using SGB.Application.Contracts.Repository.Interfaces;
using System.Collections.Generic;
using SGB.Application.Loggers;

namespace SGB.Persistence.Repositories
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly IAppLogger<PrestamoRepository> _logger;
        private readonly string? _ConnectionStrings;
        private readonly IConfiguration _configuration;

        public PrestamoRepository(SGBContext context,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration,
                                  IAppLogger<PrestamoRepository> logger)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<OperationResult<Prestamo>> AddAsync(Prestamo entity)
        {
            try
            {
                if (entity == null)
                {
                    var msg = "El préstamo no puede ser nulo.";
                    _logger.Error(msg);
                    return OperationResult<Prestamo>.Failure(msg);
                }

                if (entity.UsuarioId <= 0)
                {
                    var msg = "El ID de usuario es inválido: {0}";
                    _logger.Error(msg, entity.UsuarioId);
                    return OperationResult<Prestamo>.Failure("El ID de usuario es inválido.");
                }

                if (string.IsNullOrWhiteSpace(entity.ISBN))
                {
                    var msg = "El ISBN es obligatorio para el préstamo del usuario {0}";
                    _logger.Error(msg, entity.UsuarioId);
                    return OperationResult<Prestamo>.Failure("El ISBN es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(entity.ISBN) || entity.ISBN.Length != 13 || !entity.ISBN.All(char.IsDigit))
                {
                    var msg = "El ISBN debe contener exactamente 13 dígitos numéricos. ISBN recibido: {0}";
                    _logger.Error(msg, entity.ISBN);
                    throw new ArgumentException("El ISBN debe contener exactamente 13 dígitos numéricos.");
                }

                _logger.Info("Iniciando proceso de agregar préstamo para usuario {0} con ISBN {1}", entity.UsuarioId, entity.ISBN);

                var result = await base.AddAsync(entity);

                if (result.IsSuccess)
                {
                    _logger.Info("Préstamo agregado exitosamente con ID {0} para usuario {1}", result.Data.Id, entity.UsuarioId);
                }
                else
                {
                    _logger.Error("Error al agregar préstamo para usuario {0}: {1}", entity.UsuarioId, result.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al agregar préstamo para usuario {0}", entity?.UsuarioId ?? 0);
                return OperationResult<Prestamo>.Failure("Error interno al procesar el préstamo.");
            }
        }

        public override async Task<OperationResult<Prestamo>> UpdateAsync(Prestamo entity)
        {
            try
            {
                if (entity == null)
                {
                    var msg = "El préstamo no puede ser nulo.";
                    _logger.Error(msg);
                    return OperationResult<Prestamo>.Failure(msg);
                }

                if (entity.Id <= 0)
                {
                    var msg = "El ID del préstamo no es válido: {0}";
                    _logger.Error(msg, entity.Id);
                    return OperationResult<Prestamo>.Failure("El ID del préstamo no es válido.");
                }

                _logger.Info("Iniciando actualización de préstamo ID {0} para usuario {1}", entity.Id, entity.UsuarioId);

                var result = await base.UpdateAsync(entity);

                if (result.IsSuccess)
                {
                    _logger.Info("Préstamo actualizado exitosamente ID {0}", entity.Id);
                }
                else
                {
                    _logger.Error("Error al actualizar préstamo ID {0}: {1}", entity.Id, result.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al actualizar préstamo ID {0}", entity?.Id ?? 0);
                return OperationResult<Prestamo>.Failure("Error interno al actualizar el préstamo.");
            }
        }

        public override async Task<OperationResult<Prestamo>> DisableAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var msg = "ID inválido para desactivar préstamo: {0}";
                    _logger.Error(msg, id);
                    return OperationResult<Prestamo>.Failure("ID inválido para desactivar préstamo.");
                }

                var exists = await Entity.AnyAsync(p => p.Id == id);
                if (!exists)
                {
                    var msg = "No se encontró préstamo con ID {0} para desactivar";
                    _logger.Error(msg, id);
                    return OperationResult<Prestamo>.Failure($"No se encontró préstamo con ID {id} para desactivar.");
                }

                _logger.Info("Iniciando desactivación de préstamo ID {0}", id);

                var result = await base.DisableAsync(id);

                if (result.IsSuccess)
                {
                    _logger.Info("Préstamo desactivado exitosamente ID {0}", id);
                }
                else
                {
                    _logger.Error("Error al desactivar préstamo ID {0}: {1}", id, result.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al desactivar préstamo ID {0}", id);
                return OperationResult<Prestamo>.Failure("Error interno al desactivar el préstamo.");
            }
        }

        public override async Task<OperationResult<Prestamo>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var msg = "ID inválido para consultar préstamo: {0}";
                    _logger.Error(msg, id);
                    return OperationResult<Prestamo>.Failure("ID inválido para consultar préstamo.");
                }

                _logger.Info("Consultando préstamo ID {0}", id);

                var result = await base.GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    _logger.Info("No se encontró préstamo con ID {0}", id);
                }
                else
                {
                    _logger.Info("Préstamo encontrado exitosamente ID {0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al consultar préstamo ID {0}", id);
                return OperationResult<Prestamo>.Failure("Error interno al consultar el préstamo.");
            }
        }

        public override async Task<OperationResult<IEnumerable<Prestamo>>> GetAllAsync()
        {
            try
            {
                _logger.Info("Iniciando consulta de todos los préstamos");

                var result = await base.GetAllAsync();

                if (result.IsSuccess)
                {
                    var count = result.Data?.Count() ?? 0;
                    _logger.Info("Consulta completada exitosamente. Total préstamos encontrados: {0}", count);
                }
                else
                {
                    _logger.Error("Error al obtener todos los préstamos: {0}", result.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al obtener todos los préstamos");
                return OperationResult<IEnumerable<Prestamo>>.Failure("Error interno al consultar los préstamos.");
            }
        }

        #region "Implementación de IPrestamoRepository"

        public async Task<OperationResult<DateTime>> GetFechaVencimientoByPrestamoIdAsync(int prestamoId)
        {
            try
            {
                if (prestamoId <= 0)
                {
                    var msg = "ID de préstamo inválido: {0}";
                    _logger.Error(msg, prestamoId);
                    return OperationResult<DateTime>.Failure("ID de préstamo inválido.");
                }

                _logger.Info("Consultando fecha de vencimiento para préstamo ID {0}", prestamoId);

                var fechaFin = await Entity
                    .Where(p => p.Id == prestamoId)
                    .Select(p => p.FechaFin)
                    .FirstOrDefaultAsync();

                if (fechaFin != default)
                {
                    _logger.Info("Fecha de vencimiento encontrada para préstamo ID {0}: {1}", prestamoId, fechaFin);
                    return OperationResult<DateTime>.Success(fechaFin);
                }
                else
                {
                    string msg = _configuration["ErrorMessages:Prestamos:LoanNotFound"] ?? "Préstamo no encontrado.";
                    _logger.Error("Préstamo no encontrado ID {0}", prestamoId);
                    return OperationResult<DateTime>.Failure(msg);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = _configuration["ErrorMessages:Prestamos:GetFechaFinError"] ?? "Error al obtener la fecha de fin.";
                _logger.Error(ex, "Error al obtener fecha de vencimiento para préstamo ID {0}", prestamoId);
                return OperationResult<DateTime>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<List<(int PrestamoId, string Estado)>>> GetEstadosPrestamosPorUsuarioAsync(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                {
                    var msg = "ID de usuario inválido: {0}";
                    _logger.Error(msg, usuarioId);
                    return OperationResult<List<(int, string)>>.Failure("ID de usuario inválido.");
                }

                _logger.Info("Consultando estados de préstamos para usuario ID {0}", usuarioId);

                var estados = await Entity
                    .AsNoTracking()
                    .Where(p => p.UsuarioId == usuarioId)
                    .Select(p => new ValueTuple<int, string>(p.Id, p.Estado.ToString()))
                    .ToListAsync();

                _logger.Info("Estados obtenidos exitosamente para usuario ID {0}. Total préstamos: {1}", usuarioId, estados.Count);
                return OperationResult<List<(int, string)>>.Success(estados);
            }
            catch (Exception ex)
            {
                string errorMessage = _configuration["ErrorMessages:Prestamos:GetEstadosPorUsuarioError"] ?? "Error al obtener los estados de préstamos del usuario.";
                _logger.Error(ex, "Error al obtener estados de préstamos para usuario ID {0}", usuarioId);
                return OperationResult<List<(int, string)>>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<List<Prestamo>>> GetPrestamosActivosPorUsuarioAsync(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                {
                    var msg = "ID de usuario inválido para consulta de préstamos activos: {0}";
                    _logger.Error(msg, usuarioId);
                    return OperationResult<List<Prestamo>>.Failure("ID de usuario inválido.");
                }

                _logger.Info("Consultando préstamos activos para usuario ID {0}", usuarioId);

                var prestamosActivos = await Entity
                    .AsNoTracking()
                    .Where(p =>
                        p.UsuarioId == usuarioId &&
                        p.EstaActivo &&
                        (p.Estado == EstadoPrestamo.Activo || p.Estado == EstadoPrestamo.Atrasado))
                    .ToListAsync();

                _logger.Info("Préstamos activos obtenidos exitosamente para usuario ID {0}. Total: {1}", usuarioId, prestamosActivos.Count);
                return OperationResult<List<Prestamo>>.Success(prestamosActivos);
            }
            catch (Exception ex)
            {
                string mensaje = _configuration["ErrorMessages:Prestamos:GetPrestamosActivosError"] ?? "Error al obtener los préstamos activos del usuario.";
                _logger.Error(ex, "Error al obtener préstamos activos para usuario ID {0}", usuarioId);
                return OperationResult<List<Prestamo>>.Failure(mensaje);
            }
        }

        #endregion
    }
}