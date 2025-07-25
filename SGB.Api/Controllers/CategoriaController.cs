using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Wrappers;
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

        [HttpGet("GetAllCategorias")]
        public async Task<IActionResult> GetAll()
        {
            var resultado = await _categoriaService.GetAllAsync();

            if (!resultado.IsSuccess)
            {
                var errorResponse = new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = resultado.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }

            var successResponse = new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Categorías obtenidas correctamente.",
                Data = resultado.Data
            };

            return Ok(successResponse);
        }

        [HttpGet("GetCategoriaById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _categoriaService.GetByIdAsync(id);

            if (!resultado.IsSuccess || resultado.Data == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = resultado.Message ?? "Categoría no encontrada.",
                    Data = null
                };
                return NotFound(errorResponse);
            }

            var successResponse = new ApiResponse<CategoriaDto>
            {
                IsSuccess = true,
                Message = "Categoría obtenida correctamente.",
                Data = resultado.Data
            };

            return Ok(successResponse);

        }

        [HttpPost("AddCategoria")]
        public async Task<IActionResult> Crear([FromBody] AddCategoriaDto dto)
        {
            if (!ModelState.IsValid)
            {
                var validationErrorResponse = new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Los datos proporcionados no son válidos.",
                    Data = ModelState 
                };
                return BadRequest(validationErrorResponse);
            }

            var resultado = await _categoriaService.AddAsync(dto);

            if (!resultado.IsSuccess)
            {
                var errorResponse = new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = resultado.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }

            var successResponse = new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Categoría creada exitosamente.",
                Data = resultado.Data
            };

            var categoriaCreada = (CategoriaDto)resultado.Data;
            return CreatedAtAction(nameof(GetById), new { id = categoriaCreada.Id }, successResponse);
        }

        [HttpPut("UpdateCategoria/{id}")]
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

        [HttpDelete("DisableCategoria/{id}")]
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