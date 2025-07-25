using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public class UpdatePenalizacionDto 
    {
        public int IDPenalizacion { get; set; }
      
        public DateTime? FechaFin { get; set; }

        public string? Motivo { get; set; } = string.Empty;

        public decimal? Monto { get; set; }    







    }
}

