using SGB.Domain.Base;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Api.Validators.BusinessValidators.Configuracion
{
    public interface IConfiguracionValidator
    {
        Task<OperationResult<bool>> ValidateForDeleteAsync(int id);
        Task<OperationResult<bool>> ValidarAsync(AddConfiguracionDto dto);      
        Task<OperationResult<bool>> ValidateForUpdateAsync(UpdateConfiguracionDto dto);
    }
}
