using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public record  AddPenalizacionDto
    {
        public string ISBN { get; set; }
        public int UsuarioId { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }




    }
}
