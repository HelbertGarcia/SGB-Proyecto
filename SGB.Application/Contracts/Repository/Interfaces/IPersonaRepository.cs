using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPersonaRepository : IBaseRepository<Persona> 
    {
        Task<OperationResult> ObtenerPorEmailAsync(string email);

        Task<OperationResult> ExisteEmailAsync(string email);

        Task<OperationResult> BuscarPorRolAsync(int idRol);

        Task<OperationResult> ObtenerTodosActivosAsync();

        Task<OperationResult> ActivarCuentaAsync(int idUsuario);

        Task<OperationResult> DesactivarCuentaAsync(int idUsuario);
    }
}