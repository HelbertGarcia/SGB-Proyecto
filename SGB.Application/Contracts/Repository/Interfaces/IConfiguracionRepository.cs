using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface IConfiguracionRepository : IBaseRepository<Configuracion>
    {
        Task<OperationResult<Configuracion>> ObtenerPorIdAsync(int idConfiguracion);
        Task<OperationResult<Configuracion>> ObtenerPorNombreAsync(string nombre);
    }
}
