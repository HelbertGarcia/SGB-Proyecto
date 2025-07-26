using System.ComponentModel.DataAnnotations;
using SGB.Domain.Base;

namespace SGB.Domain.Entities.Configuracion
{
    public class Configuracion : BaseEntityFecha, IEstaActivo
    {
        [Key]
        public int IDConfiguracion { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get;  set; }
        public DateTime FechaCreacion { get; set; }

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
