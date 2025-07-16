using SGB.Application.Dtos.BaseDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public  class UpdatePrestamoDto : DTOBase
    {
        public int IDPrestamo { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaInicio { get; set; }
        
    }
}
