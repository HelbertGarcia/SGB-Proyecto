using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Interfaces
{
    public interface ILibroRepository: IBaseRepository<Libro>
    {
        Task<OperationResult> BuscarPorAutorAsync(string autor);

        Task<OperationResult> BuscarPorTituloAsync(string titulo);

        Task<OperationResult> BuscarPorEditorialAsync(string editorial);

        Task<OperationResult> BuscarPorIsbnAsync(string isbn);

        Task<OperationResult> BuscarPorCategoriaAsync(string nombreCategoria);
    }
}
