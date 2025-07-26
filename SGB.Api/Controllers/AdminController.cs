using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.AdministracionDto;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Wrappers;
using SGB.Persistence.Context;

namespace SGB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IConfiguracionService _configuracionService;
        private readonly SGBContext _dbContext;

        public AdminController(
            IConfiguracionService configuracionService,
            SGBContext dbContext)
        {
            _configuracionService = configuracionService;
            _dbContext = dbContext;
        }

        // GET: api/admin/Get_Configuraciones
        [HttpGet("Get_Configuraciones")]
        public async Task<IActionResult> GetConfiguraciones()
        {
            var configuraciones = await _dbContext.Configuraciones.ToListAsync();

            var result = new ApiResponse<List<ConfiguracionDto>>
            {
                IsSuccess = true,
                Data = configuraciones.Select(c => new ConfiguracionDto
                {
                    IDConfiguracion = c.IDConfiguracion,
                    Nombre = c.Nombre,
                    Valor = c.Valor,
                    Descripcion = c.Descripcion,
                    FechaCreacion = c.FechaCreacion,
                    EstaActivo = c.EstaActivo
                }).ToList(),
                Message = "Listado obtenido correctamente"
            };

            return Ok(result);
        }

        // GET: api/admin/Get_Configuraciones_By_Id?id=5
        [HttpGet("Get_Configuraciones_By_Id")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _configuracionService.GetByIdAsync(id);
            return resultado.IsSuccess ? Ok(resultado) : NotFound(resultado);
        }

        // PUT: api/admin/Actualizar_configuraciones?id=5
        [HttpPut("Actualizar_configuraciones")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateConfiguracionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _configuracionService.UpdateAsync(id, dto);
            return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
        }

        // DELETE: api/admin/Delete_Configuraciones?id=5
        [HttpDelete("Delete_Configuraciones")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _configuracionService.DeleteAsync(id);
            return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
        }

        // POST: api/admin/Agregar_Configuraciones
        [HttpPost("Agregar_Configuraciones")]
        public async Task<IActionResult> Crear([FromBody] AddConfiguracionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _configuracionService.AddAsync(dto);
            return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
        }
    }
}
