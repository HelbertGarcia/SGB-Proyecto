using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly ILogger<PenalizacionRepository> _logger;
        private readonly IConfiguration _configuration;

        public PenalizacionRepository(SGBContext context,
                                      ILoggerFactory loggerFactory,
                                      IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PenalizacionRepository>();
        }

        #region "Implementation of IPenalizacionRepository"

        public async Task<OperationResult<IEnumerable<Penalizacion>>> GetActivePenalizacionesAsync()
        {
            try
            {
                var now = DateTime.UtcNow;
                var penalizacionesActivas = await Entity
                    .AsNoTracking()
                    .Where(p => p.EstaActivo && p.FechaInicio <= now && p.FechaFin >= now)
                    .ToListAsync();

                return OperationResult<IEnumerable<Penalizacion>>.Success(penalizacionesActivas);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Penalizaciones:GetActiveError"] ?? "Error al obtener penalizaciones activas.";
                _logger.LogError(ex, errorMessage);
                return OperationResult<IEnumerable<Penalizacion>>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<IEnumerable<Penalizacion>>> GetPenalizacionesPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return OperationResult<IEnumerable<Penalizacion>>.Failure("El ID de usuario es inválido.");
            }
            return await base.FindByConditionAsync(p => p.IDUsuario == usuarioId);
        }

        #endregion
    }
}