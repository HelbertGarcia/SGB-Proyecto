using SGB.Domain.Entities.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface IRol
    {
        Task<Rol> GetByIdAsync(int id);

        Task<IEnumerable<Rol>> GetAllAsync();

        Task AddAsync(Rol rol);

        Task UpdateAsync(Rol rol);

        Task DeleteAsync(int id);

        Task<Rol> GetByNameAsync(string name);
    }
}
