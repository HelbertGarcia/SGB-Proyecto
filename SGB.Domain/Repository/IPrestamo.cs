using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface IPrestamo
    {
        Task<Prestamo> GetByIdAsync(int id);

        Task<IEnumerable<Prestamo>> GetAllAsync();

        Task AddAsync(Prestamo prestamo);

        Task UpdateAsync(Prestamo prestamo);

        Task DeleteAsync(int id);

        Task<IEnumerable<Prestamo>> GetByUsuarioIdAsync(int usuarioId);

        Task<IEnumerable<Prestamo>> GetByLibroIdAsync(int libroId);

        Task<IEnumerable<Prestamo>> GetActiveLoansAsync();

        Task<IEnumerable<Prestamo>> GetOverdueLoansAsync(DateTime currentDate);
    }
}
