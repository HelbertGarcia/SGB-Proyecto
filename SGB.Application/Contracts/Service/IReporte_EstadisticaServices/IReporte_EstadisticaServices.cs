using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Application.Dtos.Reportes_EstadisticasDto;
using SGB.Domain.Base;

namespace SGB.Application.Contracts.Service.IReporte_EstadisticaServices
{
    public interface IReporte_EstadisticaServices
    {

        Task<OperationResult<ReporteEstadisticaDto>> GenerarLibrosMasPrestadosAsync();
        Task<OperationResult<ReporteEstadisticaDto>> GenerarHistorialPrestamosPorUsuarioAsync(int idUsuario);
        Task<OperationResult <ReporteEstadisticaDto>> GenerarUsuariosConPenalizacionesActivasAsync();
        Task<OperationResult<ReporteEstadisticaDto>> ExportarReporteAsync(int idReporte, string tipoArchivo);
        Task<OperationResult<ReporteEstadisticaDto>> GetLibrosMasPrestadosAsync();
        Task<OperationResult<ReporteEstadisticaDto>> GetHistorialPrestamosUsuarioAsync(int idUsuario);
        Task<OperationResult<ReporteEstadisticaDto>> GetUsuariosConPenalizacionesAsync();
    }
}
