using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly ILogger<PrestamoRepository> _logger;
        private readonly IConfiguration _configuration;

        public PrestamoRepository(SGBContext context,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PrestamoRepository>();
        }

        #region "Implementación de IPrestamoRepository"

        public async Task<OperationResult> GetFechaVencimientoByPrestamoIdAsync(int prestamoId)
        {
            if (prestamoId <= 0)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "ID de préstamo inválido." });
            }

            try
            {
                var fechaVencimiento = await Entity
                                           .Where(p => p.Id == prestamoId)
                                           .Select(p => p.FechaVencimiento)
                                           .FirstOrDefaultAsync();

                if (fechaVencimiento != default(DateTime))
                {
                    return new OperationResult { Data = fechaVencimiento };
                }
                else
                {
                    return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Prestamos:LoanNotFound"] ?? "Préstamo no encontrado." };
                }
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Prestamos:GetFechaVencimientoError"] ?? "Error al obtener la fecha de vencimiento.";
                _logger.LogError(ex, "{ErrorMessage} - ID de Préstamo: {PrestamoId}", errorMessage, prestamoId);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }

        public async Task<OperationResult> GetEstadosPrestamosPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "ID de usuario inválido." });
            }

            try
            {
                var estados = await Entity
                    .AsNoTracking()
                    .Where(p => p.Id == usuarioId) 
                    .Select(p => new
                    {
                        PrestamoId = p.Id,
                        Estado = p.Estado.ToString()
                    })
                    .ToListAsync();

                return new OperationResult { Data = estados };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Prestamos:GetEstadosPorUsuarioError"] ?? "Error al obtener los estados de préstamos del usuario.";
                _logger.LogError(ex, "{ErrorMessage} - UsuarioId: {UsuarioId}", errorMessage, usuarioId);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }
        #endregion
    }
}