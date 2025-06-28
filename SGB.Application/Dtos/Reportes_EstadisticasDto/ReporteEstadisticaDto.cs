using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Reportes_EstadisticasDto
{
    public record ReporteEstadisticaDto
    {
        public int IDReporte { get; set; }
        public string TipoReporte { get; set; } = string.Empty; 
        public DateTime FechaGeneracion { get; set; }
        public string GeneradoPor { get; set; } = string.Empty; 
        public string FormatoExportado { get; set; } = string.Empty; 
    }
}
