using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public record  AddPrestamoDto
    {
        public string ISBN { get; set; }
        public int EjemplarId { get; set; }
        public int UsuarioId { get; set; }
        public int DiasDePrestamo { get; set; }


    }
}
