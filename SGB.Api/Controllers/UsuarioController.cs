using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.IUsuarioServices;
using SGB.Application.Dtos.UsuarioDto.UsuarioDto;
using SGB.Application.Services.UsuarioServices;
using SGB.Application.Wrappers;


namespace SGB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _UsuarioServices;

        public UsuarioController(IUsuarioServices UsuarioServices)
        {
            _UsuarioServices = UsuarioServices;
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAll()
        {
            var resultado = await _UsuarioServices.GetAllAsync();
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("(GetById)/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _UsuarioServices.GetByIdAsync(id);
            if (!resultado.IsSuccess || resultado.Data == null)
                return NotFound(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("buscar/{termino}")]
        public async Task<IActionResult> BuscarUsuariosAsync(string termino)
        {
            var resultado = await _UsuarioServices.BuscarUsuariosAsync(termino);
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] SaveUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _UsuarioServices.AddAsync(usuarioDto);
            if (!resultado.IsSuccess) return BadRequest(resultado);

            var usuarioCreado = resultado.Data!;
            return CreatedAtAction(nameof(GetById), new { id = usuarioCreado.IDUsuario }, usuarioCreado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _UsuarioServices.UpdateAsync(id, usuarioDto);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _UsuarioServices.DeleteAsync(id);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return NoContent();
        }
    }
}

