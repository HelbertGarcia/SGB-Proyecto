using SGB.Application.Contracts.Mappers.CategoriaMapper;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Domain.Entities.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Extensions.Mappers.CategoriaMapper
{
    public class CategoriaMapper: ICategoriaMapper
    {
        public Categoria MapFromDto(AddCategoriaDto dto)
        {
            return new Categoria(dto.Nombre);
        }

        public void ApplyUpdateDto(Categoria entity, UpdateCategoriaDto dto)
        {
            entity.ActualizarNombre(dto.Nombre);
        }

        public CategoriaDto MapToDto(Categoria entity)
        {
            return new CategoriaDto(
                entity.Id,
                entity.Nombre,
                entity.EstaActivo
            );
        }
    }
}
