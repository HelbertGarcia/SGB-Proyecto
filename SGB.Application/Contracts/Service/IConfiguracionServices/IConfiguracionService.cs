using SGB.Api.Base;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Application.Dtos.DashboardDto;
using SGB.Domain.Base;

namespace SGB.Api.Contracts.Service.IConfiguracionService
{
    public interface IConfiguracionService : IBaseService<AddConfiguracionDto, UpdateConfiguracionDto, ConfiguracionDto>
    {
        Task<OperationResult<ConfiguracionDto>> ObtenerPorNombreAsync(string nombre);
        Task<OperationResult<DashboardDto>> ObtenerDatosDashboardAsync();
    }
}

