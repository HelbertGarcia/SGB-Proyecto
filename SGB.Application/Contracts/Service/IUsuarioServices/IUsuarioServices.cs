using SGB.Application.Base;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Service.IUsuarioServices
{
    public interface IUsuarioServices : IBaseService<UsuarioDto, UpdateUsuarioDto, SaveUsuarioDto>
    {
        Task<OperationResult<UsuarioDto>> AddAsync(SaveUsuarioDto dto);
        Task<OperationResult<UsuarioDto>> UpdateAsync(int id, UpdateUsuarioDto dto);
        Task<OperationResult<bool>> DeleteAsync(int id);
        Task<OperationResult<UsuarioDto>> GetByIdAsync(int id);
        Task<OperationResult<IEnumerable<UsuarioDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<UsuarioDto>>> BuscarUsuariosAsync(string termino);
    }

}
