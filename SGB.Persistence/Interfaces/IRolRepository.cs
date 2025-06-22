using SGB.Domain.Base;
using SGB.Domain.Entities.Rol;
using SGB.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Interfaces
{
    public interface IRolRepository: IBaseRepository<Rol>
    {


        Task<OperationResult> ObtenerPorNombreAsync(string nombre);
        Task<OperationResult> ObtenerTodosActivosAsync();
        Task<OperationResult> ActivarRolAsync(int idRol);
        Task<OperationResult> DeleteAsync(int id);




    }
}
