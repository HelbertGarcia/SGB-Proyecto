using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Wrappers;
using SGB.Application.Dtos.LibrosDto.LibroDto;

namespace SGB.Api.Controllers
{
    [Route("api/Libro")]
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

            if (!resultado.IsSuccess)
            {
                return BadRequest(new ApiResponse<object> { IsSuccess = false, Message = resultado.Message });
            }

            var successResponse = new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Libros obtenidos correctamente.",
                Data = resultado.Data
            };
            return Ok(successResponse);
        }

        [HttpGet("GetLibroById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _libroService.GetByIdAsync(id);

            if (!resultado.IsSuccess || resultado.Data == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = resultado.Message ?? "Libro no encontrado.",
                };
                return NotFound(errorResponse);
            }

            var successResponse = new ApiResponse<LibroDto>
            {
                IsSuccess = true,
                Message = "Libro obtenido correctamente.",
                Data = resultado.Data
            };
            return Ok(successResponse);
        }

        [HttpGet("buscar/{isbn}")]
        public async Task<IActionResult> BuscarPorIsbn(string isbn)
        {
            var resultado = await _libroService.BuscarPorIsbnAsync(isbn);

            if (!resultado.IsSuccess)
            {
                return BadRequest(new ApiResponse<object> { IsSuccess = false, Message = resultado.Message });
            }
            if (resultado.Data == null)
            {
                return NotFound(new ApiResponse<object> { IsSuccess = false, Message = "Libro no encontrado con ese ISBN." });
            }

            return Ok(new ApiResponse<object> { IsSuccess = true, Data = resultado.Data });
        }

        [HttpPost("AddLibro")]
        public async Task<IActionResult> Crear([FromBody] AddLibroDto libroDto)
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

            var resultado = await _libroService.AddAsync(libroDto);

            if (!resultado.IsSuccess)
            {
                return BadRequest(new ApiResponse<object> { IsSuccess = false, Message = resultado.Message });
            }

            var successResponse = new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Libro creado exitosamente.",
                Data = resultado.Data
            };

            var libroCreado = (LibroDto)resultado.Data;
            return CreatedAtAction(nameof(GetById), new { id = libroCreado.Id }, successResponse);
        }

        [HttpPut("UpdateLibro/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateLibroDto libroDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<object> { IsSuccess = false, Message = "Datos inválidos.", Data = ModelState });

            var resultado = await _libroService.UpdateAsync(id, libroDto);

            if (!resultado.IsSuccess)
            {
                var errorResponse = new ApiResponse<object> { IsSuccess = false, Message = resultado.Message };
                if (resultado.Message.Contains("encontrado")) return NotFound(errorResponse);
                return BadRequest(errorResponse);
            }

            var successResponse = new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Libro actualizado correctamente.",
                Data = resultado.Data
            };
            return Ok(successResponse);
        }

        [HttpDelete("DisableLibro/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _libroService.DeleteAsync(id);
            if (!resultado.IsSuccess)
            {
                var errorResponse = new ApiResponse<object> { IsSuccess = false, Message = resultado.Message };
                if (resultado.Message.Contains("encontrado")) return NotFound(errorResponse);
                return BadRequest(errorResponse);
            }

            return Ok(new ApiResponse<bool> { IsSuccess = true, Message = "Libro desactivado correctamente.", Data = true });
        }
    }
}