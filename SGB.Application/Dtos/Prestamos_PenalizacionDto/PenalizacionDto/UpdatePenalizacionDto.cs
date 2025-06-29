using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public record UpdatePenalizacionDto
    {
        public int IDPenalizacion { get; set; }
        public DateTime FechaVencimiento { get; set; }  // corregido
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }

    }
}
