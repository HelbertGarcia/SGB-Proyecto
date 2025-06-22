using SGB.Domain.Base;
using SGB.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Interfaces
{
    public interface IPersonaRepository: IBaseRepository<Persona>
    {
        Task<OperationResult> DeleteAsync(int id);

        Task<OperationResult> ObtenerPorEmailAsync(string email);

        Task<OperationResult> BuscarPorIdAsync(int id);


        Task<bool> ExisteEmailAsync(string email);


        Task<OperationResult> BuscarPorRolAsync(int idRol);


        Task<OperationResult> ObtenerTodosActivosAsync();


        Task<OperationResult> ActivarCuentaAsync(int idUsuario);



    }
}
