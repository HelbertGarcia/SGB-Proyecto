using SGB.Domain.Entities.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface IRol: IRepository<Rol>
    {
        Task<Rol> GetByNameAsync(string name);
    }
}
