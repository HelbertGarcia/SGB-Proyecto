using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly ILogger<PrestamoRepository> _logger;
        private readonly IConfiguration _configuration;

        // The constructor is aligned with the final pattern.
        public PrestamoRepository(SGBContext context,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PrestamoRepository>();
        }

        // No overrides are needed here. The BaseRepository handles Add, Update, Delete, etc.

        #region "Implementation of IPrestamoRepository"

        public async Task<OperationResult<DateTime?>> GetFechaVencimientoByPrestamoIdAsync(int prestamoId)
        {
            if (prestamoId <= 0)
            {
                return OperationResult<DateTime?>.Failure("ID de préstamo inválido.");
            }

            try
            {
                var fechaVencimiento = await Entity
                                           .Where(p => p.Id == prestamoId)
                                           .Select(p => (DateTime?)p.FechaVencimiento)
                                           .FirstOrDefaultAsync();

                if (fechaVencimiento != null)
                {
                    return OperationResult<DateTime?>.Success(fechaVencimiento);
                }
                else
                {
                    return OperationResult<DateTime?>.Failure(_configuration["ErrorMessages:Prestamos:LoanNotFound"] ?? "Préstamo no encontrado.");
                }
            }
            catch (Exception ex)
            {
                var errorMessage = _configuration["ErrorMessages:Prestamos:GetFechaVencimientoError"] ?? "Error al obtener la fecha de vencimiento.";
                _logger.LogError(ex, "{ErrorMessage} - ID de Préstamo: {PrestamoId}", errorMessage, prestamoId);
                return OperationResult<DateTime?>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<IEnumerable<Prestamo>>> GetPrestamosPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return OperationResult<IEnumerable<Prestamo>>.Failure("El ID de usuario es inválido.");
            }

            // We can reuse the generic FindByConditionAsync from the base repository.
            // It correctly handles errors and returns the strongly-typed OperationResult.
            return await base.FindByConditionAsync(p => p.UsuarioId == usuarioId);
        }

        #endregion
    }
}