using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.IConfiguracionService;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Wrappers;

namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguracionService _configuracionService;

        public AdminController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }
       
            [HttpGet("GetAllConfiguraciones")]
            public async Task<IActionResult> GetAll()
            {
                var resultado = await _configuracionService.GetAllAsync();
                if (!resultado.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = resultado.Message,
                        Data = null
                    });
                }
                return Ok(new ApiResponse<object>
                {
                    IsSuccess = true,
                    Message = "Configuraciones obtenidas correctamente.",
                    Data = resultado.Data
                });
            }

            [HttpGet("GetConfiguracionById")]
            public async Task<IActionResult> GetById(int id)
            {
                var resultado = await _configuracionService.GetByIdAsync(id);

                if (!resultado.IsSuccess || resultado.Data == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = resultado.Message ?? "Configuración no encontrada.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<ConfiguracionDto>
                {
                    IsSuccess = true,
                    Message = "Configuración obtenida correctamente.",
                    Data = resultado.Data
                });
            }

            [HttpGet("GetConfiguracionByName")]
            public async Task<IActionResult> GetByName(string nombre)
            {
                var resultado = await _configuracionService.ObtenerPorNombreAsync(nombre);
                if (!resultado.IsSuccess || resultado.Data == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Configuración no encontrada por nombre.",
                        Data = null
                    });
                }
                return Ok(new ApiResponse<ConfiguracionDto>
                {
                    IsSuccess = true,
                    Message = "Configuración obtenida correctamente.",
                    Data = resultado.Data
                });
            }

            [HttpPost("AddConfiguracion")]
            public async Task<IActionResult> Crear([FromBody] AddConfiguracionDto dto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Los datos proporcionados no son válidos.",
                        Data = ModelState
                    });
                }
                var resultado = await _configuracionService.AddAsync(dto);
                if (!resultado.IsSuccess)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = resultado.Message,
                        Data = null
                    });
                }
                var configCreada = (ConfiguracionDto)resultado.Data;
                return CreatedAtAction(nameof(GetById), new { id = configCreada.IDConfiguracion }, new ApiResponse<object>
                {
                    IsSuccess = true,
                    Message = "Configuración creada exitosamente.",
                    Data = configCreada
                });
            }

            [HttpPut("UpdateConfiguracion")]
            public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateConfiguracionDto dto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Los datos proporcionados no son válidos.",
                        Data = ModelState
                    });
                }
                var resultado = await _configuracionService.UpdateAsync(id, dto);
                if (!resultado.IsSuccess)
                {
                    if ((resultado.Message ?? "").Contains("encontrado"))
                    {
                        return NotFound(new ApiResponse<object>
                        {
                            IsSuccess = false,
                            Message = resultado.Message,
                            Data = null
                        });
                    }
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = resultado.Message,
                        Data = null
                    });
                }
                return Ok(new ApiResponse<object>
                {
                    IsSuccess = true,
                    Message = "Configuración actualizada correctamente.",
                    Data = resultado.Data
                });
            }

            [HttpDelete("DisableConfiguracion")]
            public async Task<IActionResult> Eliminar(int id)
            {
                var resultado = await _configuracionService.DeleteAsync(id);

                if (!resultado.IsSuccess)
                {
                    if ((resultado.Message ?? "").Contains("encontrado"))
                    {
                        return NotFound(new ApiResponse<object>
                        {
                            IsSuccess = false,
                            Message = resultado.Message,
                            Data = null
                        });
                    }
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = resultado.Message,
                        Data = null
                    });
                }
                return NoContent();
            }
        }




    }
