using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Libro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Mappers.LibroMapper
{
    public interface ILibroMapper
    {
        Libro MapFromDto(AddLibroDto dto);
        void ApplyUpdateDto(Libro entity, UpdateLibroDto dto);

        LibroDto MapToDto(Libro entity, Categoria categoria);
    }
}
