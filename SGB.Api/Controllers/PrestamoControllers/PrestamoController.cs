using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Prestamos;
using SGB.Application.Contracts.Interfaces.Service.IPrestamos_PenalizacionServices.Prestamos;

namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamosServices _prestamosService;

        public PrestamoController(IPrestamosServices prestamosService)
        {
            _prestamosService = prestamosService;
        }

        [HttpGet("GetAllPrestamos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPrestamos()
        {
            var resultado = await _prestamosService.GetAllAsync(); 

            if (!resultado.Success)
                return BadRequest(resultado);

            return Ok(resultado.Data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPrestamoById(int id)
        {
            var resultado = await _prestamosService.GetByIdAsync(id);

            if (!resultado.Success)
            {
                if (resultado.Message.Contains("no existe", StringComparison.OrdinalIgnoreCase) ||
                    resultado.Message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
                    resultado.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(resultado);
                }
                return BadRequest(resultado);
            }

            return Ok(resultado.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPrestamo([FromBody] AddPrestamoDto addPrestamoDto)
        {
            var result = await _prestamosService.AddAsync(addPrestamoDto);

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = "Préstamo creado correctamente.", data = result.Data });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePrestamo(int id, [FromBody] UpdatePrestamoDto updatePrestamoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updatePrestamoDto.IDPrestamo)
                return BadRequest(new { Message = "El ID de la ruta no coincide con el ID del préstamo en el cuerpo de la solicitud." });

            var resultado = await _prestamosService.UpdateAsync(updatePrestamoDto);

            if (!resultado.Success)
            {
                if (resultado.Message.Contains("no existe", StringComparison.OrdinalIgnoreCase) ||
                    resultado.Message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
                    resultado.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(resultado);
                }
                return BadRequest(resultado);
            }

            return Ok(resultado.Data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var dto = new DiseblePrestamoDto { IDPrestamo = id };

            var result = await _prestamosService.DeleteAsync(dto);

            if (!result.Success)
            {
                if (result.Message.Contains("no existe", StringComparison.OrdinalIgnoreCase) ||
                    result.Message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
                    result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }

            return Ok(new
            {
                success = true,
                message = "Préstamo eliminado (desactivado) correctamente."
            });
        }

    }
}

