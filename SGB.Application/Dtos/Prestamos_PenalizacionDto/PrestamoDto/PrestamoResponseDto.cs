using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto
{
    public class PrestamoResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string ISBN { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }
        public bool EstaActivo { get; set; }
    }
}
