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

namespace SGB.Persistence.Repositories
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly ILogger<PenalizacionRepository> _logger;
        private readonly IConfiguration _configuration;
       

        public PenalizacionRepository(
            SGBContext context,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _logger = loggerFactory.CreateLogger<PenalizacionRepository>();
            _configuration = configuration;
           
        }



        public override async Task<OperationResult<Penalizacion>> AddAsync(Penalizacion entity)
        {
            if (entity == null)
            {
                var msg = "La penalización no puede ser nula.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }

            if (entity.IDUsuario <= 0)
            {
                var msg = "El ID de usuario es inválido.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }


            if (string.IsNullOrWhiteSpace(entity.Motivo))
            {
                var msg = "El motivo de la penalización no puede ser nulo o vacío.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }

            if (entity.FechaInicio == default || entity.FechaFin == default)
            {
                var msg = "Las fechas de inicio y fin de la penalización no pueden estar vacías.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }



            _logger.LogInformation("Agregando penalización para usuario {UsuarioId}.", entity.IDUsuario);

            var result = await base.AddAsync(entity);

            if (result.IsSuccess)
                _logger.LogInformation("Penalización agregada con ID {PenalizacionId}.", result.Data.Id);
            else
                _logger.LogError("Error al agregar penalización: {Mensaje}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Penalizacion>> UpdateAsync(Penalizacion entity)
        {
            if (entity == null)
            {
                var msg = "La penalización no puede ser nula.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }

            if (entity.Id <= 0)
            {
                var msg = "El ID de la penalización no es válido.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }

          

            _logger.LogInformation("Actualizando penalización ID {PenalizacionId}.", entity.Id);

            var result = await base.UpdateAsync(entity);

            if (result.IsSuccess)
                _logger.LogInformation("Penalización actualizada ID {PenalizacionId}.", entity.Id);
            else
                _logger.LogError("Error al actualizar penalización ID {PenalizacionId}: {Mensaje}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Penalizacion>> DisableAsync(int id)
        {
            if (id <= 0)
            {
                var msg = "ID inválido para desactivar penalización.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }

            _logger.LogInformation("Desactivando penalización ID {PenalizacionId}.", id);

            var result = await base.DisableAsync(id);

            if (result.IsSuccess)
                _logger.LogInformation("Penalización desactivada ID {PenalizacionId}.", id);
            else
                _logger.LogError("Error al desactivar penalización ID {PenalizacionId}: {Mensaje}", id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Penalizacion>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                var msg = "ID inválido para consultar penalización.";
                _logger.LogWarning(msg);
                return OperationResult<Penalizacion>.Failure(msg);
            }

            _logger.LogInformation("Consultando penalización ID {PenalizacionId}.", id);

            var result = await base.GetByIdAsync(id);

            if (!result.IsSuccess)
                _logger.LogWarning("No se encontró penalización con ID {PenalizacionId}.", id);

            return result;
        }

        public override async Task<OperationResult<IEnumerable<Penalizacion>>> GetAllAsync()
        {
            _logger.LogInformation("Consultando todas las penalizaciones.");

            var result = await base.GetAllAsync();

            if (!result.IsSuccess)
                _logger.LogWarning("Error obteniendo penalizaciones: {Mensaje}", result.Message);

            return result;
        }






        //metodo unico 

        public async Task<OperationResult<List<Penalizacion>>> GetPenalizacionesActivasPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                return OperationResult<List<Penalizacion>>.Failure("El ID del usuario es inválido.");

            try
            {
                var penalizaciones = await Entity
                    .AsNoTracking()
                    .Where(p =>
                        p.IDUsuario == usuarioId &&
                        p.EstaActivo &&
                        p.FechaInicio <= DateTime.UtcNow &&
                        p.FechaFin >= DateTime.UtcNow)
                    .ToListAsync();

                return OperationResult<List<Penalizacion>>.Success(penalizaciones, "Penalizaciones activas obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                var errorMsg = _configuration["ErrorMessages:Penalizaciones:GetActiveByUserError"] ?? "Error al obtener penalizaciones activas del usuario.";
                _logger.LogError(ex, errorMsg);
                return OperationResult<List<Penalizacion>>.Failure(errorMsg);
            }
        }

       
    }
}
