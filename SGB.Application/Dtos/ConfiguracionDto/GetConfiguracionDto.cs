using SGB.Application.Base;

namespace SGB.Application.Dtos.AdministracionDto
{
    public record GetConfiguracionDto : BaseConfiguracion
    {
        public int IDConfiguracion { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
