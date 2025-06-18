using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Rol
{
    public class Rol: BaseEntity
    {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }

        private Rol() : base() { }

        public Rol(string nombre, string descripcion = "") : base()
        {
            ValidarYAsignarNombre(nombre);
            Descripcion = descripcion;
        }

        public void ActualizarDescripcion(string nuevaDescripcion)
        {
            Descripcion = nuevaDescripcion;
        }

        public void CambiarNombre(string nuevoNombre)
        {
            ValidarYAsignarNombre(nuevoNombre);
        }

        private void ValidarYAsignarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del rol es obligatorio.", nameof(nombre));
            }
            Nombre = nombre;
        }
    }
}
