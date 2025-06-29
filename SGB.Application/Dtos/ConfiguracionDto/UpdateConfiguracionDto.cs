using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.AdministracionDto
{
    public record UpdateConfiguracionDto
    {
        public int IDConfiguracion { get; init; }
        public string Valor { get; set; }
        public string? Descripcion { get; set; }
        public bool? EstaActivo { get; set; }
    }
}
