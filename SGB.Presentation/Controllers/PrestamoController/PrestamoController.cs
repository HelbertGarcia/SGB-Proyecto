using Microsoft.AspNetCore.Mvc;
using SGB.Presentation.Models;
using SGB.Presentation.Models.PrestamoModels;
using SGB.Presentation.Services;
using System.Text;
using System.Text.Json;


namespace SGB.Presentation.Controllers
{
    public class PrestamoController : Controller
    {

        private readonly IPrestamoHttpService _prestamoHttpService;
        private readonly ILogger<PrestamoController> _logger;

        public PrestamoController(IPrestamoHttpService prestamoService, ILogger<PrestamoController> logger)
        {
            _prestamoHttpService = prestamoService;
            _logger = logger;
        }

        // GET: PrestamoController

        // GET: Prestamo
        public async Task<IActionResult> Index()
        {
            var response = await _prestamoHttpService.GetPrestamosAsync();

            if (!response.IsSuccess)
            {
                _logger.LogWarning("Error al obtener los préstamos: {Message}", response.Message);
                TempData["ErrorMessage"] = response.Message ?? "No se pudieron cargar los préstamos.";
                return View(new List<PrestamoModel>());
            }

            return View(response.Data);
        }

        // GET: Prestamo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _prestamoHttpService.GetPrestamoByIdAsync(id);

            if (!response.IsSuccess || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Préstamo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(response.Data);
        }

        // GET: Prestamo/Create
        public IActionResult Create() => View();

        // POST: Prestamo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestamoCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _prestamoHttpService.CreatePrestamoAsync(model);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Préstamo creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el préstamo.");
            _logger.LogWarning("Error al crear el préstamo: {Message}", response.Message);

            return View(model);
        }

        // GET: Prestamo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _prestamoHttpService.GetPrestamoByIdAsync(id);

            if (!response.IsSuccess || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Préstamo no encontrado para edición.";
                return RedirectToAction(nameof(Index));
            }

            var editModel = new PrestamoEditModel
            {
                IDPrestamo = response.Data.id,
                FechaInicio = response.Data.fechaInicio,
                FechaFin = response.Data.fechaFin
            };

            return View(editModel);
        }

        // POST: Prestamo/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrestamoEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _prestamoHttpService.UpdatePrestamoAsync(model);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Préstamo actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar el préstamo.");
            _logger.LogWarning("Error al actualizar préstamo ID {ID}: {Message}", model.IDPrestamo, response.Message);

            return View(model);
        }

        // GET: Prestamo/Devolver/5
        public async Task<IActionResult> Devolver(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            var response = await _prestamoHttpService.GetPrestamoByIdAsync(id);

            if (!response.IsSuccess || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Préstamo no encontrado para devolución.";
                return RedirectToAction(nameof(Index));
            }

            var model = new PrestamoDevolucionModel
            {
                IdPrestamo = response.Data.id,
                FechaDevolucion = DateTime.Now
            };

            return View(model);
        }

        // POST: Prestamo/Devolver
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(PrestamoDevolucionModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _prestamoHttpService.RegistrarDevolucionAsync(model);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Devolución registrada exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response.Message ?? "Error al registrar la devolución.");
            _logger.LogWarning("Error devolviendo préstamo ID {ID}: {Message}", model.IdPrestamo, response.Message);

            return View(model);
        }
































        // no lo vamos a nececitar 

        /*
        
        // GET: PrestamoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }




        // POST: PrestamoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
