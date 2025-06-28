using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.UsuarioDto.BibliotecarioDto
{
    public record UpdateBibliotecarioDto
    {
        [Required(ErrorMessage = "El ID del bibliotecario es obligatorio.")]
        public int IDBibliotecario { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue)]
        public int IDUsuario { get; set; }

        public bool EstaActivo { get; set; }
    }
}
