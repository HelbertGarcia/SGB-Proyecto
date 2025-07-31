namespace SGB.Presentation.Models.PrestamoModels
{
    public class PrestamoCreateModel
    {
        public int UsuarioId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }


    
}
