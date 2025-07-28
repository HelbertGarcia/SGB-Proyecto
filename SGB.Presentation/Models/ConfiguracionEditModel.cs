using System.ComponentModel.DataAnnotations;

namespace SGB.Presentation.Models
{
    public class ConfiguracionEditModel
    {
        [Required]
        public int IDConfiguracion { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El valor es obligatorio.")]
        [StringLength(200)]
        public string Valor { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
