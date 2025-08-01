using System.Data;

namespace SGB.Api.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public record UpdatePrestamoDto
    {
        public DataSetDateTime FechaVencimiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }
    }
}
