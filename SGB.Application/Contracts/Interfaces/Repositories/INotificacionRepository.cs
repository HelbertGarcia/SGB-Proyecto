using SGB.Domain.Entities.Notificaciones;
using SGB.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Interfaces
{
    public interface INotificacionRepository: IBaseRepository<Notificacion>
    {
    }
}
