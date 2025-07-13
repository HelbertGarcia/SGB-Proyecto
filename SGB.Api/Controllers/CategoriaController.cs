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
        public async Task<IActionResult> GetAll()
        {
            var resultado = await _categoriaService.GetAllAsync();
            if (!resultado.IsSuccess) return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("{id}", Name = "ObtenerCategoriaPorId")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _categoriaService.GetByIdAsync(id);
            if (!resultado.IsSuccess || resultado.Data == null) return NotFound(resultado);
            return Ok(resultado.Data);
        }

        [HttpPost(Name = "CrearCategoria")]
        public async Task<IActionResult> Crear([FromBody] AddCategoriaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _categoriaService.AddAsync(dto);
            if (!resultado.IsSuccess) return BadRequest(resultado);

            var categoriaCreada = (CategoriaDto)resultado.Data;
            return CreatedAtAction(nameof(GetById), new { id = categoriaCreada.Id }, categoriaCreada);
        }

        [HttpPut("{id}", Name = "ActualizarCategoria")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateCategoriaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _categoriaService.UpdateAsync(id, dto);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return Ok(resultado.Data);
        }

        [HttpDelete("{id}", Name = "EliminarCategoria")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _categoriaService.DeleteAsync(id);
            if (!resultado.IsSuccess)
            {
                if (resultado.Message.Contains("encontrado")) return NotFound(resultado);
                return BadRequest(resultado);
            }
            return NoContent();
        }
    }
}