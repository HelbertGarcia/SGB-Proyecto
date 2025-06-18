using SGB.Domain.Entities.Notificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Domain.Repository
{
    public interface INotificacion: IRepository<Notificacion>
    {
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Notificacion>> GetNotificacionesPendientesAsync();
    }
}
