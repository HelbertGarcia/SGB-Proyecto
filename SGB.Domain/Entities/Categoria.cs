using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nombre { get; private set; }

        private Categoria() : base() { }

        public Categoria(string nombre) : base()
        {
            ValidarYAsignarNombre(nombre);
        }

        public void ActualizarNombre(string nuevoNombre)
        {
            ValidarYAsignarNombre(nuevoNombre);
        }

        private void ValidarYAsignarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.", nameof(nombre));
            }
            Nombre = nombre;
        }
    }
}
