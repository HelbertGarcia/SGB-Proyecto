namespace SGB.Api.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public record GetPrestamoDto
    {
        public int IdPrestamo { get; set; }
        public int IdUsuario { get; set; }
        public string ISBN { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Fechadevolucion { get; set; }
        public string Estado { get; set; }
    }
}

