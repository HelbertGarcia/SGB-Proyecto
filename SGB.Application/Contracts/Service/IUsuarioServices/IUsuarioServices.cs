using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Usuario;


namespace SGB.Application.Contracts.Service.IUsuarioServices
{
  public interface IUsuarioServices
    {
        public interface IUsuarioService
        {
            Task<OperationResult> AddUsuarioAsync(SaveUsuarioDto usuarioDto);
            Task<OperationResult> UpdateUsuarioAsync(int idUsuario, UpdateUsuarioDto usuarioDto);
            Task<OperationResult> DeleteUsuarioAsync(int idUsuario);
            Task<OperationResult> ObtenerDetallesUsuarioAsync(int idUsuario);
            Task<OperationResult> BuscarUsuariosAsync(string terminoBusqueda);
            Task<OperationResult> ObtenerTodosLosUsuariosAsync();
            Task<OperationResult> GetByIdAsync(int id);
            Task<OperationResult> SearchAsync(string terminoBusqueda);
            Task<OperationResult> GetAllAsync();
            Task<OperationResult> AddAsync(Usuario usuario);
            Task<OperationResult> AddAsync(UsuarioDto usuarioDto);
            Task<OperationResult> UpdateAsync(UsuarioDto usuarioDto);
        }





    }
}
