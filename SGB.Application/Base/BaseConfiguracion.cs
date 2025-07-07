using System.ComponentModel.DataAnnotations;

namespace SGB.Application.Base
{
    public abstract record BaseConfiguracion
    {
        [Required]
        public string Valor { get; set; }
        public string? Descripcion { get; set; }
    }
}
