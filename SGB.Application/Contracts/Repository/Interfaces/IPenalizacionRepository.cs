using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using SGB.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
    
        Task<OperationResult> GetActivePenalizacionesAsync();
        Task<OperationResult> GetMotivosPenalizacionesPorUsuarioAsync(int usuarioId);

      

    }
}
