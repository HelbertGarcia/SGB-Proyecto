using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<PenalizacionRepository> _logger;
        private readonly IConfiguration _configuration;

        public PenalizacionRepository(SGBContext context,
                                      ILoggerFactory loggerFactory,
                                      IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PenalizacionRepository>();
        }

        #region "Métodos Propios de IPenalizacionRepository"

        public async Task<OperationResult> GetActivePenalizacionesAsync()
        {
            var result = new OperationResult();
            try
            {
                var now = DateTime.Now;
                var penalizacionesActivas = await Entity
                    .AsNoTracking()
                    .Where(p => p.EstaActivo && p.FechaInicio <= now && p.FechaFin >= now)
                    .ToListAsync();

                result.Data = penalizacionesActivas;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorMessages:Penalizaciones:GetActiveError"] ?? "Error al obtener penalizaciones activas.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetMotivosPenalizacionesPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El ID de usuario es inválido." });
            }

            var result = new OperationResult();
            try
            {
                var motivos = await Entity
                    .AsNoTracking()
                    .Where(p => p.IDUsuario == usuarioId)
                    .Select(p => new { p.Id, p.Motivo }) 
                    .ToListAsync();

                result.Data = motivos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorMessages:Penalizaciones:GetMotivosError"] ?? "Error al obtener los motivos de penalización.";
                _logger.LogError(ex, "{ErrorMessage} - UsuarioId: {UsuarioId}", result.Message, usuarioId);
            }
            return result;
        }

        #endregion
    }
}