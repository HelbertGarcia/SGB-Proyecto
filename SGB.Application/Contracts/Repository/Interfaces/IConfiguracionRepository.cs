using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IConfiguracionRepository : IBaseRepository<Configuracion>
    {
        Task<OperationResult<Configuracion>> ObtenerPorNombreAsync(string nombre);
    }
}
