using SGB.Application.Contracts.Mappers;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Domain.Entities.Configuracion;


namespace SGB.Application.Extensions.Mappers.ConfiguracionMapper
{
    public class ConfiguracionMapper : IConfiguracionMapper
    {
        public Configuracion MapFromDto(AddConfiguracionDto dto)
        {
            return new Configuracion(dto.Nombre, dto.Valor, dto.Descripcion);
        }

        public void ApplyUpdateDto(Configuracion entity, UpdateConfiguracionDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                entity.Nombre = dto.Nombre;

            if (!string.IsNullOrWhiteSpace(dto.Valor))
                entity.Valor = dto.Valor;

            if (!string.IsNullOrWhiteSpace(dto.Descripcion))
                entity.Descripcion = dto.Descripcion;

            if (dto.EstaActivo.HasValue)
                entity.EstaActivo = dto.EstaActivo.Value;
        }

        public ConfiguracionDto MapToDto(Configuracion entity)
        {
            return new ConfiguracionDto
            {
                IDConfiguracion = entity.IDConfiguracion,
                Nombre = entity.Nombre,
                Valor = entity.Valor,
                Descripcion = entity.Descripcion,
                FechaCreacion = entity.FechaCreacion,
                EstaActivo = entity.EstaActivo
            };
        }
    }
}
