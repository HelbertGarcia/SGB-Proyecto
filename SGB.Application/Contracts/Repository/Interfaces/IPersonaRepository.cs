using SGB.Domain.Base;

namespace SGB.Api.Contracts.Repository.Interfaces
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