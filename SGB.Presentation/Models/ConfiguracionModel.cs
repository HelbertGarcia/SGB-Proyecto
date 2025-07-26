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

    public class Root
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
