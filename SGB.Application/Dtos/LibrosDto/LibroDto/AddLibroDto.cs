using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.LibrosDto.LibroDto
{
    public record AddLibroDto
    {
        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 13 caracteres.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre del autor no puede exceder los 150 caracteres.")]
        public string Autor { get; set; }

        [StringLength(200, ErrorMessage = "El nombre de la editorial no puede exceder los 200 caracteres.")]
        public string Editorial { get; set; }

        // Alineado con tu BD, que permite nulos.
        public DateTime? FechaPublicacion { get; set; }

        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la categoría no es válido.")]
        public int IDCategoria { get; set; }
    }
}
