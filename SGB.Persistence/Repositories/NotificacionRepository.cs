using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Notificaciones;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        private readonly SGBContext _context;
        private readonly ILogger<NotificacionRepository> _logger;
        private readonly IConfiguration _configuration;

        public NotificacionRepository(SGBContext context,
                                      ILoggerFactory loggerFactory,
                                      IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<NotificacionRepository>();
        }

        #region "Implementación de INotificacionRepository"

        public async Task<OperationResult<Dictionary<string, int>>> ContarPorTipoAsync(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return OperationResult<Dictionary<string, int>>.Failure("ID de usuario inválido.");
            }

            try
            {
                var data = await _context.Notificaciones
                                         .Where(n => n.IDUsuario == idUsuario)
                                         .AsNoTracking()
                                         .GroupBy(n => n.TipoNotificacion)
                                         .ToDictionaryAsync(g => g.Key, g => g.Count());

                return OperationResult<Dictionary<string, int>>.Success(data);
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Notificaciones:GenerateError"] ?? "Ocurrió un error al contar las notificaciones.";
                _logger.LogError(ex, "{ErrorMessage} para el usuario ID: {UsuarioID}", errorMessage, idUsuario);

                return OperationResult<Dictionary<string, int>>.Failure(errorMessage);
            }
        }

        #endregion
    }
}