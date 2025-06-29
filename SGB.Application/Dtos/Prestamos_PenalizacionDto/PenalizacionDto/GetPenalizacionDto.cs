using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public record  GetPenalizacionDto
    {

        public int IDPenalizacion { get; set; }
        public int IdUsuario { get; set; }

        public string Motivo { get; set; } = string.Empty;
        
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public bool EstaActiva { get; set; }
    }
}
