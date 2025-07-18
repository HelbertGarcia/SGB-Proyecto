using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public class RegistrarDevolucionDto
    {
        public int IdPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; } 
    }
}
