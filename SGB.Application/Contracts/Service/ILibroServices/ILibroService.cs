using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.ILibroServices
{
    public interface ILibroService
    {
        Task<OperationResult> AddLibroAsync(AddLibroDto libroDto);

        Task<OperationResult> UpdateLibroAsync(string isbn, UpdateLibroDto libroDto);

        Task<OperationResult> DeleteLibroAsync(string isbn);

        Task<OperationResult> GetLibroDetailsAsync(string isbn);

        Task<OperationResult> GetLibrosAsync(string terminoBusqueda);

        Task<OperationResult> GetAllLibrosAsync();
    }
}
