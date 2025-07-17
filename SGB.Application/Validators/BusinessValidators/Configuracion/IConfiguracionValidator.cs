using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;

namespace SGB.Application.Validators.BusinessValidators.Configuracion
{
    public interface IConfiguracionValidator
    {
        Task<OperationResult<bool>> ValidateForAddAsync(AddConfiguracionDto dto);
        Task<OperationResult<bool>> ValidateForDeleteAsync(int id);
        Task<OperationResult<bool>> ValidateForUpdateAsync(UpdateConfiguracionDto dto);
    }
}
