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
        [HttpGet("Get_Configuraciones")]
        public async Task<IActionResult> ObtenerTodo()
        {
            var resultado = await _configuracionService.GetAllAsync();
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        // GET: api/admin/configuraciones/{id}
        [HttpGet("Get_Configuraciones_By_Id")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _configuracionService.GetByIdAsync(id);
            return resultado.Success ? Ok(resultado) : NotFound(resultado);
        }

        // PUT: api/admin/configuraciones
        [HttpPut("Actualizar_configuraciones")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateConfiguracionDto dto)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);
            dto.IDConfiguracion = id;
            var resultado = await _configuracionService.UpdateAsync(dto);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        // DELETE (soft): api/admin/configuraciones
        [HttpDelete("Delete_Configuraciones")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var dto = new DeleteConfiguracionDto { IDConfiguracion = id };
            var resultado = await _configuracionService.DeleteAsync(dto);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        // POST: api/admin/configuraciones
        [HttpPost("Agregar_Configuraciones")]
        public async Task<IActionResult> Crear([FromBody] AddConfiguracionDto dto)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);
            var resultado = await _configuracionService.SaveAsync(dto);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }
    }
}