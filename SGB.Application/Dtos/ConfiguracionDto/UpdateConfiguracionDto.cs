using SGB.Application.Base;

namespace SGB.Application.Dtos.AdministracionDto
{
    public record UpdateConfiguracionDto : BaseConfiguracion
    {
        public int IDConfiguracion { get; set; }
        public bool? EstaActivo { get; set; }
    }
}
