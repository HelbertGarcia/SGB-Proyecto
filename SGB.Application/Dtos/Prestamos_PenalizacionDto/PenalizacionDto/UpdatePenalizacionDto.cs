using System.Data;

namespace SGB.Api.Dtos.Prestamos_PenalizacionDto.PenalizacionDto
{
    public record UpdatePenalizacionDto
    {
        public DataSetDateTime FechaVencmiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }
    }
}
