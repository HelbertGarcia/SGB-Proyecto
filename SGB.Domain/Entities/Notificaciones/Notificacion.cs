using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Entities.Notificaciones
{
    public class Notificacion: Base.BaseEntity
    {
        public Notificacion(int id, string mensaje, string tipoDeNotificacion, DateTime fechaCreacion, int idUsuario)
            : base(id)
        {
            Mensaje = mensaje;
            TipoDeNotificacion = tipoDeNotificacion;
            FechaCreacion = fechaCreacion;
            IdUsuario = idUsuario;
        }
        public Notificacion(string mensaje, string tipoDeNotificacion, DateTime fechaCreacion, int idUsuario)
            : base(0)
        {
            Mensaje = mensaje;
            TipoDeNotificacion = tipoDeNotificacion;
            FechaCreacion = fechaCreacion;
            IdUsuario = idUsuario;
        }
        public string Mensaje { get; private set; }
        public string TipoDeNotificacion { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public int IdUsuario { get; private set; }
    }
}
