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
          
           
        }




        public override async Task<OperationResult<Prestamo>> AddAsync(Prestamo entity)
        {
            if (entity == null)
            {
                var msg = "El préstamo no puede ser nulo.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

            if (entity.UsuarioId <= 0)
            {
                var msg = "El ID de usuario es inválido.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

            if (string.IsNullOrWhiteSpace(entity.ISBN))
            {
                var msg = "El ISBN es obligatorio.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

            if (string.IsNullOrWhiteSpace(entity.ISBN) || entity.ISBN.Length != 13 || !entity.ISBN.All(char.IsDigit))
                throw new ArgumentException("El ISBN debe contener exactamente 13 dígitos numéricos.");

         


            _logger.LogInformation("Agregando préstamo para usuario {UsuarioId}.", entity.UsuarioId);

            var result = await base.AddAsync(entity);

            if (result.IsSuccess)
                _logger.LogInformation("Préstamo agregado con ID {PrestamoId}.", result.Data.Id);
            else
                _logger.LogError("Error al agregar préstamo: {Mensaje}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Prestamo>> UpdateAsync(Prestamo entity)
        {
            if (entity == null)
            {
                var msg = "El préstamo no puede ser nulo.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

          

            if (entity.Id <= 0)
            {
                var msg = "El ID del préstamo no es válido.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

          

            _logger.LogInformation("Actualizando préstamo ID {PrestamoId}.", entity.Id);

            var result = await base.UpdateAsync(entity);

            if (result.IsSuccess)
                _logger.LogInformation("Préstamo actualizado ID {PrestamoId}.", entity.Id);
            else
                _logger.LogError("Error al actualizar préstamo ID {PrestamoId}: {Mensaje}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Prestamo>> DisableAsync(int id)
        {
            if (id <= 0)
            {
                var msg = "ID inválido para desactivar préstamo.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

            var exists = await Entity.AnyAsync(p => p.Id == id);
            if (!exists)
            {
                var msg = $"No se encontró préstamo con ID {id} para desactivar.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

            _logger.LogInformation("Desactivando préstamo ID {PrestamoId}.", id);

            var result = await base.DisableAsync(id);

            if (result.IsSuccess)
                _logger.LogInformation("Préstamo desactivado ID {PrestamoId}.", id);
            else
                _logger.LogError("Error al desactivar préstamo ID {PrestamoId}: {Mensaje}", id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Prestamo>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                var msg = "ID inválido para consultar préstamo.";
                _logger.LogWarning(msg);
                return OperationResult<Prestamo>.Failure(msg);
            }

            _logger.LogInformation("Consultando préstamo ID {PrestamoId}.", id);

            var result = await base.GetByIdAsync(id);

            if (!result.IsSuccess)
                _logger.LogWarning("No se encontró préstamo con ID {PrestamoId}.", id);

            return result;
        }

        public override async Task<OperationResult<IEnumerable<Prestamo>>> GetAllAsync()
        {

            _logger.LogInformation("Consultando todos los préstamos.");

            var result = await base.GetAllAsync();

            if (!result.IsSuccess)
                _logger.LogWarning("Error obteniendo préstamos: {Mensaje}", result.Message);

            return result;
        }

        //metodos unicos 


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

                return OperationResult<List<Prestamo>>.Success(prestamosActivos); // lista vacía si no tiene préstamos, lo cual es válido
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
