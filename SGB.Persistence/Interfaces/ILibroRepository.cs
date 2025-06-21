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
        public Task<OperationResult> BuscarPorAutor(string autor);

        public Task<OperationResult> BuscarPorTitulo(string titulo);

        public Task<OperationResult> BuscarPorEditorial(string editorial);

        Task<OperationResult> BuscarPorIsbnAsync(string isbn);

        Task<OperationResult> BuscarPorCategoriaAsync(string nombreCategoria);
    }
}
