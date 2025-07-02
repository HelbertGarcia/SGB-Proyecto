using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Domain.Entities.Prestamos
{
    [Table("Prestamos")]
    public class Prestamo : BaseEntity, IEstaActivo
    {
        [Key]
        [Column("IdPrestamo")]
        public int Id { get; set; }

        [Required]
        [StringLength(13)]
        [Column("ISBN")]
        public string ISBN { get; set; }

        [Required]
        [Column("EjemplarId")]
        public int EjemplarId { get; set; }

        [Required]
        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("FechaPrestamo")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Column("FechaVencimiento")]
        public DateTime FechaFin { get; set; }

        [Column("FechaDevolucion")]
        public DateTime? FechaDevolucion { get; set; }

        [Required]
        [Column("Estado", TypeName = "nvarchar(50)")]
        public EstadoPrestamo Estado { get; set; } = EstadoPrestamo.Activo;

        public bool EstaActivo { get; set; }

        // Constructor sin parámetros para EF Core
        private Prestamo() { }

        public Prestamo(int ejemplarId, int usuarioId, int diasDePrestamo, string isbn)
        {
            if (ejemplarId <= 0)
                throw new ArgumentException("El Id del ejemplar es inválido.", nameof(ejemplarId));

            if (usuarioId <= 0)
                throw new ArgumentException("El Id del usuario es inválido.", nameof(usuarioId));

            if (diasDePrestamo <= 0)
                throw new ArgumentException("Los días de préstamo deben ser un número positivo.", nameof(diasDePrestamo));

            if (string.IsNullOrWhiteSpace(isbn) || isbn.Length > 13)
                throw new ArgumentException("ISBN inválido.", nameof(isbn));

            EjemplarId = ejemplarId;
            UsuarioId = usuarioId;
            FechaInicio = DateTime.UtcNow;
            FechaFin = FechaInicio.AddDays(diasDePrestamo);
            FechaDevolucion = null;
            Estado = EstadoPrestamo.Activo;
            ISBN = isbn;
            EstaActivo = true;
        }


        public void RegistrarDevolucion()
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Atrasado)
                throw new InvalidOperationException("No se puede registrar la devolución de un préstamo que no está activo o atrasado.");

            FechaDevolucion = DateTime.UtcNow;
            Estado = FechaDevolucion > FechaFin ? EstadoPrestamo.DevueltoConAtraso : EstadoPrestamo.Devuelto;
        }

        public void ActualizarEstadoSiEstaAtrasado()
        {
            if (Estado == EstadoPrestamo.Activo && DateTime.UtcNow > FechaFin)
            {
                Estado = EstadoPrestamo.Atrasado;
            }
        }

        public void Deshabilitar()
        {
            EstaActivo = false;

            // Si quieres que el Estado cambie cuando deshabilitas:
            if (Estado == EstadoPrestamo.Activo || Estado == EstadoPrestamo.Atrasado)
            {
                Estado = EstadoPrestamo.Devuelto; // O el que corresponda en tu negocio
            }
        }

        public void Habilitar()
        {
            EstaActivo = true;
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
