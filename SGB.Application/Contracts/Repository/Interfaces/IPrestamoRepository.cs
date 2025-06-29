using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPrestamoRepository: IBaseRepository<Prestamo>
    {
        Task<OperationResult> GetFechaVencimientoByPrestamoIdAsync(int prestamoId);

        Task<OperationResult> GetEstadosPrestamosPorUsuarioAsync(int usuarioId);
    }
}
