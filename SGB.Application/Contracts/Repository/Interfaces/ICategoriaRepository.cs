using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;

namespace SGB.Api.Contracts.Repository.Interfaces
{
    public interface ICategoriaRepository : IBaseRepository<Categoria>
    {
        Task<OperationResult<Categoria>> ObtenerPorNombreAsync(string nombreCategoria);
    }
}