using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Penalizaciones
{
    public class Penalizacion: BaseEntity
    {
        protected Penalizacion(int id, int idUsuario, string motivo, DateTime fechaInicio, DateTime fechaFin, bool estaActiva) : base(id)
        {
            IdUsuario = idUsuario;
            Motivo = motivo;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            EstaActiva = estaActiva;
        }

        public Penalizacion(int idUsuario, string motivo, DateTime fechaInicio, DateTime fechaFin) : base(0)
        {
            ValidarYAsignarIdUsuario(idUsuario);
            ValidarYAsignarMotivo(motivo);
            ValidarFechas(fechaInicio, fechaFin);

            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            EstaActiva = true;
        }

        public int IdUsuario { get; private set; }
        public string Motivo { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }
        public bool EstaActiva { get; private set; }

        public void DesactivarPenalizacion()
        {
            if (!EstaActiva)
            {
                throw new InvalidOperationException("La penalización ya está inactiva.");
            }
            EstaActiva = false;
        }

        public void ExtenderPenalizacion(DateTime nuevaFechaFin)
        {
            if (nuevaFechaFin <= FechaFin)
            {
                throw new ArgumentException("La nueva fecha de fin debe ser posterior a la fecha de fin actual.", nameof(nuevaFechaFin));
            }
            if (nuevaFechaFin <= FechaInicio)
            {
                throw new ArgumentException("La nueva fecha de fin no puede ser anterior o igual a la fecha de inicio.", nameof(nuevaFechaFin));
            }
            FechaFin = nuevaFechaFin;
        }

        public void CambiarMotivo(string nuevoMotivo)
        {
            ValidarYAsignarMotivo(nuevoMotivo);
        }

        private void ValidarYAsignarIdUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El ID de usuario no es válido.", nameof(idUsuario));
            }
            IdUsuario = idUsuario;
        }

        private void ValidarYAsignarMotivo(string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo de la penalización no puede estar vacío.", nameof(motivo));
            }
            if (motivo.Length > 250)
            {
                throw new ArgumentException("El motivo de la penalización no puede exceder los 250 caracteres.", nameof(motivo));
            }
            Motivo = motivo;
        }

        private void ValidarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
            {
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.", nameof(fechaInicio));
            }
        }
    }
}
