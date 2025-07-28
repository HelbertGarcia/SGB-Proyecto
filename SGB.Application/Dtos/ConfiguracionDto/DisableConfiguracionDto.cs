using System.ComponentModel.DataAnnotations;

namespace SGB.Application.Dtos.ConfiguracionDto
{
    public class DisableConfiguracionDto
    {
        [Required]
        public int IDConfiguracion { get; set; }
    }
}
