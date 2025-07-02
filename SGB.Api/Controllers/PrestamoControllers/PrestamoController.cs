using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Prestamos;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Domain.Base;
using System;
using SGB.Application.Services.Prestamos_y_PenalizacionServices.PrestamoServices;

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
            var resultado = await _prestamosService.GetAllPrestamosAsync(); 

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
            var resultado = await _prestamosService.GetPrestamoByIdAsync(id);

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
            var result = await _prestamosService.AddPrestamoAsync(addPrestamoDto);

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

            var resultado = await _prestamosService.UpdatePrestamoAsync(updatePrestamoDto);

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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var result = await _prestamosService.DisablePrestamoAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new
            {
                success = true,
                message = "Préstamo eliminado (desactivado) correctamente."
            });
        }
    }
}
