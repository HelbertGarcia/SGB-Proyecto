using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Application.Dtos.UsuarioDto.BibliotecarioDto;

namespace SGB.Application.Dtos.UsuarioDto.BibliotecarioDto
{
    public record BibliotecarioDto
    {
        public int IDBibliotecario { get; set; }
        public int IDUsuario { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool EstaActivo { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}













//BibliotecarioDto