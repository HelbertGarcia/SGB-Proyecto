using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Penalizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface IPenalizacion: IRepository<Penalizacion>
    {
        Task<IEnumerable<Penalizacion>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Penalizacion>> GetActiveByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Penalizacion>> GetActiveOnDateAsync(DateTime date);
    }
}
