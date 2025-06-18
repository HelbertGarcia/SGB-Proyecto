using SGB.Domain.Entities.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface ICategoria
    {
        Task<Categoria> GetByIdAsync(int id);

        Task<IEnumerable<Categoria>> GetAllAsync();

        Task AddAsync(Categoria categoria);

        Task UpdateAsync(Categoria categoria);

        Task DeleteAsync(int id);

        Task<Categoria> GetByNameAsync(string name);
    }
}
