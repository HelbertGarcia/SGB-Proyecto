using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using System.Threading.Tasks;

namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet(Name = "ObtenerTodasLasCategorias")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCategorias()
        {
            var resultado = await _categoriaService.GetAllCategoriasAsync();
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpGet("{id}", Name = "ObtenerCategoriaPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoria(int id)
        {
            var resultado = await _categoriaService.GetCategoriaAsync(id);
            if (!resultado.Success)
            {
                return NotFound(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpPost(Name = "CrearCategoria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategoria([FromBody] AddCategoriaDto addCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultado = await _categoriaService.AddCategoriaAsync(addCategoriaDto);
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }

            var categoriaCreada = (CategoriaDto)resultado.Data;
            return CreatedAtAction(nameof(GetCategoria), new { id = categoriaCreada.Id }, categoriaCreada);
        }

        [HttpPut("{id}", Name = "ActualizarCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] UpdateCategoriaDto updateCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultado = await _categoriaService.UpdateCategoriaAsync(id, updateCategoriaDto);
            if (!resultado.Success)
            {
                if (resultado.Message.Contains("encontrado"))
                    return NotFound(resultado);

                return BadRequest(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpDelete("{id}", Name = "EliminarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var resultado = await _categoriaService.DeleteCategoriaAsync(id);
            if (!resultado.Success)
            {
                if (resultado.Message.Contains("encontrado"))
                    return NotFound(resultado);

                return BadRequest(resultado);
            }
            return NoContent();
        }
    }
}