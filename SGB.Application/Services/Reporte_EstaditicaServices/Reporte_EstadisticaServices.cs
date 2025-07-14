using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Service.IReporte_EstadisticaServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Dtos.Reportes_EstadisticasDto;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGB.Application.Services.Reporte_EstaditicaServices
{
    public sealed class Reporte_EstadisticaServices : IReporte_EstadisticaServices
    {
        private readonly IReporte_EstadisticaServices _repository;
        private readonly ILogger<Reporte_EstadisticaServices> _logger;
        private readonly IConfiguration _configuration;

        public Reporte_EstadisticaServices(
            IReporte_EstadisticaServices repository,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<Reporte_EstadisticaServices>();
            _configuration = configuration;
        }

        public async Task<OperationResult<IEnumerable<UsuarioDto>>> GenerarLibrosMasPrestadosAsync()
        {
            try
            {
                var result = await _repository.GetLibrosMasPrestadosAsync();
                if (!result.IsSuccess)
                    return OperationResult<IEnumerable<UsuarioDto>>.Failure(result.Message);

                return OperationResult<IEnumerable<UsuarioDto>>.Success(result.Data!, "Reporte de libros más prestados generado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de libros más prestados.");
                return OperationResult<IEnumerable<UsuarioDto>>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<ReporteEstadisticaDto>> GenerarHistorialPrestamosPorUsuarioAsync(int idUsuario)
        {
            try
            {
                var result = await _repository.GetHistorialPrestamosUsuarioAsync(idUsuario);
                if (!result.IsSuccess)
                    return OperationResult<ReporteEstadisticaDto>.Failure(result.Message);

                return OperationResult<ReporteEstadisticaDto>.Success(result.Data!, "Historial de préstamos generado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar historial de préstamos del usuario con ID: {ID}", idUsuario);
                return OperationResult<ReporteEstadisticaDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<IEnumerable<UsuarioDto>>> GenerarUsuariosConPenalizacionesActivasAsync()
        {
            try
            {
                var result = await _repository.GetUsuariosConPenalizacionesAsync();
                if (!result.IsSuccess)
                    return OperationResult<IEnumerable<UsuarioDto>>.Failure(result.Message);

                return OperationResult<IEnumerable<UsuarioDto>>.Success(result.Data!, "Usuarios con penalizaciones activas generados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de penalizaciones activas.");
                return OperationResult<IEnumerable<UsuarioDto>>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> ExportarReporteAsync(int idReporte, string tipoArchivo)
        {
            try
            {
                var result = await _repository.ExportarReporteAsync(idReporte, tipoArchivo);
                if (!result.IsSuccess)
                    return OperationResult<bool>.Failure(result.Message);

                return OperationResult<bool>.Success(true, $"Reporte exportado como {tipoArchivo}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar el reporte con ID: {ID}", idReporte);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        // Métodos no implementados
        public Task<OperationResult<IEnumerable<UsuarioDto>>> GetLibrosMasPrestadosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ReporteEstadisticaDto>> GetHistorialPrestamosUsuarioAsync(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<UsuarioDto>>> GetUsuariosConPenalizacionesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> ExportarReporteAsync(int idReporte, string tipoArchivo, bool dummy = true)
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<ReporteEstadisticaDto>> IReporte_EstadisticaServices.GenerarLibrosMasPrestadosAsync()
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<ReporteEstadisticaDto>> IReporte_EstadisticaServices.GenerarUsuariosConPenalizacionesActivasAsync()
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<ReporteEstadisticaDto>> IReporte_EstadisticaServices.ExportarReporteAsync(int idReporte, string tipoArchivo)
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<ReporteEstadisticaDto>> IReporte_EstadisticaServices.GetLibrosMasPrestadosAsync()
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<ReporteEstadisticaDto>> IReporte_EstadisticaServices.GetUsuariosConPenalizacionesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
