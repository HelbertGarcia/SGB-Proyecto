using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Penalizaciones
{
    public class Penalizacion: BaseEntityFecha, IEstaActivo
    {
        public int IDUsuario { get; private set; }
        public string Motivo { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }
        public bool EstaActivo { get; set; }

        private Penalizacion() { }

        public Penalizacion(int idUsuario, string motivo, DateTime fechaInicio, DateTime fechaFin)
        {
            ValidarYAsignarIdUsuario(idUsuario);
            ValidarYAsignarMotivo(motivo);
            ValidarFechas(fechaInicio, fechaFin);

            Habilitar();
            FechaActualizacion = DateTime.UtcNow;
        }

        public void DesactivarPenalizacion()
        {
            if (!EstaActivo)
            {
                throw new InvalidOperationException("La penalización ya está inactiva.");
            }
            Deshabilitar();
            ActualizarFechaModificacion();
        }

        public void ExtenderPenalizacion(DateTime nuevaFechaFin)
        {
            ValidarFechas(this.FechaInicio, nuevaFechaFin);
            if (nuevaFechaFin <= FechaFin)
            {
                throw new ArgumentException("La nueva fecha de fin debe ser posterior a la fecha de fin actual.", nameof(nuevaFechaFin));
            }
            FechaFin = nuevaFechaFin;
            ActualizarFechaModificacion();
        }

        public void CambiarMotivo(string nuevoMotivo)
        {
            ValidarYAsignarMotivo(nuevoMotivo);
            ActualizarFechaModificacion();
        }

        private void ActualizarFechaModificacion()
        {
            FechaActualizacion = DateTime.UtcNow;
        }

        private void ValidarYAsignarIdUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("El ID de usuario no es válido.", nameof(idUsuario));
            IDUsuario = idUsuario;
        }

        private void ValidarYAsignarMotivo(string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo) || motivo.Length > 200)
                throw new ArgumentException("El motivo de la penalización es inválido.", nameof(motivo));
            Motivo = motivo;
        }

        private void ValidarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio >= fechaFin)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.", nameof(fechaInicio));
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
