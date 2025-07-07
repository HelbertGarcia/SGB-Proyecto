using System.ComponentModel.DataAnnotations;
using SGB.Application.Base;

namespace SGB.Application.Dtos.ConfiguracionDto
{
    public record AddConfiguracionDto : BaseConfiguracion
    {
        [Required]
        public string Nombre { get; set; }
    }
}
