using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public  class PenalizacionResponseDto : PenalizacionDto
    {
        public int IDPenalizacion { get; set; }
        public int IDPrestamo { get; set; }  // ← CLAVE para rastrear
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Monto { get; set; }
        public bool EstaActivo { get; set; }
    }
}
