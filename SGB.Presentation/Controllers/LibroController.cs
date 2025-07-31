using Microsoft.AspNetCore.Mvc;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Presentation.Models.Libro;
using SGB.Presentation.Services;

namespace SGB.Presentation.Controllers
{
    public class LibroController : Controller
    {
        private readonly ILibroHttpService _libroHttpService;

        public LibroController(ILibroHttpService libroHttpService)
        {
            _libroHttpService = libroHttpService;
        }

        // GET: Libro
        public async Task<IActionResult> Index()
        {
            var listaDeLibros = await _libroHttpService.ObtenerTodos();
            return View(listaDeLibros);
        }

        // GET: Libro/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var libro = await _libroHttpService.ObtenerPorId(id);
            if (libro == null)
            {
                TempData["ErrorMessage"] = "El libro solicitado no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libro/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = await _libroHttpService.PrepararCreateViewModel();
            return View(viewModel);
        }

        // POST: Libro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibroCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var addDto = new AddLibroDto
                {
                    Titulo = viewModel.Libro.titulo,
                    Autor = viewModel.Libro.autor,
                    ISBN = viewModel.Libro.isbn,
                    Editorial = viewModel.Libro.editorial,
                    FechaPublicacion = viewModel.Libro.fechaPublicacion,
                    IDCategoria = viewModel.Libro.IDCategoria
                };

                var apiResponse = await _libroHttpService.Crear(addDto);
                if (apiResponse.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Ocurrió un error al crear el libro.");
            }

            var refreshedViewModel = await _libroHttpService.PrepararCreateViewModel();
            refreshedViewModel.Libro = viewModel.Libro;
            return View(refreshedViewModel);
        }

        // GET: Libro/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _libroHttpService.PrepararEditViewModel(id);
            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "El libro que intentas editar no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // POST: Libro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibroEditViewModel viewModel)
        {
            if (id != viewModel.Libro.id) return NotFound();

            if (ModelState.IsValid)
            {
                var updateDto = new UpdateLibroDto
                {
                    Titulo = viewModel.Libro.titulo,
                    Autor = viewModel.Libro.autor,
                    Editorial = viewModel.Libro.editorial,
                    FechaPublicacion = viewModel.Libro.fechaPublicacion,
                    IDCategoria = viewModel.Libro.IDCategoria
                };

                var apiResponse = await _libroHttpService.Actualizar(id, updateDto);
                if (apiResponse.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Ocurrió un error al actualizar el libro.");
            }

            var refreshedViewModel = await _libroHttpService.PrepararEditViewModel(id);
            refreshedViewModel.Libro = viewModel.Libro;
            return View(refreshedViewModel);
        }

        // GET: Libro/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _libroHttpService.ObtenerPorId(id);
            if (libro == null)
            {
                TempData["ErrorMessage"] = "El libro que intentas eliminar no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiResponse = await _libroHttpService.Eliminar(id);
            if (!apiResponse.IsSuccess)
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "Ocurrió un error al eliminar el libro.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}