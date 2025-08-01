using SGB.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using SGB.Api.Dtos.ConfiguracionDto;
using SGB.Presentation.Handlers;

namespace SGB.Presentation.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly IConfiguracionAppHandler _handler;

        public ConfiguracionController(IConfiguracionAppHandler handler)
        {
            _handler = handler;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _handler.GetAllAsync();
            if (!lista.Any())
                ViewBag.ErrorMessage = "No se encontraron configuraciones.";
            return View(lista);
        }

        public async Task<IActionResult> Details(int id)
        {
            var configuracion = await _handler.GetByIdAsync(id);

            if (configuracion == null)
            {
                TempData["ErrorMessage"] = $"No se encontró la configuración con ID {id}.";
                return RedirectToAction("Index");
            }

            return View(configuracion);
        }


        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddConfiguracionDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            var response = await _handler.CreateAsync(dto);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Configuración creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, response.Message ?? "Error desconocido.");
            return View(dto);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var configuracion = await _handler.GetByIdAsync(id);
            if (configuracion == null)
                return NotFound();
            var model = new ConfiguracionEditModel
            {
                IDConfiguracion = configuracion.IDConfiguracion,
                Nombre = configuracion.Nombre,
                Valor = configuracion.Valor,
                Descripcion = configuracion.Descripcion,
                EstaActivo = configuracion.EstaActivo
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConfiguracionEditModel model)
        {
            if (id != model.IDConfiguracion)
            {
                TempData["ErrorMessage"] = "El ID proporcionado no coincide.";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
                return View(model);
            var dto = new UpdateConfiguracionDto
            {
                IDConfiguracion = model.IDConfiguracion,
                Nombre = model.Nombre,
                Valor = model.Valor,
                Descripcion = model.Descripcion,
                EstaActivo = model.EstaActivo
            };
            var response = await _handler.UpdateAsync(dto);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Configuración actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar la configuración.");
            return View(model);
        }
    }
}
