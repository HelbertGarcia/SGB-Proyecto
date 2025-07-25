namespace SGB.Presentation.Models.PenalizacionModels
{
    public class PenalizacionCreateModel
    {
        public string Motivo { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int IDPrestamo { get; set; }
        public decimal Monto { get; set; }
    }
}
