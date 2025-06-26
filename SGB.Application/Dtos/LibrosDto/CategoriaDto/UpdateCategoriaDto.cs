using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.LibrosDto.CategoriaDto
{
    public record UpdateCategoriaDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
    }
}
