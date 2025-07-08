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

        [HttpGet(Name = "ObtenerTodosLosLibros")]
        public async Task<IActionResult> GetAll()
        {
            var resultado = await _libroService.GetAllAsync();
            if (!resultado.Success) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("{id}", Name = "ObtenerLibroPorId")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _libroService.GetByIdAsync(id);
            if (!resultado.Success || resultado.Data == null) return NotFound(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("buscar/isbn/{isbn}", Name = "BuscarLibroPorIsbn")]
        public async Task<IActionResult> BuscarPorIsbn(string isbn)
        {
            var resultado = await _libroService.BuscarPorIsbnAsync(isbn);
            if (!resultado.Success) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpPost(Name = "CrearLibro")]
        public async Task<IActionResult> Crear([FromBody] AddLibroDto libroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _libroService.AddAsync(libroDto);
            if (!resultado.Success) return BadRequest(resultado);

            var libroCreado = (LibroDto)resultado.Data;
            return CreatedAtAction(nameof(GetById), new { id = libroCreado.Id }, libroCreado);
        }

        [HttpPut("{id}", Name = "ActualizarLibro")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateLibroDto libroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _libroService.UpdateAsync(id, libroDto);
            if (!resultado.Success)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpDelete("{id}", Name = "EliminarLibro")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _libroService.DeleteAsync(id);
            if (!resultado.Success)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return NoContent();
        }
    }
}