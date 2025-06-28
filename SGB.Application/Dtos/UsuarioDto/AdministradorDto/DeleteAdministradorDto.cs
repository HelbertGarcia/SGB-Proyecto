using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.UsuarioDto.AdministradorDto
{
    public record DeleteAdministradorDto
    {
        [Required(ErrorMessage = "El ID del administrador es obligatorio.")]
        public int IDAdmin { get; set; }
    }
}
