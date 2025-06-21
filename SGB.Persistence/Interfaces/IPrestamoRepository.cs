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

        // Aquí puedes agregar métodos específicos para el repositorio de Prestamo si es necesario.
        // Por ejemplo, si necesitas un método para obtener préstamos por un criterio específico.
        // Task<IEnumerable<Prestamo>> GetPrestamosByCriteriaAsync(Expression<Func<Prestamo, bool>> criteria);

       
         Task<OperationResult> GetFechaVencimientoByPrestamoIdAsync(int prestamoId);

        Task<OperationResult> GetEstadosPrestamosPorUsuarioAsync(int usuarioId);




    }
}
