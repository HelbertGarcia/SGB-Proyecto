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
            //
            _libroService = libroService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllLibros()
        {
            var resultado = await _libroService.GetAllLibrosAsync();
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpGet("{isbn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLibroPorIsbn(string isbn)
        {
            var resultado = await _libroService.GetLibroDetailsAsync(isbn);
            if (!resultado.Success)
            {
                return NotFound(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpGet("buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarLibros([FromQuery] string termino)
        {
            var resultado = await _libroService.GetLibrosAsync(termino);
            return Ok(resultado.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearLibro([FromBody] AddLibroDto libroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _libroService.AddLibroAsync(libroDto);

            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }

            var libroCreado = resultado.Data;
            return CreatedAtAction(nameof(GetLibroPorIsbn), new { isbn = libroCreado.ISBN }, libroCreado);
        }

        [HttpPut("{isbn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActualizarLibro(string isbn, [FromBody] UpdateLibroDto libroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _libroService.UpdateLibroAsync(isbn, libroDto);

            if (!resultado.Success)
            {
                if (resultado.Message.Contains("no encontrado"))
                    return NotFound(resultado);

                return BadRequest(resultado);
            }

            return Ok(resultado.Data);
        }

        [HttpDelete("{isbn}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EliminarLibro(string isbn)
        {
            var resultado = await _libroService.DeleteLibroAsync(isbn);

            if (!resultado.Success)
            {
                if (resultado.Message.Contains("no encontrado"))
                    return NotFound(resultado);

                return BadRequest(resultado);
            }

            return NoContent();
        }
    }
}