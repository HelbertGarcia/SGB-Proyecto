using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.ConfiguracionDto
{
    public record AddConfiguracionDto
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string? Descripcion { get; set; }
    }
}
