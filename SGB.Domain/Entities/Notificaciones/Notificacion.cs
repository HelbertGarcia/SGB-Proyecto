using SGB.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Notificaciones
{
    public class Notificacion: BaseEntity
    {
        public int IDNotificacion { get;  set; }
        public int IDUsuario { get;  set; }
        public string Mensaje { get;  set; }
        public string TipoNotificacion { get;  set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaEnvio { get; set; }

        private Notificacion() { }

        public Notificacion(int idUsuario, string mensaje, string tipoNotificacion)
        {
            ValidarYAsignarUsuario(idUsuario);
            ValidarYAsignarMensaje(mensaje);
            ValidarYAsignarTipoNotificacion(tipoNotificacion);

            FechaCreacion = DateTime.UtcNow;
            FechaEnvio = null; 
        }

        public void MarcarComoEnviada()
        {
            if (!FechaEnvio.HasValue)
            {
                FechaEnvio = DateTime.UtcNow;
            }
        }

        private void ValidarYAsignarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("El ID de usuario es inválido.", nameof(idUsuario));
            IDUsuario = idUsuario;
        }

        private void ValidarYAsignarMensaje(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                throw new ArgumentException("El mensaje no puede estar vacío.", nameof(mensaje));
            Mensaje = mensaje;
        }

        private void ValidarYAsignarTipoNotificacion(string tipoNotificacion)
        {
            if (string.IsNullOrWhiteSpace(tipoNotificacion))
                throw new ArgumentException("El tipo de notificación no puede estar vacío.", nameof(tipoNotificacion));
            TipoNotificacion = tipoNotificacion;
        }
    }
}
