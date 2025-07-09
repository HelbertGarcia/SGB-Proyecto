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
        private readonly String? _ConnectionStrings;

        private readonly IConfiguration _configuration;

        public PenalizacionRepository(SGBContext context,
                                      ILoggerFactory loggerFactory,
                                      IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PenalizacionRepository>();
            _ConnectionStrings = _configuration.GetConnectionString("SGBDatabase")!;
        }


        #region "Métodos Propios de IPenalizacionRepository"

        public async Task<OperationResult> GetPenalizacionesActivasPorUsuarioAsync(int usuarioId)
        {
            var result = new OperationResult();

            if (usuarioId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "El ID del usuario es inválido."
                };
            }

            try
            {
                var penalizaciones = await Entity
                    .AsNoTracking()
                    .Where(p => p.IDUsuario == usuarioId && p.EstaActivo && p.FechaInicio <= DateTime.UtcNow && p.FechaFin >= DateTime.UtcNow)
                    .ToListAsync();

                result.Success = true;
                result.Data = penalizaciones;
                result.Message = "Penalizaciones activas obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorMessages:Penalizaciones:GetActiveByUserError"] ?? "Error al obtener penalizaciones activas del usuario.";
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