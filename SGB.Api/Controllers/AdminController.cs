using SGB.Api.Handlers;
using Microsoft.AspNetCore.Mvc;
using SGB.Api.Dtos.ConfiguracionDto;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IConfiguracionHandler _handler;
    public AdminController(IConfiguracionHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("GetAllConfigurations")]
    public async Task<IActionResult> GetAllAsync() =>
        Ok(await _handler.GetAllAsync());

    [HttpGet("GetConfigurationById")]
    public async Task<IActionResult> GetByIdAsync(int id) =>
        Ok(await _handler.GetByIdAsync(id));

    [HttpGet("GetConfigurationByName")]
    public async Task<IActionResult> GetByNameAsync(string name) =>
        Ok(await _handler.GetByNameAsync(name));

    [HttpPost("AddConfiguration")]
    public async Task<IActionResult> CreateAsync([FromBody] AddConfiguracionDto dto) =>
        Ok(await _handler.CreateAsync(dto));

    [HttpPut("UpdateConfiguration")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateConfiguracionDto dto) =>
        Ok(await _handler.UpdateAsync(id, dto));

    [HttpDelete("DeleteConfiguration")]
    public async Task<IActionResult> DeleteAsync(int id) =>
        Ok(await _handler.DeleteAsync(id));
}
