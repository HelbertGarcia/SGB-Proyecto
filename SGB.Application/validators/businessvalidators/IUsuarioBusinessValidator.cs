using System.Threading.Tasks;
using SGB.Application.Dtos.UsuarioDto;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;

namespace SGB.Application.Validators.BusinessValidators
{
    public interface IUsuarioBusinessValidator
    {
        Task<OperationResult<Domain.Base.Usuario>> ValidateForAddAsync(UpdateUsuarioDto dto);
        Task<OperationResult<Domain.Base.Usuario>> ValidateForUpdateAsync(int id, UpdateUsuarioDto dto);
        Task<OperationResult<bool>> ValidateForDeleteAsync(int id);
    }
}
