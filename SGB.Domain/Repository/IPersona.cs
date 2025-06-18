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
        Task<Persona> GetByCorreoAsync(string correo);
        Task<IEnumerable<Persona>> SearchByNombreApellidoAsync(string searchTerm);
    }
}
