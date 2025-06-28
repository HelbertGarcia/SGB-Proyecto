using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;

namespace SGB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IConfiguracionService _configuracionService;

        public AdminController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        // GET: api/admin/configuraciones
        [HttpGet("configuraciones")]
        public async Task<IActionResult> ObtenerTodas()
        {
            var resultado = await _configuracionService.GetAllAsync();
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        // GET: api/admin/configuraciones/{id}
        [HttpGet("configuraciones/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _configuracionService.GetByIdAsync(id);
            return resultado.Success ? Ok(resultado) : NotFound(resultado);
        }

        // POST: api/admin/configuraciones
        [HttpPost("configuraciones")]
        public async Task<IActionResult> Crear([FromBody] AddConfiguracionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _configuracionService.SaveAsync(dto);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        // PUT: api/admin/configuraciones
        [HttpPut("configuraciones")]
        public async Task<IActionResult> Actualizar([FromBody] UpdateConfiguracionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _configuracionService.UpdateAsync(dto);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        // DELETE (soft): api/admin/configuraciones
        [HttpDelete("configuraciones")]
        public async Task<IActionResult> Eliminar([FromBody] DeleteConfiguracionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _configuracionService.DeleteAsync(dto);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }
    }
}