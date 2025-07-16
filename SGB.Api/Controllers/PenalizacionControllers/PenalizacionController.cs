using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using SGB.Application.Contracts.Interfaces;

namespace SGB.Api.Controllers.PenalizacionControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenalizacionController : ControllerBase
    {
        private readonly IPenalizacionServices _penalizacionService;

        public PenalizacionController(IPenalizacionServices penalizacionService)
        {
            _penalizacionService = penalizacionService ?? throw new ArgumentNullException(nameof(penalizacionService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PenalizacionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPenalizaciones()
        {
            var result = await _penalizacionService.GetAllAsync();

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }

        [HttpGet("{idPenalizacion:int}")]
        [ProducesResponseType(typeof(PenalizacionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPenalizacionById(int idPenalizacion)
        {
            var result = await _penalizacionService.GetByIdAsync(idPenalizacion);

            if (!result.IsSuccess)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PenalizacionResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPenalizacion([FromBody] AddPenalizacionDto addPenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _penalizacionService.AddAsync(addPenalizacionDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            var nuevaPenalizacionDto = result.Data as PenalizacionResponseDto;

            return CreatedAtAction(nameof(GetPenalizacionById),
                                   new { idPenalizacion = nuevaPenalizacionDto?.IDPenalizacion },
                                   nuevaPenalizacionDto);
        }

        [HttpPut("{idPenalizacion:int}")]
        [ProducesResponseType(typeof(PenalizacionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePenalizacion(int idPenalizacion, [FromBody] UpdatePenalizacionDto updatePenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (idPenalizacion != updatePenalizacionDto.IDPenalizacion)
                return BadRequest(new { Message = "El ID de la ruta no coincide con el del cuerpo de la solicitud." });

            var result = await _penalizacionService.UpdateAsync(updatePenalizacionDto);

            if (!result.IsSuccess)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result.Data);
        }

        // Aquí cambiamos a recibir el ID en la ruta para borrar / desactivar
        [HttpDelete("{idPenalizacion:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DisablePenalizacion(int idPenalizacion)
        {
            if (idPenalizacion <= 0)
                return BadRequest(new { Message = "ID inválido." });

            var dto = new DisablePenalizacionDto { IDPenalizacion = idPenalizacion };
            var result = await _penalizacionService.DeleteAsync(dto);

            if (!result.IsSuccess)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
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
