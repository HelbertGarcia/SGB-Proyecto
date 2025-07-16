using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public class UpdatePenalizacionDto : PenalizacionDto
    {
        public int IDPenalizacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        



        
       

    }
}

