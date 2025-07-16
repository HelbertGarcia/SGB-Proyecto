using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
using SGB.Application.Services.Prestamos_y_PenalizacionServices;
using SGB.Domain.Base;

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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PrestamoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPrestamos()
        {
            var resultado = await _prestamosService.GetAllAsync();

            if (!resultado.IsSuccess)
                return BadRequest(resultado);

            return Ok(resultado.Data);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PrestamoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPrestamoById(int id)
        {
            var resultado = await _prestamosService.GetByIdAsync(id);

            if (!resultado.IsSuccess)
            {
                if (EsNotFound(resultado.Message))
                    return NotFound(resultado);

                return BadRequest(resultado);
            }

            return Ok(resultado.Data);

        }
        [HttpPost]
        [ProducesResponseType(typeof(PrestamoResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPrestamo([FromBody] AddPrestamoDto addPrestamoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos del modelo.",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var result = await _prestamosService.AddAsync(addPrestamoDto);

            if (!result.IsSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = result.Message
                });

            return CreatedAtAction(nameof(GetPrestamoById), new { id = result.Data.Id }, result.Data);
        }




        [HttpPost("Registrar-devolucion")]
        public async Task<IActionResult> RegistrarDevolucion([FromBody] RegistrarDevolucionDto dto)
        {
            var result = await _prestamosService.RegistrarDevolucionAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result); // Esto enviará el mensaje de error

            return Ok(result); // Esto enviará el mensaje de éxito en el body
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(PrestamoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePrestamo(int id, [FromBody] UpdatePrestamoDto updatePrestamoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updatePrestamoDto.IDPrestamo)
                return BadRequest(new { Message = "El ID de la ruta no coincide con el ID del préstamo en el cuerpo de la solicitud." });

            var resultado = await _prestamosService.UpdateAsync(updatePrestamoDto);

            if (!resultado.IsSuccess)
            {
                if (EsNotFound(resultado.Message))
                    return NotFound(resultado);

                return BadRequest(resultado);
            }

            return Ok(resultado.Data);
        }


        

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var dto = new DiseblePrestamoDto { IDPrestamo = id };

            var result = await _prestamosService.DeleteAsync(dto);

            if (!result.IsSuccess)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(new
            {
                success = true,
                message = "Préstamo eliminado (desactivado) correctamente."
            });
        }

       

        private bool EsNotFound(string message)
        {
            if (string.IsNullOrEmpty(message))
                return false;

            return message.Contains("no existe", StringComparison.OrdinalIgnoreCase) ||
                   message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
                   message.Contains("not found", StringComparison.OrdinalIgnoreCase);
        }
    }
}
