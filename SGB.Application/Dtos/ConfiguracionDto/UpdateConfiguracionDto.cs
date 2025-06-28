using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.AdministracionDto
{
    public record UpdateConfiguracionDto
    {
        [Required(ErrorMessage = "El ID de configuración es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor que cero.")]
        public int IDConfiguracion { get; init; }

        [Required(ErrorMessage = "El valor es obligatorio.")]
        [StringLength(1000, ErrorMessage = "El valor no puede superar los 1000 caracteres.")]
        public string Valor { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
        public string? Descripcion { get; set; }

        public bool? EstaActivo { get; set; }
    }
}
