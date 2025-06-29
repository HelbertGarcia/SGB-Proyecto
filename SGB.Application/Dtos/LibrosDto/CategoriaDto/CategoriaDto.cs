using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.LibrosDto.CategoriaDto
{
    public record CategoriaDto
    (
        int Id,
        string Nombre,
        bool EstaActivo
    );
}
