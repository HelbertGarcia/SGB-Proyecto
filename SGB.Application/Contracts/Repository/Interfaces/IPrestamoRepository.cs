using SGB.Domain.Entities.Libro;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;

namespace SGB.Api.Contracts.Repository.Interfaces
{
    public interface IPrestamoRepository : IBaseRepository<Prestamo>
    {
        Task<OperationResult<DateTime?>> GetFechaVencimientoByPrestamoIdAsync(int prestamoId);
        Task<OperationResult<IEnumerable<Prestamo>>> GetPrestamosPorUsuarioAsync(int usuarioId);
    }
}