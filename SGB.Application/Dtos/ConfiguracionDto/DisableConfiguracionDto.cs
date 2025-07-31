using System.ComponentModel.DataAnnotations;

namespace SGB.Api.Dtos.ConfiguracionDto
{
    public class DisableConfiguracionDto
    {
        [Required]
        public int IDConfiguracion { get; set; }
    }
}
