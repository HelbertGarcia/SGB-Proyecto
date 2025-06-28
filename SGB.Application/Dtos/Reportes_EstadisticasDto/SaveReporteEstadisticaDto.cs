using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Reportes_EstadisticasDto
{
    public record SaveReporteEstadisticaDto
    {
        [Required(ErrorMessage = "El tipo de reporte es obligatorio.")]
        public string TipoReporte { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario que genera el reporte es obligatorio.")]
        public string GeneradoPor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe indicar el formato exportado.")]
        [RegularExpression("^(PDF|Excel)$", ErrorMessage = "Solo se permiten los formatos PDF o Excel.")]
        public string FormatoExportado { get; set; } = string.Empty;
    }
}
