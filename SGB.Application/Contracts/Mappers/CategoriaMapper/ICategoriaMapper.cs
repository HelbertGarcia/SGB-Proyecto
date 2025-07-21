using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Domain.Entities.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Mappers.CategoriaMapper
{
    public interface ICategoriaMapper
    {
        Categoria MapFromDto(AddCategoriaDto dto);
        void ApplyUpdateDto(Categoria entity, UpdateCategoriaDto dto);
        CategoriaDto MapToDto(Categoria entity);
    }
}
