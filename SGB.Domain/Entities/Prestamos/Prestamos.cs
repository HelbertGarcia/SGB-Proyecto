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
       
        public Prestamo() { }
        public Prestamo(int usuarioId, string isbn, DateTime fechaInicio, DateTime fechaFin)
        {


            UsuarioId = usuarioId;
            ISBN = isbn;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Estado = EstadoPrestamo.Activo;
            EstaActivo = true;
            FechaCreacion = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        public void RegistrarDevolucion(DateTime fechaDevolucion)
        {
            if (FechaDevolucion.HasValue)
                throw new InvalidOperationException("La devolución ya fue registrada.");

            FechaDevolucion = fechaDevolucion;

            Estado = FechaDevolucion > FechaFin
                ? EstadoPrestamo.DevueltoConAtraso
                : EstadoPrestamo.Devuelto;

            EstaActivo = false; // Marcar como inactivo al devolver

            ActualizarFechaModificacion();
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

            if (!FechaDevolucion.HasValue)
            {
                Estado = EstadoPrestamo.Cancelado;
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
        Cancelado

    }
}
