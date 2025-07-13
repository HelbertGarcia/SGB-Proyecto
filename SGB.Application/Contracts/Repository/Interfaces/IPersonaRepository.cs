using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPersonaRepository : IBaseRepository<Persona> 
    {
        Task<OperationResult<Persona>> ObtenerPorEmailAsync(string email);

        Task<OperationResult<bool>> ExisteEmailAsync(string email);

        Task<OperationResult<IEnumerable<Persona>>> BuscarPorRolAsync(int idRol);

        Task<OperationResult<IEnumerable<Persona>>> ObtenerTodosActivosAsync();

        Task<OperationResult<bool>> ActivarCuentaAsync(int idUsuario);

        Task<OperationResult<bool>> DesactivarCuentaAsync(int idUsuario);
    }
}