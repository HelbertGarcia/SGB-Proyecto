using SGB.Domain.Entities.Penalizaciones;
using SGB.Domain.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
        /// <summary>
        /// Obtiene las penalizaciones activas para un usuario dado.
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <returns>OperationResult con la lista de penalizaciones activas</returns>
        Task<OperationResult<List<Penalizacion>>> GetPenalizacionesActivasPorUsuarioAsync(int usuarioId);
    }
}
