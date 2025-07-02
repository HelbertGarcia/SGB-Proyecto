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

        public string? Motivo { get; set; }  // Puede ser opcional, o requerido

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        public bool? EstaActivo { get; set; }

    }
}

