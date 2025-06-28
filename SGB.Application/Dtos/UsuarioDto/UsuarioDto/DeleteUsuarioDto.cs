using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.UsuarioDto.UsuarioDto
{
    public record DeleteUsuarioDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int IDUsuario { get; set; }
    }
}