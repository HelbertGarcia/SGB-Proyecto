using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface IPersona
    {
        Task<Persona> GetByIdAsync(int id);

        Task<IEnumerable<Persona>> GetAllAsync();

        Task AddAsync(Persona persona);

        Task UpdateAsync(Persona persona);

        Task DeleteAsync(int id);

        Task<Persona> GetByCorreoAsync(string correo);

        Task<IEnumerable<Persona>> SearchByNombreApellidoAsync(string searchTerm);
    }
}
