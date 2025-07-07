using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using SGB.Domain.Repository;

namespace SGB.Persistence.Interfaces
{
    public interface IConfiguracionRepository : IBaseRepository<Configuracion>
    {
        Task<OperationResult> ObtenerPorNombreAsync(string nombre);
    }
}
