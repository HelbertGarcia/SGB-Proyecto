using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.UsuarioDto.AdministradorDto
{
    public record UpdateAdministradorDto
    {
        [Required(ErrorMessage = "El ID del administrador es obligatorio.")]
        public int IDAdmin { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue)]
        public int IDUsuario { get; set; }

        public bool EstaActivo { get; set; }
    }
}
