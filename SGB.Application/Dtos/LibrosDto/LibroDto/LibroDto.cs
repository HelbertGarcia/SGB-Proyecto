namespace SGB.Api.Dtos.LibrosDto.LibroDto
{
    public record LibroDto
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string NombreCategoria { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
