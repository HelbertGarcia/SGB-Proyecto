using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Prestamos;
using SGB.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Persistence.Interfaces
{
    public interface IPrestamoRepository: IBaseRepository<Prestamo>
    {
        Task<OperationResult> GetFechaVencimientoByPrestamoIdAsync(int prestamoId);

        Task<OperationResult> GetEstadosPrestamosPorUsuarioAsync(int usuarioId);
    }
}
