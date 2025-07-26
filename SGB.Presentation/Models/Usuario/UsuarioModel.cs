using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SGB.Presentation.Models.Usuario
{
    public class UsuarioModel
    {
        public int IDUsuario { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Clave { get; set; }

        public int IDRol { get; set; }

        [ValidateNever]
        public string NombreRol { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool EstaActivo { get; set; }

        [ValidateNever]
        public string Estado => EstaActivo ? "Activo" : "Inactivo";
    }
}
