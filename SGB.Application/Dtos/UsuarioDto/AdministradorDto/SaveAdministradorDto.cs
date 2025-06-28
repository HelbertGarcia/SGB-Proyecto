using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.UsuarioDto.AdministradorDto
{
    public record SaveBibliotecarioDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de usuario inválido.")]
        public int IDUsuario { get; set; }
    }
}
