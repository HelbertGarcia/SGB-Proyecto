using SGB.Application.Base;
using System.ComponentModel.DataAnnotations;

namespace SGB.Application.Dtos.ConfiguracionDto
{
    public class UpdateConfiguracionDto : BaseConfiguracion
    {
        [Required(ErrorMessage = "El ID de configuración es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor a cero.")]
        public int IDConfiguracion { get; set; }
    }
}
