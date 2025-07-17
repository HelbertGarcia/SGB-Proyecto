using SGB.Application.Base;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Base;

namespace SGB.Application.Contracts.Service.IConfiguracionService
{
    public interface IConfiguracionService : IBaseService<AddConfiguracionDto, UpdateConfiguracionDto, ConfiguracionDto>
    {
    }

}

