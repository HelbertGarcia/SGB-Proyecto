using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
        Task<OperationResult<IEnumerable<Penalizacion>>> GetActivePenalizacionesAsync();
        Task<OperationResult<IEnumerable<Penalizacion>>> GetPenalizacionesPorUsuarioAsync(int usuarioId);
    }
}