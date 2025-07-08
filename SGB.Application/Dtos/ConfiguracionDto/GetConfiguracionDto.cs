using SGB.Application.Base;

namespace SGB.Application.Dtos.AdministracionDto
{
    public class GetConfiguracionDto : BaseConfiguracion
    {
        public int IDConfiguracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
