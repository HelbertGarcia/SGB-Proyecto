namespace SGB.Presentation.Models.PenalizacionModels
{
    public class PenalizacionEditModel
    {
        public int IDPenalizacion { get; set; }
        public string? Motivo { get; set; } = string.Empty;

        public DateTime? FechaFin { get; set; }

        public decimal? Monto { get; set; }
    }
}
