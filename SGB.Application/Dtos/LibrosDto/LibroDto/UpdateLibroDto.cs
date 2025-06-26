using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.LibrosDto.LibroDto
{
    public record UpdateLibroDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(150)]
        public string Autor { get; set; }

        [StringLength(200)]
        public string Editorial { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        [Range(1, int.MaxValue)]
        public int IDCategoria { get; set; }
    }
}
