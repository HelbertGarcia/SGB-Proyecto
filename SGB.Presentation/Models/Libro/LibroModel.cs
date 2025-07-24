namespace SGB.Presentation.Models.Libro
{
    public class LibroModel
    {
        public int id { get; set; }
        public string isbn { get; set; }
        public string titulo { get; set; }
        public string autor { get; set; }
        public string editorial { get; set; }
        public DateTime? fechaPublicacion { get; set; }
        public string nombreCategoria { get; set; }
        public string estado { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}

