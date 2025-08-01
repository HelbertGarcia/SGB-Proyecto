using SGB.Api.Dtos.ConfiguracionDto;

namespace SGB.Application.Dtos.DashboardDto
{
    public class DashboardDto
    {
        public int TotalConfiguraciones { get; set; }
        public int ConfiguracionesActivas { get; set; }
        public int ConfiguracionesInactivas { get; set; }
        public List<ConfiguracionDto> Configuraciones { get; set; } = new();
    }
}
