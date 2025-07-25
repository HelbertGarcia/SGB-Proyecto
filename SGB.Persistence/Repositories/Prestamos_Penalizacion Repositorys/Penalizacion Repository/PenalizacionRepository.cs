using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
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
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly IAppLogger<PenalizacionRepository> _logger;
        private readonly IConfiguration _configuration;

        public PenalizacionRepository(
            SGBContext context,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IAppLogger<PenalizacionRepository> logger)
            : base(context, loggerFactory, configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public override async Task<OperationResult<Penalizacion>> AddAsync(Penalizacion entity)
        {
            try
            {
                var (isValid, message) = ValidationHelpers.ValidateEntityNotNull(entity, nameof(Penalizacion));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }

                (isValid, message) = ValidationHelpers.ValidateId(entity.IDUsuario, nameof(entity.IDUsuario));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }

                (isValid, message) = ValidationHelpers.ValidateStringNotNullOrWhitespace(entity.Motivo, nameof(entity.Motivo));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }


                (isValid, message) = ValidationHelpers.ValidateDateRange(entity.FechaInicio, entity.FechaFin);
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }

                _logger.Info("Iniciando proceso de agregar penalización para usuario {0}. Motivo: {1}, FechaInicio: {2}, FechaFin: {3}",
                    entity.IDUsuario, entity.Motivo, entity.FechaInicio, entity.FechaFin);

                var result = await base.AddAsync(entity);


                if (result.IsSuccess)
                    _logger.Info("Penalización agregada exitosamente con ID {0} para usuario {1}", result.Data.Id, entity.IDUsuario);
                else
                    _logger.Error("Error al agregar penalización para usuario {0}: {1}", entity.IDUsuario, result.Message);

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al agregar penalización para usuario {0}", entity?.IDUsuario ?? 0);
                return OperationResult<Penalizacion>.Failure("Error interno al procesar la penalización.");
            }
        }



        public override async Task<OperationResult<Penalizacion>> UpdateAsync(Penalizacion entity)
        {
            try
            {
                var (isValid, message) = ValidationHelpers.ValidateEntityNotNull(entity, nameof(Penalizacion));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }

                (isValid, message) = ValidationHelpers.ValidateId(entity.Id, nameof(entity.Id));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }

                (isValid, message) = ValidationHelpers.ValidateDateRange(entity.FechaInicio, entity.FechaFin);
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }

                _logger.Info("Iniciando actualización de penalización ID {0} para usuario {1}", entity.Id, entity.IDUsuario);

                var result = await base.UpdateAsync(entity);


                if (result.IsSuccess)
                    _logger.Info("Penalización actualizada exitosamente ID {0}", entity.Id);

                else
                    _logger.Error("Error al actualizar penalización ID {0}: {1}", entity.Id, result.Message);

                return result;

            }

            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al actualizar penalización ID {0}", entity?.Id ?? 0);
                return OperationResult<Penalizacion>.Failure("Error interno al actualizar la penalización.");
            }
        }



        public override async Task<OperationResult<Penalizacion>> DisableAsync(int id)
        {
            try
            {
                var (isValid, message) = ValidationHelpers.ValidateId(id, nameof(id));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }



                var exists = await Entity.AnyAsync(p => p.Id == id);
                if (!exists)
                {
                    var msg = $"No se encontró penalización con ID {id} para desactivar";
                    _logger.Error(msg);
                    return OperationResult<Penalizacion>.Failure(msg);
                }

                _logger.Info("Iniciando desactivación de penalización ID {0}", id);

                var result = await base.DisableAsync(id);


                if (result.IsSuccess)
                    _logger.Info("Penalización desactivada exitosamente ID {0}", id);
                else
                    _logger.Error("Error al desactivar penalización ID {0}: {1}", id, result.Message);

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al desactivar penalización ID {0}", id);
                return OperationResult<Penalizacion>.Failure("Error interno al desactivar la penalización.");
            }
        }



        public override async Task<OperationResult<Penalizacion>> GetByIdAsync(int id)
        {
            try
            {
                var (isValid, message) = ValidationHelpers.ValidateId(id, nameof(id));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<Penalizacion>.Failure(message);
                }

                _logger.Info("Consultando penalización ID {0}", id);

                var result = await base.GetByIdAsync(id);

                if (!result.IsSuccess)
                    _logger.Info("No se encontró penalización con ID {0}", id);
                else
                    _logger.Info("Penalización encontrada exitosamente ID {0}", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al consultar penalización ID {0}", id);
                return OperationResult<Penalizacion>.Failure("Error interno al consultar la penalización.");
            }
        }





        public override async Task<OperationResult<IEnumerable<Penalizacion>>> GetAllAsync()
        {
            try
            {
                _logger.Info("Iniciando consulta de todas las penalizaciones");

                var result = await base.GetAllAsync();

                if (result.IsSuccess)
                {
                    var count = result.Data?.Count() ?? 0;
                    _logger.Info("Consulta completada exitosamente. Total penalizaciones encontradas: {0}", count);
                }
                else
                {
                    _logger.Error("Error al obtener todas las penalizaciones: {0}", result.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Excepción no controlada al obtener todas las penalizaciones");
                return OperationResult<IEnumerable<Penalizacion>>.Failure("Error interno al consultar las penalizaciones.");
            }


        }

        public async Task<OperationResult<List<Penalizacion>>> GetPenalizacionesActivasPorUsuarioAsync(int usuarioId)
        {
            try
            {
                var (isValid, message) = ValidationHelpers.ValidateId(usuarioId, nameof(usuarioId));
                if (!isValid)
                {
                    _logger.Error(message);
                    return OperationResult<List<Penalizacion>>.Failure(message);
                }

                _logger.Info("Consultando penalizaciones activas para usuario ID {0}", usuarioId);

                var penalizaciones = await Entity
    .AsNoTracking()
    .Where(p => p.IDUsuario == usuarioId && p.EstaActivo)
    .ToListAsync();


                _logger.Info("Penalizaciones activas obtenidas exitosamente para usuario ID {0}. Total: {1}", usuarioId, penalizaciones.Count);

                return OperationResult<List<Penalizacion>>.Success(penalizaciones, "Penalizaciones activas obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                var errorMsg = _configuration["ErrorMessages:Penalizaciones:GetActiveByUserError"] ?? "Error al obtener penalizaciones activas del usuario.";
                _logger.Error(ex, "Error al obtener penalizaciones activas para usuario ID {0}", usuarioId);
                return OperationResult<List<Penalizacion>>.Failure(errorMsg);
            }
        }
    }
}
