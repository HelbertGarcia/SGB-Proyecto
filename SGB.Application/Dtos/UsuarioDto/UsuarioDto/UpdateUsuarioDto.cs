using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Dtos.UsuarioDto.UsuarioDto
{
    public record UpdateUsuarioDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int IDUsuario { get; set; }

        [Required(ErrorMessage = "El ID del rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de rol inválido.")]
        public int IDRol { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public bool EstaActivo { get; set; }
    }
}
