using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface IPrestamo: IRepository<Prestamo>
    {
        Task<IEnumerable<Prestamo>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Prestamo>> GetByLibroIdAsync(int libroId);
        Task<IEnumerable<Prestamo>> GetActiveLoansAsync();
        Task<IEnumerable<Prestamo>> GetOverdueLoansAsync(DateTime currentDate);
    }
}
