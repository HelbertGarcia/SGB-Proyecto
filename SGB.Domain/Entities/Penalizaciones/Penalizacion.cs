using SGB.Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGB.Domain.Entities.Penalizaciones
{
    [Table("Penalizaciones")]
    public class Penalizacion : BaseEntityFecha, IEstaActivo
    {
        [Key]
        [Column("IDPenalizacion")]
        public int Id { get; set; }

        [Required]
        [Column("IDUsuario")]
        public int IDUsuario { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("Motivo")]
        public string Motivo { get; private set; } = string.Empty;

        [Required]
        [Column("FechaInicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Column("FechaFin")]
        public DateTime FechaFin { get; private set; }

        [Column("Monto")]
        public decimal Monto { get; set; }

        [Column("IDPrestamo")]
        public int IDPrestamo { get; set; }

        [Column("EstaActiva")]
        public bool EstaActivo { get; set; } = true;

        // Constructor sin parámetros requerido por EF Core
        private Penalizacion() { }

        public Penalizacion(int idUsuario, string motivo, DateTime fechaInicio, DateTime fechaFin, int idPrestamo, decimal monto)
        {
            // Asignar sin validar ni lanzar excepción
            IDUsuario = idUsuario;
            Motivo = motivo ?? string.Empty;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Monto = monto;
            IDPrestamo = idPrestamo;

            Habilitar();
            ActualizarFechaModificacion();
        }

        #region Comportamientos de dominio

        public void DesactivarPenalizacion()
        {
            if (!EstaActivo)
                throw new InvalidOperationException("La penalización ya está inactiva.");

            Deshabilitar();
            ActualizarFechaModificacion();
        }

        public void ExtenderPenalizacion(DateTime nuevaFechaFin)
        {
            if (nuevaFechaFin <= FechaFin)
                throw new ArgumentException("La nueva fecha debe ser posterior a la actual.", nameof(nuevaFechaFin));

            FechaFin = nuevaFechaFin;
            ActualizarFechaModificacion();
        }

        public void CambiarMotivo(string nuevoMotivo)
        {
            Motivo = nuevoMotivo ?? string.Empty;
            ActualizarFechaModificacion();
        }
        public void CambiarMonto(decimal nuevoMonto)
        {
            if (nuevoMonto <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            Monto = nuevoMonto;
        }

        public void Deshabilitar() => EstaActivo = false;

        public void Habilitar() => EstaActivo = true;

        #endregion

        // Quité métodos privados de validación ya que no se usan
    }
}