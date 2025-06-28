using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Reportes_EstadisticasDto
{
    public record UpdateReporteEstadisticaDto
    {
        [Required(ErrorMessage = "El ID del reporte es obligatorio.")]
        public int IDReporte { get; set; }

        public string? FormatoExportado { get; set; }
        public DateTime? FechaGeneracion { get; set; } = DateTime.Now;
    }
}
