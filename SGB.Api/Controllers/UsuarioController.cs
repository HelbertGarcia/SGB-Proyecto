using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using System.Threading.Tasks;

namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;

        public UsuarioController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        [HttpGet(Name = "ObtenerTodosLosUsuarios")]
        public async Task<IActionResult> GetAll()
        {
            var resultado = await _usuarioServices.GetAllAsync();
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("{id}", Name = "ObtenerUsuarioPorId")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _usuarioServices.GetByIdAsync(id);
            if (!resultado.IsSuccess || resultado.Data == null) return NotFound(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("buscar/{termino}", Name = "BuscarUsuarioPorTermino")]
        public async Task<IActionResult> BuscarPorTermino(string termino)
        {
            var resultado = await _usuarioServices.BuscarUsuariosAsync(termino);
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpPost(Name = "CrearUsuario")]
        public async Task<IActionResult> Crear([FromBody] SaveUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _usuarioServices.AddUsuarioAsync(usuarioDto);
            if (!resultado.IsSuccess) return BadRequest(resultado);

            var usuarioCreado = (UsuarioDto)resultado.Data!;
            return CreatedAtAction(nameof(GetById), new { id = usuarioCreado.IDUsuario }, usuarioCreado);
        }

        [HttpPut("{id}", Name = "ActualizarUsuario")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _usuarioServices.UpdateAsync(id, usuarioDto);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }

            return Ok(resultado.Data);
        }

        [HttpDelete("{id}", Name = "EliminarUsuario")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _usuarioServices.DeleteAsync(id);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }

            return NoContent();
        }
    }
}
