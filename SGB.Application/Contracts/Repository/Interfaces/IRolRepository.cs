using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;

using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{

    public interface IRolRepository : IBaseRepository<Rol> 
    {
        Task<OperationResult> ObtenerPorNombreAsync(string nombre);

        Task<OperationResult> ObtenerTodosActivosAsync();

        Task<OperationResult> ActivarRolAsync(int idRol);

        Task<OperationResult> DesactivarRolAsync(int idRol);
    }
}
