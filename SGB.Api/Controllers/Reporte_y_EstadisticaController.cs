using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.IReporte_EstadisticaServices;
using System.Threading.Tasks;

namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Reporte_y_EstadisticaController : ControllerBase
    {
        private readonly IReporte_EstadisticaServices _reporteService;

        public Reporte_y_EstadisticaController(IReporte_EstadisticaServices reporteService)
        {
            _reporteService = reporteService;
        }

      
        [HttpGet("libros-mas-prestados")]
        public async Task<IActionResult> GenerarLibrosMasPrestadosAsync()
        {
            var resultado = await _reporteService.GenerarLibrosMasPrestadosAsync();
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

       
        [HttpGet("historial-usuario/{idUsuario}")]
        public async Task<IActionResult> GetHistorialUsuario(int idUsuario)
        {
            var resultado = await _reporteService.GenerarHistorialPrestamosPorUsuarioAsync(idUsuario);
            if (!resultado.IsSuccess || resultado.Data == null) return NotFound(resultado);
            return Ok(resultado.Data);
        }

     
        [HttpGet("usuarios-con-penalizaciones")]
        public async Task<IActionResult> GetUsuariosConPenalizaciones()
        {
            var resultado = await _reporteService.GenerarUsuariosConPenalizacionesActivasAsync();
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

       
        [HttpPost("exportar/{idReporte}")]
        public async Task<IActionResult> Exportar(int idReporte, [FromQuery] string tipoArchivo)
        {
            if (string.IsNullOrWhiteSpace(tipoArchivo))
                return BadRequest("Debe especificar el tipo de archivo a exportar.");

            var resultado = await _reporteService.ExportarReporteAsync(idReporte, tipoArchivo);
            if (!resultado.IsSuccess) return BadRequest(resultado);

            return Ok(new { mensaje = resultado.Message });
        }
    }
}
