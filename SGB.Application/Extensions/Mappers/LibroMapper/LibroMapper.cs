using SGB.Application.Contracts.Mappers.LibroMapper;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Libro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Extensions.Mappers.LibroMapper
{
    public class LibroMapper: ILibroMapper
    {
        public Libro MapFromDto(AddLibroDto dto)
        {
            return new Libro(
                dto.ISBN,
                dto.Titulo,
                dto.Autor,
                dto.Editorial,
                dto.FechaPublicacion,
                dto.IDCategoria
            );
        }

        public void ApplyUpdateDto(Libro entity, UpdateLibroDto dto)
        {
            entity.ActualizarDetalles(
                dto.Titulo,
                dto.Autor,
                dto.Editorial,
                dto.FechaPublicacion,
                dto.IDCategoria
            );
        }

        public LibroDto MapToDto(Libro entity, Categoria categoria)
        {
            return new LibroDto
            {
                Id = entity.Id,
                ISBN = entity.ISBN,
                Titulo = entity.Titulo,
                Autor = entity.Autor,
                Editorial = entity.Editorial,
                FechaPublicacion = entity.FechaPublicacion,
                NombreCategoria = categoria?.Nombre ?? "Desconocida",
                Estado = entity.EstaActivo ? "Disponible" : "Inactivo",
                FechaRegistro = entity.FechaRegistro
            };
        }
    }
}
