using Microsoft.AspNetCore.Mvc;
using SGB.Application.Contracts.Service.IUsuarioServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;

        public UsuarioController(IUsuarioServices usuarioServices) 
        {
            _usuarioServices = usuarioServices;
        }


        
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var result = await _usuarioServices.ObtenerDetallesUsuarioAsync();
            return new string[] { "value1", "value2" };
        }

       
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

       
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

       
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

       
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
