using Microsoft.AspNetCore.Mvc;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Presentation.Models.Categoria;
using SGB.Presentation.Services;

namespace SGB.Presentation.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaHttpService _categoriaHttpService;

        public CategoriaController(ICategoriaHttpService categoriaHttpService)
        {
            _categoriaHttpService = categoriaHttpService;
        }

        // GET: Categoria
        public async Task<IActionResult> Index()
        {
            var listaDeCategorias = await _categoriaHttpService.ObtenerTodas();
            return View(listaDeCategorias);
        }

        // GET: Categoria/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var categoria = await _categoriaHttpService.ObtenerPorId(id);
            if (categoria == null)
            {
                TempData["ErrorMessage"] = "La categoría solicitada no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaModel categoria)
        {
            if (ModelState.IsValid)
            {
                var addDto = new AddCategoriaDto { Nombre = categoria.nombre };
                var apiResponse = await _categoriaHttpService.Crear(addDto);
                if (apiResponse.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Ocurrió un error al crear la categoría.");
            }
            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _categoriaHttpService.ObtenerPorId(id);
            if (categoria == null)
            {
                TempData["ErrorMessage"] = "La categoría que intentas editar no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaModel categoria)
        {
            if (id != categoria.id) return NotFound();

            if (ModelState.IsValid)
            {
                var updateDto = new UpdateCategoriaDto { Nombre = categoria.nombre };
                var apiResponse = await _categoriaHttpService.Actualizar(id, updateDto);
                if (apiResponse.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Ocurrió un error al actualizar la categoría.");
            }
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaHttpService.ObtenerPorId(id);
            if (categoria == null)
            {
                TempData["ErrorMessage"] = "La categoría que intentas eliminar no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiResponse = await _categoriaHttpService.Eliminar(id);
            if (!apiResponse.IsSuccess)
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "Ocurrió un error al eliminar la categoría.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}