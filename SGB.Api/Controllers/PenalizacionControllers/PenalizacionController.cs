using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using SGB.Application.Contracts.Service.IPrestamos_PenalizacionServices.Penalizacion;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Domain.Base;

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
        [ProducesResponseType(typeof(IEnumerable<GetPenalizacionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPenalizaciones()
        {
            var getPenalizacionDto = new GetPenalizacionDto();
            var result = await _penalizacionService.GetAllPenalizacionesAsync(getPenalizacionDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result.Data);
        }

    
        [HttpGet("{idPenalizacion:int}")]
        [ProducesResponseType(typeof(GetPenalizacionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPenalizacionById(int idPenalizacion)
        {
            var result = await _penalizacionService.GetPenalizacionByIdAsync(idPenalizacion);

            if (!result.Success)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result.Data);
        }

     
        [HttpPost]
        [ProducesResponseType(typeof(GetPenalizacionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPenalizacion([FromBody] AddPenalizacionDto addPenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _penalizacionService.AddPenalizacionAsync(addPenalizacionDto);

            if (!result.Success)
                return BadRequest(result);

            if (result.Data is GetPenalizacionDto nuevaPenalizacion)
            {
                return CreatedAtAction(nameof(GetPenalizacionById),
                                     new { idPenalizacion = nuevaPenalizacion.IDPenalizacion },
                                     nuevaPenalizacion);
            }
            return Ok(result.Data);
        }

    
        [HttpPut("{idPenalizacion:int}")]
        [ProducesResponseType(typeof(GetPenalizacionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePenalizacion(int idPenalizacion, [FromBody] UpdatePenalizacionDto updatePenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (idPenalizacion != updatePenalizacionDto.IDPenalizacion)
                return BadRequest(new { Message = "El ID de la ruta no coincide con el del cuerpo de la solicitud." });

            var result = await _penalizacionService.UpdatePenalizacionAsync(updatePenalizacionDto);

            if (!result.Success)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DisablePenalizacion([FromBody] DisablePenalizacionDto disablePenalizacionDto)
        {
            var result = await _penalizacionService.DisablePenalizacionAsync(disablePenalizacionDto);

            if (!result.Success)
            {
                if (EsNotFound(result.Message))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }



        /// Evalúa si un mensaje sugiere que el recurso no fue encontrado.

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