using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;
using SGB.Domain.Repository;
using System.Threading.Tasks;

namespace SGB.Persistence.Interfaces
{

    public interface IRolRepository : IBaseRepository<Rol> 
    {
        Task<OperationResult> ObtenerPorNombreAsync(string nombre);

        Task<OperationResult> ObtenerTodosActivosAsync();

        Task<OperationResult> ActivarRolAsync(int idRol);

        Task<OperationResult> DesactivarRolAsync(int idRol);
    }
}
