using SGB.Domain.Entities.Libro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface ILibro: IRepository<Libro>
    {
        Task<IEnumerable<Libro>> FindByTituloAsync(string titulo);
        Task<IEnumerable<Libro>> FindByAutorAsync(string autor);
        Task<Libro> GetByIsbnAsync(string isbn);
    }
}
