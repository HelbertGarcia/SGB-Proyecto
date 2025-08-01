namespace SGB.Api.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public record  AddPrestamoDto
    {
        public string ISBN { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
