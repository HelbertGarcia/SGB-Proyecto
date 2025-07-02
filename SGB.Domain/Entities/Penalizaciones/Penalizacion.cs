using SGB.Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGB.Domain.Entities.Penalizaciones
{
    [Table("Penalizaciones")] // Nombre exacto de la tabla
    public class Penalizacion : BaseEntityFecha, IEstaActivo
    {
        [Key]
        [Column("IDPenalizacion")]   // 👈 Coincide con tu tabla
        public int Id { get; set; }

        [Column("IDUsuario")]
        public int IDUsuario { get; set; }

        [Column("Motivo")]
        public string Motivo { get; set; } = string.Empty;

        [Column("FechaInicio")]
        public DateTime FechaInicio { get; set; }

        [Column("FechaFin")]
        public DateTime FechaFin { get; set; }

        [Column("EstaActiva")]
        public bool EstaActivo { get; set; } = true;

        [Column("FechaCreacion")]
        public new DateTime FechaCreacion { get; set; }  // Usa `new` para sobrescribir BaseEntityFecha si aplica

        [Column("FechaActualizacion")]
        public new DateTime FechaActualizacion { get; set; }

        // Si tienes estas columnas en la tabla, inclúyelas:
        [Column("FechaDevolucion")]
        public DateTime? FechaDevolucion { get; set; }

    

        // 👇 Constructor sin parámetros para EF Core
        private Penalizacion() { }

        public Penalizacion(int idUsuario, string motivo, DateTime fechaInicio, DateTime fechaFin)
        {
            ValidarYAsignarIdUsuario(idUsuario);
            ValidarYAsignarMotivo(motivo);
            ValidarFechas(fechaInicio, fechaFin);

            FechaInicio = fechaInicio;
            FechaFin = fechaFin;

            Habilitar(); // Marca como activa al crear
            ActualizarFechaModificacion(); // Actualiza FechaActualizacion
        }

        public void DesactivarPenalizacion()
        {
            if (!EstaActivo)
                throw new InvalidOperationException("La penalización ya está inactiva.");

            Deshabilitar();
            ActualizarFechaModificacion();
        }

        public void ExtenderPenalizacion(DateTime nuevaFechaFin)
        {
            ValidarFechas(FechaInicio, nuevaFechaFin);
            if (nuevaFechaFin <= FechaFin)
                throw new ArgumentException("La nueva fecha de fin debe ser posterior a la fecha de fin actual.", nameof(nuevaFechaFin));

            FechaFin = nuevaFechaFin;
            ActualizarFechaModificacion();
        }

        public void CambiarMotivo(string nuevoMotivo)
        {
            ValidarYAsignarMotivo(nuevoMotivo);
            ActualizarFechaModificacion();
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
            Motivo = motivo.Trim();
        }

        private void ValidarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio >= fechaFin)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.", nameof(fechaInicio));
        }

        private void ActualizarFechaModificacion() => FechaActualizacion = DateTime.UtcNow;

        public void Deshabilitar() => EstaActivo = false;

        public void Habilitar() => EstaActivo = true;
    }
}
