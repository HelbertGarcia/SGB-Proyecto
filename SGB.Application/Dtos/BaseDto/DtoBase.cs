using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SGB.Application.Dtos.BaseDto
{
    public abstract class DtoBase
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }

}

