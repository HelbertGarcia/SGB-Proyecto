using SGB.Api.Base;

namespace SGB.Api.Dtos.ConfiguracionDto
{
    public class ConfiguracionDto : BaseConfiguracion
    {
        public int IDConfiguracion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
