using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Domain.Entities
{
    public class Prestamo : BaseEntity
    {
        public int EjemplarId { get; private set; }
        public int UsuarioId { get; private set; }
        public DateTime FechaPrestamo { get; private set; }
        public DateTime FechaVencimiento { get; private set; }
        public DateTime? FechaDevolucion { get; private set; }
        public EstadoPrestamo Estado { get; private set; }

        private Prestamo() : base() { }

        public Prestamo(int ejemplarId, int usuarioId, int diasDePrestamo) : base()
        {
            if (ejemplarId <= 0)
                throw new ArgumentException("El Id del ejemplar es inválido.", nameof(ejemplarId));

            if (usuarioId <= 0)
                throw new ArgumentException("El Id del usuario es inválido.", nameof(usuarioId));

            if (diasDePrestamo <= 0)
                throw new ArgumentException("Los días de préstamo deben ser un número positivo.", nameof(diasDePrestamo));

            EjemplarId = ejemplarId;
            UsuarioId = usuarioId;
            FechaPrestamo = DateTime.UtcNow;
            FechaVencimiento = DateTime.UtcNow.AddDays(diasDePrestamo);
            FechaDevolucion = null;
            Estado = EstadoPrestamo.Activo;
        }

        public void RegistrarDevolucion()
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Atrasado)
                throw new InvalidOperationException("No se puede registrar la devolución de un préstamo que no está activo o atrasado.");

            FechaDevolucion = DateTime.UtcNow;

            Estado = (FechaDevolucion > FechaVencimiento) ? EstadoPrestamo.DevueltoConAtraso : EstadoPrestamo.Devuelto;
        }

        public void ActualizarEstadoSiEstaAtrasado()
        {
            if (Estado == EstadoPrestamo.Activo && DateTime.UtcNow > FechaVencimiento)
            {
                Estado = EstadoPrestamo.Atrasado;
            }
        }
    }

    public enum EstadoPrestamo
    {
        Activo,
        Atrasado,
        Devuelto,
        DevueltoConAtraso
    }
}