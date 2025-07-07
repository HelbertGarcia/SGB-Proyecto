using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;
using SGB.Domain.Entities.Notificaciones;
using SGB.Domain.Repository;

namespace SGB.Persistence.Interfaces
{
    public interface INotificacionRepository: IBaseRepository<Notificacion>
    {
        Task<OperationResult> ContarPorTipoAsync(int idUsuario);
    }
} 
