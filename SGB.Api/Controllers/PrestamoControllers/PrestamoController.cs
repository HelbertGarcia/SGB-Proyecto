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
        public async Task<IActionResult> GetAllPrestamos()
        {
            var resultado = await _prestamosService.GetAllAsync();
            if (!resultado.IsSuccess)
                return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPrestamoById(int id)
        {
            var resultado = await _prestamosService.GetByIdAsync(id);
            if (!resultado.IsSuccess)
                return BadRequest(resultado);
            return Ok(resultado.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddPrestamo([FromBody] AddPrestamoDto addPrestamoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _prestamosService.AddAsync(addPrestamoDto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return CreatedAtAction(nameof(GetPrestamoById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPost("Registrar-devolucion")]
        public async Task<IActionResult> RegistrarDevolucion([FromBody] RegistrarDevolucionDto dto)
        {
            var result = await _prestamosService.RegistrarDevolucionAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePrestamo(int id, [FromBody] UpdatePrestamoDto updatePrestamoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updatePrestamoDto.IDPrestamo)
                return BadRequest(new { Message = "El ID de la ruta no coincide con el ID del préstamo en el cuerpo de la solicitud." });

            var resultado = await _prestamosService.UpdateAsync(updatePrestamoDto);
            if (!resultado.IsSuccess)
                return BadRequest(resultado);

            return Ok(resultado.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var dto = new DiseblePrestamoDto { IDPrestamo = id };
            var result = await _prestamosService.DeleteAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(new { success = true, message = "Préstamo eliminado (desactivado) correctamente." });
        }
    }
}
