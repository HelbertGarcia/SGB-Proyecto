using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.UsuarioDto.BibliotecarioDto
{
    public record DeleteBibliotecarioDto
    {
        [Required(ErrorMessage = "El ID del bibliotecario es obligatorio.")]
        public int IDBibliotecario { get; set; }
    }
}
