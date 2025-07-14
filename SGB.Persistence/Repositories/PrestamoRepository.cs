using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using SGB.Persistence.Base;
using SGB.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using SGB.Application.Contracts.Repository.Interfaces;
using System.Collections.Generic;

namespace SGB.Persistence.Repositories
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly ILogger<PrestamoRepository> _logger;
        private readonly string? _ConnectionStrings;
        private readonly IConfiguration _configuration;

        public PrestamoRepository(SGBContext context,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration)
            : base(context, loggerFactory, configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PrestamoRepository>();
            _ConnectionStrings = _configuration.GetConnectionString("SGBDatabase");
        }

        #region "Implementación de IPrestamoRepository"

        public async Task<OperationResult<DateTime>> GetFechaVencimientoByPrestamoIdAsync(int prestamoId)
        {
            if (prestamoId <= 0)
            {
                return OperationResult<DateTime>.Failure("ID de préstamo inválido.");
            }

            try
            {
                var fechaFin = await Entity
                    .Where(p => p.Id == prestamoId)
                    .Select(p => p.FechaFin)
                    .FirstOrDefaultAsync();

                if (fechaFin != default)
                {
                    return OperationResult<DateTime>.Success(fechaFin);
                }
                else
                {
                    string msg = _configuration["ErrorMessages:Prestamos:LoanNotFound"] ?? "Préstamo no encontrado.";
                    return OperationResult<DateTime>.Failure(msg);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = _configuration["ErrorMessages:Prestamos:GetFechaFinError"] ?? "Error al obtener la fecha de fin.";
                _logger.LogError(ex, "{ErrorMessage} - ID de Préstamo: {PrestamoId}", errorMessage, prestamoId);
                return OperationResult<DateTime>.Failure(errorMessage);
            }
        }

        public async Task<OperationResult<List<(int PrestamoId, string Estado)>>> GetEstadosPrestamosPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return OperationResult<List<(int, string)>>.Failure("ID de usuario inválido.");
            }

            try
            {
                var estados = await Entity
                    .AsNoTracking()
                    .Where(p => p.UsuarioId == usuarioId)
                    .Select(p => new ValueTuple<int, string>(p.Id, p.Estado.ToString()))
                    .ToListAsync();

                return OperationResult<List<(int, string)>>.Success(estados);
            }
            catch (Exception ex)
            {
                string errorMessage = _configuration["ErrorMessages:Prestamos:GetEstadosPorUsuarioError"] ?? "Error al obtener los estados de préstamos del usuario.";
                _logger.LogError(ex, "{ErrorMessage} - UsuarioId: {UsuarioId}", errorMessage, usuarioId);
                return OperationResult<List<(int, string)>>.Failure(errorMessage);
            }
        }

        #endregion

        public async Task<OperationResult<List<Prestamo>>> GetPrestamosActivosPorUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                _logger.LogWarning("ID de usuario inválido para la consulta de préstamos activos.");
                return OperationResult<List<Prestamo>>.Failure("ID de usuario inválido.");
            }

            try
            {
                var prestamosActivos = await Entity
                    .AsNoTracking()
                    .Where(p =>
                        p.UsuarioId == usuarioId &&
                        p.EstaActivo &&
                        (p.Estado == EstadoPrestamo.Activo || p.Estado == EstadoPrestamo.Atrasado))
                    .ToListAsync();

                return OperationResult<List<Prestamo>>.Success(prestamosActivos);
            }
            catch (Exception ex)
            {
                string mensaje = _configuration["ErrorMessages:Prestamos:GetPrestamosActivosError"] ?? "Error al obtener los préstamos activos del usuario.";
                _logger.LogError(ex, "{Mensaje} - UsuarioId: {UsuarioId}", mensaje, usuarioId);
                return OperationResult<List<Prestamo>>.Failure(mensaje);
            }
        }

       
    }
}
