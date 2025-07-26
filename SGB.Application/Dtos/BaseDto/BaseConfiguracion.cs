using System.ComponentModel.DataAnnotations;

namespace SGB.Application.Base
{
    public abstract class BaseConfiguracion
    {
        public int IDConfiguracion { get; set; }
        public string Nombre { get; set; }
        [Required]
        public string Valor { get; set; }
        public string? Descripcion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
