using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPersonaRepository : IBaseRepository<Domain.Base.Usuario>
    {
        Task<OperationResult<Domain.Base.Usuario>> ObtenerPorEmailAsync(string email);

        Task<OperationResult<bool>> ExisteEmailAsync(string email);

        Task<OperationResult<IEnumerable<Domain.Base.Usuario>>> BuscarPorRolAsync(int idRol);

        Task<OperationResult<IEnumerable<Domain.Base.Usuario>>> ObtenerTodosActivosAsync();

        Task<OperationResult<bool>> ActivarCuentaAsync(int idUsuario);

        Task<OperationResult<bool>> DesactivarCuentaAsync(int idUsuario);
        Task <OperationResult<Domain.Base.Usuario>> AddAsync(UsuarioDto usuarioEntity);
        Task <OperationResult<bool>>UpdateAsync(UsuarioDto usuario);
        Task<OperationResult<IEnumerable<UsuarioDto>>> SearchAsync(string termino);
        Task<OperationResult<IEnumerable<UsuarioDto>>> ObtenerTodosConDetallesAsync();
        Task<Domain.Base.Usuario> ObtenerParaActualizacionAsync(int id);
        Task BuscarPorCorreoAsync(object correo);
    }
}