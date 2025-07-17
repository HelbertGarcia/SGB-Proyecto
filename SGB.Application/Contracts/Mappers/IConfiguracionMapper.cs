using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Entities.Configuracion;

namespace SGB.Application.Contracts.Mappers
{
    public interface IConfiguracionMapper
    {
        Configuracion MapFromDto(AddConfiguracionDto dto);
        void ApplyUpdateDto(Configuracion entity, UpdateConfiguracionDto dto);
        ConfiguracionDto MapToDto(Configuracion entity);
    }
}
