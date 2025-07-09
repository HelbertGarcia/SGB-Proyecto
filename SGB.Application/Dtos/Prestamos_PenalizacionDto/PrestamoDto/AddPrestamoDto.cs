using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public class  AddPrestamoDto : PrestamoDto
    {
      public DateTime FechaInicio { get; set; } // Fecha del préstamo
      public DateTime FechaFin { get; set; }     // Fecha límite para devolver


    }
}
