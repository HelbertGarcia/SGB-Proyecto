using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Reportes_EstadisticasDto
{

    public record DeleteReporteEstadisticaDto
    {
        [Required(ErrorMessage = "El ID del reporte a eliminar es obligatorio.")]
        public int IDReporte { get; set; }
    }
}
