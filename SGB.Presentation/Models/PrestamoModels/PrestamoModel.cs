namespace SGB.Presentation.Models
{
    public class PrestamoModel
    {
        public int id { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime? fechaDevolucion { get; set; }
        public string estado { get; set; }
        public bool estaActivo { get; set; }
        public int usuarioId { get; set; }
        public string isbn { get; set; }
    }





}



