using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SGB.Presentation.Models
{
    public class ConfiguracionModel
    {
        public int idConfiguracion { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string valor { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public DateTime fechaCreacion { get; set; }
        public bool estaActivo { get; set; }
    }

    public class Root
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
