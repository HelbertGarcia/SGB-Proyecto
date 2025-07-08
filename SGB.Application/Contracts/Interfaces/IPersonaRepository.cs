
using SGB.Domain.Base;
using SGB.Domain.Repository;
using System.Threading.Tasks;

namespace SGB.Persistence.Interfaces
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