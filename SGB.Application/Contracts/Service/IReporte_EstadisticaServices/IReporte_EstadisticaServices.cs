using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Application.Contracts.Service.IReporte_EstadisticaServices
{
    public interface IReporte_EstadisticaServices
    {

        Task<OperationResult> GenerarLibrosMasPrestadosAsync();
        Task<OperationResult> GenerarHistorialPrestamosPorUsuarioAsync(int idUsuario);
        Task<OperationResult> GenerarUsuariosConPenalizacionesActivasAsync();
        Task<OperationResult> ExportarReporteAsync(int idReporte, string tipoArchivo);
        Task<OperationResult> GetLibrosMasPrestadosAsync();
        Task<OperationResult> GetHistorialPrestamosUsuarioAsync(int idUsuario);
        Task<OperationResult> GetUsuariosConPenalizacionesAsync();
    }
}
