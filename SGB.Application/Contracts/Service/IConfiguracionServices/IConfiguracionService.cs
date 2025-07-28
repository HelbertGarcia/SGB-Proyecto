using SGB.Domain.Base;
using SGB.Application.Base;
using SGB.Application.Dtos.ConfiguracionDto;

namespace SGB.Application.Contracts.Service.IConfiguracionService
{
    public interface IConfiguracionService : IBaseService<AddConfiguracionDto, UpdateConfiguracionDto, ConfiguracionDto>
    {
        Task<OperationResult<ConfiguracionDto>> ObtenerPorNombreAsync(string nombre);
    }

}

