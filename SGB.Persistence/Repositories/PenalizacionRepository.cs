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



        // Si decides eliminar este método, simplemente bórralo
        /*
        public async Task<OperationResult> GetMotivosPenalizacionesPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                return OperationResult.Failure("El ID del usuario es inválido.");

            try
            {
                var motivos = await Entity
                    .AsNoTracking()
                    .Where(p => p.IDUsuario == usuarioId)
                    .Select(p => new { p.Id, p.Motivo })
                    .ToListAsync();

                return OperationResult.Success(motivos);
            }
            catch (Exception ex)
            {
                var errorMsg = _configuration["ErrorMessages:Penalizaciones:GetMotivosError"] ?? "Error al obtener los motivos de penalización.";
                _logger.LogError(ex, errorMsg);
                return OperationResult.Failure(errorMsg);
            }
        }
        */

       
    }
}
