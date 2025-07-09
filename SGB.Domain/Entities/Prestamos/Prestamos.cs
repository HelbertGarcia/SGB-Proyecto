using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SGB.Domain.Base;

namespace SGB.Domain.Entities.Prestamos
{
    [Table("Prestamos")]
    public class Prestamo : BaseEntityFecha, IEstaActivo
    {
        [Key]
        [Column("IDPrestamo")]
        public int Id { get; set; }

        [Required]
        [StringLength(13)]
        [Column("ISBN")]
        public string ISBN { get; set; }

        [Required]
        [Column("IDUsuario")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("FechaInicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Column("FechaFin")]
        public DateTime FechaFin { get; set; }

        [Column("FechaDevolucion")]
        public DateTime? FechaDevolucion { get; set; }

        [Required]
        [Column("Estado", TypeName = "nvarchar(50)")]
        public EstadoPrestamo Estado { get; set; } = EstadoPrestamo.Activo;

        [Column("EstaActiva")]
        public bool EstaActivo { get; set; } = true;

      

        // Constructor sin parámetros para EF Core
        private Prestamo() { }

        public Prestamo(int usuarioId, string isbn, DateTime fechaInicio, DateTime fechaFin)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("El Id del usuario es inválido.", nameof(usuarioId));

            if (string.IsNullOrWhiteSpace(isbn) || isbn.Length > 13)
                throw new ArgumentException("ISBN inválido.", nameof(isbn));

            if (fechaFin <= fechaInicio)
                throw new ArgumentException("La fecha de fin debe ser mayor que la fecha de inicio.", nameof(fechaFin));

            UsuarioId = usuarioId;
            ISBN = isbn;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Estado = EstadoPrestamo.Activo;
            EstaActivo = true;
            FechaCreacion = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void RegistrarDevolucion()
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Atrasado)
                throw new InvalidOperationException("No se puede registrar la devolución de un préstamo que no está activo o atrasado.");

            FechaDevolucion = DateTime.UtcNow;
            Estado = FechaDevolucion > FechaFin ? EstadoPrestamo.DevueltoConAtraso : EstadoPrestamo.Devuelto;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void ActualizarEstadoSiEstaAtrasado()
        {
            if (Estado == EstadoPrestamo.Activo && DateTime.UtcNow > FechaFin)
            {
                Estado = EstadoPrestamo.Atrasado;
                FechaActualizacion = DateTime.UtcNow;
            }
        }

        public void Deshabilitar()
        {
            EstaActivo = false;
            if (Estado == EstadoPrestamo.Activo || Estado == EstadoPrestamo.Atrasado)
            {
                Estado = EstadoPrestamo.Devuelto;
            }
            FechaActualizacion = DateTime.UtcNow;
        }

        public void Habilitar()
        {
            EstaActivo = true;
            FechaActualizacion = DateTime.UtcNow;
        }
    }

    public enum EstadoPrestamo
    {
        Activo,
        Atrasado,
        Devuelto,
        DevueltoConAtraso,
        Pendiente
    }
}
