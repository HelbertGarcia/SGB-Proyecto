using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.LibrosDto.CategoriaDto
{
    public record AddCategoriaDto
    {
        public string Nombre { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
        public DateTime FechaCreacion { get; init; } = DateTime.UtcNow;
    }
}
