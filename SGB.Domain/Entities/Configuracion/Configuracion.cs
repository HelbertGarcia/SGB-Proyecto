using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Domain.Entities.Configuracion
{
    public class Configuracion : IEstaActivo
    {
        public int IDConfiguracion { get; set; }
        public string Nombre { get;  set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public bool EstaActivo { get; set; }


        private Configuracion() { }
        public Configuracion(string nombre, string valor, string descripcion = null)
        {
            ValidarYAsignarNombre(nombre);
            ValidarYAsignarValor(valor);
            Descripcion = descripcion;
            FechaCreacion = DateTime.UtcNow;
            Habilitar();
        }

        public void ActualizarValor(string nuevoValor)
        {
            ValidarYAsignarValor(nuevoValor);
        }

        public void ActualizarDescripcion(string nuevaDescripcion)
        {
            if (!string.IsNullOrWhiteSpace(nuevaDescripcion) && nuevaDescripcion.Length > 255)
                throw new ArgumentException("La descripción no puede exceder los 255 caracteres.", nameof(nuevaDescripcion));

            Descripcion = nuevaDescripcion;
        }

        private void ValidarYAsignarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de configuración no puede estar vacío.", nameof(nombre));
            if (nombre.Length > 100)
                throw new ArgumentException("El nombre no debe exceder los 100 caracteres.", nameof(nombre));

            Nombre = nombre;
        }

        private void ValidarYAsignarValor(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("El valor de configuración no puede estar vacío.", nameof(valor));

            Valor = valor;
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
