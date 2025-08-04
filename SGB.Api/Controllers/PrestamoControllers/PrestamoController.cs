using Microsoft.AspNetCore.Mvc;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Application.Wrappers;


namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamosServices _prestamosService;

        public PrestamoController(IPrestamosServices prestamosService)
        {
            _prestamosService = prestamosService ?? throw new ArgumentNullException(nameof(prestamosService));
        }

        // GET: api/Prestamo/GetPrestamos
        [HttpGet("GetPrestamos")]
        public async Task<IActionResult> GetAllPrestamos()
        {
            var resultado = await _prestamosService.GetAllAsync();

            if (!resultado.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = resultado.Message
                });

            return Ok(new ApiResponse<IEnumerable<PrestamoResponseDto>>
            {
                IsSuccess = true,
                Message = "Préstamos obtenidos correctamente.",
                Data = resultado.Data
            });
        }

        // GET: api/Prestamo/GetPrestamosById?id=1
        [HttpGet("GetPrestamosById")]
        public async Task<IActionResult> GetPrestamoById([FromQuery] int id)
        {
            var resultado = await _prestamosService.GetByIdAsync(id);

            if (!resultado.IsSuccess || resultado.Data == null)
                return NotFound(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = resultado.Message ?? "Préstamo no encontrado."
                });

            return Ok(new ApiResponse<PrestamoResponseDto>
            {
                IsSuccess = true,
                Message = "Préstamo obtenido correctamente.",
                Data = resultado.Data
            });
        }

        // POST: api/Prestamo/AddPrestamo
        [HttpPost("AddPrestamo")]
        public async Task<IActionResult> AddPrestamo([FromBody] AddPrestamoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Datos inválidos.",
                    Data = ModelState
                });

            var result = await _prestamosService.AddAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = result.Message
                });

            return CreatedAtAction(nameof(GetPrestamoById), new { id = result.Data.Id }, new ApiResponse<PrestamoResponseDto>
            {
                IsSuccess = true,
                Message = "Préstamo creado correctamente.",
                Data = result.Data
            });
        }

        [HttpPost("Registrar-devolucion")]
        public async Task<IActionResult> RegistrarDevolucion([FromBody] RegistrarDevolucionDto dto)
        {
            var result = await _prestamosService.RegistrarDevolucionAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Data = false
                });
            }

            return Ok(new ApiResponse<bool>
            {
                IsSuccess = true,
                Message = result.Message,
                Data = true
            });
        }



        // PUT: api/Prestamo/UpdatePrestamo?id=1
        [HttpPut("UpdatePrestamo")]
        public async Task<IActionResult> UpdatePrestamo([FromQuery] int id, [FromBody] UpdatePrestamoDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Datos inválidos.",
                    Data = ModelState
                });

            if (id != updateDto.IDPrestamo)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "El ID de la ruta no coincide con el ID del préstamo."
                });

            var resultado = await _prestamosService.UpdateAsync(updateDto);

            if (!resultado.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = resultado.Message
                });

            return Ok(new ApiResponse<PrestamoResponseDto>
            {
                IsSuccess = true,
                Message = "Préstamo actualizado correctamente.",
                Data = resultado.Data
            });
        }



        // DELETE: api/Prestamo/DisablePrestamo?id=1
        [HttpDelete("DisablePrestamo")]
        public async Task<IActionResult> DeletePrestamo([FromQuery] int id)
        {
            var dto = new DiseblePrestamoDto { IDPrestamo = id };
            var result = await _prestamosService.DeleteAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = result.Message
                });

            return Ok(new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Préstamo desactivado correctamente."
            });
        }
    }
}
