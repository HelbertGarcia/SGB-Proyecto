using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Domain.Base;

namespace SGB.Persistence.Interfaces
{
    public interface IConfiguracionRepository
    {
        Task<OperationResult> ObtenerPorNombreAsync(string nombre);
    }
}
