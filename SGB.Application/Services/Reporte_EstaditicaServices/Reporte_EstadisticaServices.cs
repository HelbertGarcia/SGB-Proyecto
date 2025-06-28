using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Service.IReporte_EstadisticaServices;
using SGB.Domain.Base;

namespace SGB.Application.Services.Reporte_EstaditicaServices
{
    public sealed class Reporte_EstadisticaServices : IReporte_EstadisticaServices
    {
        private readonly IReporte_EstadisticaServices _repository;
        private readonly ILogger<Reporte_EstadisticaServices> _logger;
        private readonly IConfiguration _configuration;

        public Reporte_EstadisticaServices(IReporte_EstadisticaServices repository, 
                                           ILogger<Reporte_EstadisticaServices> logger,
                                           IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GenerarLibrosMasPrestadosAsync()
        {
            var result = new OperationResult();
            try
            {
                result = await _repository.GetLibrosMasPrestadosAsync();
                result.IsSuccess = true;
                result.Success = true;
                result.Message = "Reporte de libros más prestados generado correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de libros más prestados.");
                result.IsSuccess = false;
                result.Success = false;
                result.Message = "Ocurrió un error al generar el reporte.";
            }
            return result;
        }

        public async Task<OperationResult> GenerarHistorialPrestamosPorUsuarioAsync(int idUsuario)
        {
            var result = new OperationResult();
            try
            {
                result = await _repository.GetHistorialPrestamosUsuarioAsync(idUsuario);
                result.IsSuccess = true;
                result.Success = true;
                result.Message = "Historial de préstamos generado correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar historial de préstamos del usuario.");
                result.IsSuccess = false;
                result.Success = false;
                result.Message = "Ocurrió un error al generar el historial.";
            }
            return result;
        }

        public async Task<OperationResult> GenerarUsuariosConPenalizacionesActivasAsync()
        {
            var result = new OperationResult();
            try
            {
                result = await _repository.GetUsuariosConPenalizacionesAsync();
                result.IsSuccess = true;
                result.Success = true;
                result.Message = "Reporte de usuarios con penalizaciones generado correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de penalizaciones.");
                result.IsSuccess = false;
                result.Success = false;
                result.Message = "Ocurrió un error al generar el reporte.";
            }
            return result;
        }

        public async Task<OperationResult> ExportarReporteAsync(int idReporte, string tipoArchivo)
        {
            var result = new OperationResult();
            try
            {
                result = await _repository.ExportarReporteAsync(idReporte, tipoArchivo);
                result.IsSuccess = true;
                result.Success = true;
                result.Message = $"Reporte exportado como {tipoArchivo}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar el reporte.");
                result.IsSuccess = false;
                result.Success = false;
                result.Message = "Ocurrió un error al exportar el reporte.";
            }
            return result;
        }

      
        public Task<OperationResult> GetLibrosMasPrestadosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> GetHistorialPrestamosUsuarioAsync(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> GetUsuariosConPenalizacionesAsync()
        {
            throw new NotImplementedException();
        }
    }

}
