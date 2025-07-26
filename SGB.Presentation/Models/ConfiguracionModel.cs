namespace SGB.Presentation.Models
{
    public class ConfiguracionModel
    {
        public int IDConfiguracion { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; } 
        public string Descripcion { get; set; } 
        public DateTime FechaCreacion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
