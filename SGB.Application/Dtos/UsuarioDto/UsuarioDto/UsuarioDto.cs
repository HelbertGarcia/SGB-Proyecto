using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SGB.Application.Dtos.UsuarioDto.UsuarioDto
{
    public record UsuarioDto
    {
        public int IDUsuario { get; set; }
        public int IDRol { get; set; }
        public string NombreRol { get; set; } = string.Empty; 
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
        public string Estado => EstaActivo ? "Activo" : "Inactivo"; 
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public static explicit operator UsuarioDto(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
