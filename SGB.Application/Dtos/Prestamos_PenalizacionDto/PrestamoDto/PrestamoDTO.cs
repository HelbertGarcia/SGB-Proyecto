using SGB.Application.Dtos.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public abstract class PrestamoDTO :  DtoBase
    {

        public string? ISBN { get; set; }
        public int EjemplarId { get; set; }
        public int UsuarioId { get; set; }
        public int DiasDePrestamo { get; set; }




    }
}
