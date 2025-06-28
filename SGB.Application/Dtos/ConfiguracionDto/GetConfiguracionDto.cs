using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.AdministracionDto
{
    public record GetConfiguracionDto
    {
        public int IDConfiguracion { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
