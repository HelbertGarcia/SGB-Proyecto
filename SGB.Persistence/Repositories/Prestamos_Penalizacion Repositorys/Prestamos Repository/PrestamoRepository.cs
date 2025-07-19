using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SGB.Application.Loggers;
using SGB.Persistence.Repositories.Prestamos_Penalizacion_Repositorys.validator;

namespace SGB.Persistence.Repositories
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly IAppLogger<PrestamoRepository> _logger;
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
                var (isValid, message) = ValidationHelpers.ValidateEntityNotNull(entity, nameof(Prestamo));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Prestamo>.Failure(message);
                }

                (isValid, message) = ValidationHelpers.ValidateId(entity.UsuarioId, nameof(entity.UsuarioId));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Prestamo>.Failure(message);
                }

                (isValid, message) = ValidationHelpers.ValidateIsbn(entity.ISBN);
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Prestamo>.Failure(message);
                }

                _logger.Info("Iniciando proceso de agregar préstamo para usuario {0} con ISBN {1}", entity.UsuarioId, entity.ISBN);

                var result = await base.AddAsync(entity);

                if (result.IsSuccess)
                    _logger.Info("Préstamo agregado exitosamente con ID {0} para usuario {1}", result.Data.Id, entity.UsuarioId);
                else
                    _logger.Error("Error al agregar préstamo para usuario {0}: {1}", entity.UsuarioId, result.Message);

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
                var (isValid, message) = ValidationHelpers.ValidateEntityNotNull(entity, nameof(Prestamo));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Prestamo>.Failure(message);
                }

                (isValid, message) = ValidationHelpers.ValidateId(entity.Id, nameof(entity.Id));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Prestamo>.Failure(message);
                }

                _logger.Info("Iniciando actualización de préstamo ID {0} para usuario {1}", entity.Id, entity.UsuarioId);

                var result = await base.UpdateAsync(entity);

                if (result.IsSuccess)
                    _logger.Info("Préstamo actualizado exitosamente ID {0}", entity.Id);
                else
                    _logger.Error("Error al actualizar préstamo ID {0}: {1}", entity.Id, result.Message);

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
                var (isValid, message) = ValidationHelpers.ValidateId(id, nameof(id));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Prestamo>.Failure(message);
                }

                var exists = await Entity.AnyAsync(p => p.Id == id);
                if (!exists)
                {
                    var msg = $"No se encontró préstamo con ID {id} para desactivar";
                    _logger.Error(msg);
                    return OperationResult<Prestamo>.Failure(msg);
                }

                _logger.Info("Iniciando desactivación de préstamo ID {0}", id);

                var result = await base.DisableAsync(id);

                if (result.IsSuccess)
                    _logger.Info("Préstamo desactivado exitosamente ID {0}", id);
                else
                    _logger.Error("Error al desactivar préstamo ID {0}: {1}", id, result.Message);

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
                var (isValid, message) = ValidationHelpers.ValidateId(id, nameof(id));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Prestamo>.Failure(message);
                }

                _logger.Info("Consultando préstamo ID {0}", id);

                var result = await base.GetByIdAsync(id);

                if (!result.IsSuccess)
                    _logger.Info("No se encontró préstamo con ID {0}", id);
                else
                    _logger.Info("Préstamo encontrado exitosamente ID {0}", id);

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
                var (isValid, message) = ValidationHelpers.ValidateId(prestamoId, nameof(prestamoId));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<DateTime>.Failure(message);
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




       /* public async Task<OperationResult<List<(int PrestamoId, string Estado)>>> GetEstadosPrestamosPorUsuarioAsync(int usuarioId)
        {
            try
            {
                var (isValid, message) = ValidationHelpers.ValidateId(usuarioId, nameof(usuarioId));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<List<(int, string)>>.Failure(message);
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
        }*/



        public async Task<OperationResult<List<Prestamo>>> GetPrestamosActivosPorUsuarioAsync(int usuarioId)
        {
            try
            {
                var (isValid, message) = ValidationHelpers.ValidateId(usuarioId, nameof(usuarioId));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<List<Prestamo>>.Failure(message);
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
