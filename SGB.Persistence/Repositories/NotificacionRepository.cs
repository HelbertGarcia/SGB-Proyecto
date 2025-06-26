using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Notificaciones;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using SGB.Persistence.Interfaces;
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

        // 1. CONSTRUCTOR CORREGIDO: Inyecta ILoggerFactory y pasa las dependencias a la clase base.
        public NotificacionRepository(SGBContext context,
                                      ILoggerFactory loggerFactory,
                                      IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<NotificacionRepository>();
        }

        public async Task<OperationResult> ContarPorTipoAsync(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return await Task.FromResult(new OperationResult
                {
                    Success = false,
                    Message = _configuration["ErrorMessages:Global:ValidationError"] ?? "ID de usuario inválido."
                });
            }

            try
            {
                var data = await _context.Notificacion 
                                         .Where(n => n.IDUsuario == idUsuario)
                                         .AsNoTracking()
                                         .GroupBy(n => n.TipoNotificacion)
                                         .ToDictionaryAsync(g => g.Key, g => g.Count());

                return new OperationResult { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Notificaciones:GenerateError"] ?? "Ocurrió un error al contar las notificaciones.";
                _logger.LogError(ex, "{ErrorMessage} para el usuario ID: {UsuarioID}", errorMessage, idUsuario);
                return new OperationResult { Success = false, Message = errorMessage };
            }
        }
    }
}