using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Base
{
    public abstract class BaseEntityFecha: BaseEntity
    {
        protected BaseEntityFecha() : base()
        {
            FechaCreacion = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        public DateTime FechaCreacion { get; protected set; }

        public DateTime FechaActualizacion { get; protected set; }

        protected void ActualizarFechaModificacion()
        {
            FechaActualizacion = DateTime.UtcNow;
        }
    }
}
