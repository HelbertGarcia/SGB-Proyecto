using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Application.Wrappers;
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

        [HttpGet("GetPenalizaciones")]
        public async Task<IActionResult> GetAllPenalizaciones()
        {
            var result = await _penalizacionService.GetAllAsync();

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = result.Message ?? "Error al obtener penalizaciones"
                });

            return Ok(new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Penalizaciones obtenidas correctamente",
                Data = result.Data
            });
        }

        [HttpGet("GetPenalizacionById")]
        public async Task<IActionResult> GetPenalizacionById(int idPenalizacion)
        {
            var result = await _penalizacionService.GetByIdAsync(idPenalizacion);

            if (!result.IsSuccess)
                return NotFound(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = result.Message ?? "Penalización no encontrada"
                });

            return Ok(new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Penalización obtenida correctamente",
                Data = result.Data
            });
        }

        [HttpPost("AddPenalizacion")]
        public async Task<IActionResult> AddPenalizacion([FromBody] AddPenalizacionDto addPenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Datos inválidos",
                    Data = ModelState
                });

            var result = await _penalizacionService.AddAsync(addPenalizacionDto);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = result.Message
                });

            var nuevaPenalizacionDto = result.Data;

            return CreatedAtAction(nameof(GetPenalizacionById),
                new { idPenalizacion = nuevaPenalizacionDto.IDPenalizacion },
                new ApiResponse<object>
                {
                    IsSuccess = true,
                    Message = "Penalización creada correctamente",
                    Data = nuevaPenalizacionDto
                });
        }

        [HttpPut("UpdatePenalizacion")]
        public async Task<IActionResult> UpdatePenalizacion(int idPenalizacion, [FromBody] UpdatePenalizacionDto updatePenalizacionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Datos inválidos",
                    Data = ModelState
                });

            if (idPenalizacion != updatePenalizacionDto.IDPenalizacion)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "El ID de la ruta no coincide con el del cuerpo de la solicitud"
                });

            var result = await _penalizacionService.UpdateAsync(updatePenalizacionDto);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = result.Message
                });

            return Ok(new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Penalización actualizada correctamente",
                Data = result.Data
            });
        }

         [HttpDelete("DisablePenalizacion")]
        public async Task<IActionResult> DisablePenalizacion([FromBody] DisablePenalizacionDto disableDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Datos inválidos",
                    Data = ModelState
                });

            var result = await _penalizacionService.DeleteAsync(disableDto);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = result.Message
                });

            return Ok(new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Penalización desactivada correctamente"
            });
        }

    }
}

