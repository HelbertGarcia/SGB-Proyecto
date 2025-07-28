using SGB.Application.Base;

namespace SGB.Application.Dtos.ConfiguracionDto
{
    public class ConfiguracionDto : BaseConfiguracion
    {
        public int IDConfiguracion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
