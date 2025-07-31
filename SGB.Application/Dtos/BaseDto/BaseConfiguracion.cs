using System.ComponentModel.DataAnnotations;

namespace SGB.Api.Base
{
    public abstract class BaseConfiguracion
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
      
        [Required(ErrorMessage = "El campo Valor es obligatorio.")]
        public string Valor { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
        public string? Descripcion { get; set; }

        public bool EstaActivo { get; set; } = true;
    }
}
