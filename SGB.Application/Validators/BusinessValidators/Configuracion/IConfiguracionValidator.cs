using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;

namespace SGB.Application.Validators.BusinessValidators.Configuracion
{
    public interface IConfiguracionValidator
    {
        Task<OperationResult> ValidateForAddAsync(AddConfiguracionDto dto);
        Task<OperationResult> ValidateForDeleteAsync(int id);
        Task<OperationResult> ValidateForUpdateAsync(UpdateConfiguracionDto dto);
    }
}
