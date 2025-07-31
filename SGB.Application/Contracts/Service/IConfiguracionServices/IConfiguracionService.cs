using SGB.Domain.Base;
using SGB.Api.Base;
using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Api.Contracts.Service.IConfiguracionService
{
    public interface IConfiguracionService : IBaseService<AddConfiguracionDto, UpdateConfiguracionDto, ConfiguracionDto>
    {
        Task<OperationResult<ConfiguracionDto>> ObtenerPorNombreAsync(string nombre);
    }
}

