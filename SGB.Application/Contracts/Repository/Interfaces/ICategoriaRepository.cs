using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Contracts.Repository.Interfaces
{
    public interface ICategoriaRepository: IBaseRepository<Categoria>
    {
        Task<OperationResult> ObtenerPorNombreAsync(string nombreCategoria);
    }
}
