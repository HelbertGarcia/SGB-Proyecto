namespace SGB.Presentation.Models
{
    public class PenalizacionModel
    {
        public int idPenalizacion { get; set; }
        public int idPrestamo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int monto { get; set; }
        public bool estaActivo { get; set; }
        public string motivo { get; set; }
        public int usuarioId { get; set; }


    }

    public class Rootobject
    {
        public PenalizacionModel[] Property1 { get; set; }
    }

}




