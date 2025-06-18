using SGB.Domain.Entities.Libro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface ILibro
    {
        Task<Libro> GetByIdAsync(int id);

        Task<IEnumerable<Libro>> GetAllAsync();

        Task AddAsync(Libro libro);

        Task UpdateAsync(Libro libro);

        Task DeleteAsync(int id);

        Task<IEnumerable<Libro>> FindByTituloAsync(string titulo);

        Task<IEnumerable<Libro>> FindByAutorAsync(string autor);

        Task<Libro> GetByIsbnAsync(string isbn);
    }
}
