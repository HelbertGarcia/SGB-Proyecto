using SGB.Application.Base;

namespace SGB.Application.Dtos.AdministracionDto
{
    public class UpdateConfiguracionDto : BaseConfiguracion
    {
        public int IDConfiguracion { get; set; }
        public bool? EstaActivo { get; set; }
    }
}
