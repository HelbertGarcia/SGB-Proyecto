using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using System.Threading.Tasks;

namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ILibroService _libroService;

        public LibroController(ILibroService libroService)
        {
            _libroService = libroService;
        }

        [HttpGet("GetAllLibros")]
        public async Task<IActionResult> GetAll()
        {
            var resultado = await _libroService.GetAllAsync();
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("GetLibroById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _libroService.GetByIdAsync(id);
            if (!resultado.IsSuccess || resultado.Data == null) return NotFound(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("buscar/{isbn}")]
        public async Task<IActionResult> BuscarPorIsbn(string isbn)
        {
            var resultado = await _libroService.BuscarPorIsbnAsync(isbn);
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpPost("AddLibro")]
        public async Task<IActionResult> Crear([FromBody] AddLibroDto libroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _libroService.AddAsync(libroDto);
            if (!resultado.IsSuccess) return BadRequest(resultado);

            var libroCreado = (LibroDto)resultado.Data;
            return CreatedAtAction(nameof(GetById), new { id = libroCreado.Id }, libroCreado);
        }

        [HttpPut("UpdateLibro/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateLibroDto libroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _libroService.UpdateAsync(id, libroDto);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpDelete("DisableLibro/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _libroService.DeleteAsync(id);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return NoContent();
        }
    }
}