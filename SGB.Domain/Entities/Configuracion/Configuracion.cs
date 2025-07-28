using SGB.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace SGB.Domain.Entities.Configuracion
{
    public class Configuracion : BaseEntityFecha, IEstaActivo
    {
        [Key]
        public int IDConfiguracion { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        [Required]
        public string Valor { get; set; }
        [MaxLength(255)]
        public string Descripcion { get;  set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool EstaActivo { get; set; }

        private Configuracion() : base() { }

        public Configuracion(string nombre, string valor, string descripcion)
        {
            Nombre = nombre;
            Valor = valor;
            Descripcion = descripcion;
            FechaCreacion = DateTime.UtcNow;
            Habilitar();
        }
        public void Deshabilitar()
        {
            EstaActivo = false;
        }
        public void Habilitar()
        {
            EstaActivo = true;
        }
    }
}
