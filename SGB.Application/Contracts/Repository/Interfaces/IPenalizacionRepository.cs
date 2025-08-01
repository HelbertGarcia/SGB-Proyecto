using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;

namespace SGB.Api.Contracts.Repository.Interfaces
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
        Task<OperationResult<IEnumerable<Penalizacion>>> GetActivePenalizacionesAsync();
        Task<OperationResult<IEnumerable<Penalizacion>>> GetPenalizacionesPorUsuarioAsync(int usuarioId);
    }
}