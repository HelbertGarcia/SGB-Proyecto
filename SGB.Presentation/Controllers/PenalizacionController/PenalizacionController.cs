using Microsoft.AspNetCore.Mvc;
using SGB.Presentation.Models;
using SGB.Presentation.Models.PenalizacionModels;
using SGB.Presentation.Services;


namespace SGB.Presentation.Controllers
{
    public class PenalizacionController : Controller
    {
        private readonly IPenalizacionHttpService _penalizacionHttpService;
        private readonly ILogger<PenalizacionController> _logger;

        public PenalizacionController(IPenalizacionHttpService penalizacionHttpService, ILogger<PenalizacionController> logger)
        {
            _penalizacionHttpService = penalizacionHttpService;
            _logger = logger;
        }

        // GET: Penalizacion
        public async Task<IActionResult> Index()
        {
            var response = await _penalizacionHttpService.GetPenalizacionesAsync();

            if (!response.IsSuccess)
            {
                _logger.LogWarning("Error al obtener las penalizaciones: {Message}", response.Message);
                TempData["ErrorMessage"] = response.Message ?? "No se pudieron cargar las penalizaciones.";
                return View(new List<PenalizacionModel>());
            }

            return View(response.Data);
        }

        // GET: Penalizacion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _penalizacionHttpService.GetPenalizacionByIdAsync(id);

            if (!response.IsSuccess || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Penalización no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(response.Data);
        }

        // GET: Penalizacion/Create
        public IActionResult Create() => View();




        // POST: Penalizacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenalizacionCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _penalizacionHttpService.CreatePenalizacionAsync(model);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Penalización creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear la penalización.");
            _logger.LogWarning("Error al crear la penalización: {Message}", response.Message);

            return View(model);
        }



        // GET: Penalizacion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _penalizacionHttpService.GetPenalizacionByIdAsync(id);

            if (!response.IsSuccess || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Penalización no encontrada para edición.";
                return RedirectToAction(nameof(Index));
            }

            var penalizacion = response.Data;

            var editModel = new PenalizacionEditModel
            {
                IDPenalizacion = penalizacion.idPenalizacion,
                FechaFin = penalizacion.fechaFin,
                Motivo = penalizacion.motivo,
                Monto = penalizacion.monto
            };

            return View(editModel);
        }



        // POST: Penalizacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PenalizacionEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _penalizacionHttpService.UpdatePenalizacionAsync(model);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Penalización actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar la penalización.");
            _logger.LogWarning("Error al actualizar penalización ID {ID}: {Message}", model.IDPenalizacion, response.Message);

            return View(model);
        }





        // GET: Penalizacion/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _penalizacionHttpService.GetPenalizacionByIdAsync(id);
            if (!response.IsSuccess || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Penalización no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }




        // POST: Penalizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _penalizacionHttpService.DeletePenalizacionAsync(id);

            if (!response.IsSuccess)
            {
                TempData["ErrorMessage"] = response.Message ?? "Error al eliminar la penalización.";
                _logger.LogWarning("Error al eliminar penalización ID {ID}: {Message}", id, response.Message);
            }
            else
            {
                TempData["SuccessMessage"] = "Penalización eliminada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
