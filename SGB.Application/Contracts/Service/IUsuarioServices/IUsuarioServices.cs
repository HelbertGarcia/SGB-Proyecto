using SGB.Application.Base;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.IUsuarioServices
{
    public interface IUsuarioServices : IBaseService<SaveUsuarioDto, UpdateUsuarioDto, UsuarioDto>
    {
       
        Task<OperationResult<IEnumerable<UsuarioDto>>> BuscarUsuariosAsync(string terminoBusqueda);
        Task<object?> GetAllUsuario(); // Si es temporal, puedes removerlo luego.
        Task<OperationResult<IEnumerable<UsuarioDto>>> SearchAsync(string termino);
    }
}
