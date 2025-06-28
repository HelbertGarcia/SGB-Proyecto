using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;
using SGB.Domain.Entities.Configuracion;

namespace SGB.Persistence.Interfaces
{
    public interface IConfiguracionRepository
    {
        Task<OperationResult> ObtenerPorNombreAsync(string nombre);
        Task<List<Configuracion>> GetAllAsync();
        Task<Configuracion?> GetEntityByIdAsync(int id);
        Task AddAsync(Configuracion entity);
        Task UpdateEntityAsync(Configuracion entity);
        Task DeleteEntityAsync(Configuracion entity);
    }
}
