using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.AdministracionDto
{
    public record DeleteConfiguracionDto
    {
        [Required(ErrorMessage = "El ID de configuración es obligatorio para eliminar.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor que cero.")]
        public int IDConfiguracion { get; init; }
    }
}
