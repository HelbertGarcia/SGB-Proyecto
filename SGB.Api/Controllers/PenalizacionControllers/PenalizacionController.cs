using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Penalizacion;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Penalizaciones;

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
        [ProducesResponseType(typeof(IEnumerable<Penalizacion>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPenalizaciones()
        {
            var result = await _penalizacionService.GetAllAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result.Data);
        }

        [HttpGet("{idPenalizacion:int}")]
        [ProducesResponseType(typeof(Penalizacion), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPenalizacionById(int idPenalizacion)
        {
            var result = await _penalizacionService.GetByIdAsync(idPenalizacion);

            if (!result.Success)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Penalizacion), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPenalizacion([FromBody] AddPenalizacionDto addPenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _penalizacionService.AddAsync(addPenalizacionDto);

            if (!result.Success)
                return BadRequest(result);

            var nuevaPenalizacion = result.Data as Penalizacion;

            return CreatedAtAction(nameof(GetPenalizacionById),
                                   new { idPenalizacion = nuevaPenalizacion?.Id },
                                   nuevaPenalizacion);
        }

        [HttpPut("{idPenalizacion:int}")]
        [ProducesResponseType(typeof(Penalizacion), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePenalizacion(int idPenalizacion, [FromBody] UpdatePenalizacionDto updatePenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (idPenalizacion != updatePenalizacionDto.IDPenalizacion)
                return BadRequest(new { Message = "El ID de la ruta no coincide con el del cuerpo de la solicitud." });

            var result = await _penalizacionService.UpdateAsync(updatePenalizacionDto);

            if (!result.Success)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DisablePenalizacion([FromBody] DisablePenalizacionDto disablePenalizacionDto)
        {
            var result = await _penalizacionService.DeleteAsync(disablePenalizacionDto);

            if (!result.Success)
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
