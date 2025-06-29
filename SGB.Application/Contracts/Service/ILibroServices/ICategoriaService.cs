using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.ILibroServices
{
    public interface ICategoriaService
    {
        Task<OperationResult> AddCategoriaAsync(AddCategoriaDto addCategoriaDto);
        Task<OperationResult> UpdateCategoriaAsync(int id, UpdateCategoriaDto updateCategoriaDto);
        Task<OperationResult> DeleteCategoriaAsync(int id);
        Task<OperationResult> GetCategoriaAsync(int id);
        Task<OperationResult> GetAllCategoriasAsync();
    }
}
