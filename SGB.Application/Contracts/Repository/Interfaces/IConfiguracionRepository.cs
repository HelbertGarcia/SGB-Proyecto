using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;

namespace SGB.Api.Contracts.Repository.Interfaces
{
    public interface IConfiguracionRepository : IBaseRepository<Configuracion>
    {
        Task<OperationResult<Configuracion>> ObtenerPorNombreAsync(string nombre);
        Task<OperationResult<IEnumerable<Configuracion>>> GetAllSinFiltroAsync();
    }
}
