using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Domain.Entities.Configuracion;

namespace SGB.Api.Contracts.Mappers
{
    public interface IConfiguracionMapper
    {
        ConfiguracionDto MapToDto(Configuracion entity);
        Configuracion MapFromDto(AddConfiguracionDto dto);
        void ApplyUpdateDto(Configuracion entity, UpdateConfiguracionDto dto);
    }
}
