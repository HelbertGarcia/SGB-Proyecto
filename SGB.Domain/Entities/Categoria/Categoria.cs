using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Categoria
{
    public class Categoria : BaseEntityFecha, IEstaActivo
    {
        public int id { get; set; }
        public string Nombre { get; private set; }

        public bool EstaActivo { get; set; }

        private Categoria() : base() { }

        public Categoria(string nombre) : base()
        {
            ValidarYAsignarNombre(nombre);
            Habilitar();
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
