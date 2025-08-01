using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;

namespace SGB.Api.Contracts.Repository.Interfaces
{
    public interface IRolRepository : IBaseRepository<Rol>
    {
        Task<OperationResult<Rol>> ObtenerPorNombreAsync(string nombre);
        Task<OperationResult<IEnumerable<Rol>>> ObtenerTodosActivosAsync();
        Task<OperationResult<bool>> ActivarRolAsync(int idRol);
        Task<OperationResult<bool>> DesactivarRolAsync(int idRol);
    }
}