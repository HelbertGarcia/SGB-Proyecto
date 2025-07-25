using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Categoria;
using SGB.Presentation.Models.Libro;

namespace SGB.Presentation.Controllers
{
    public class LibroController : Controller
    {
        // GET: LibroController
        public async Task<IActionResult> Index()
        {
            var listaDeLibros = new List<LibroModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/");

                    var response = await client.GetAsync("Libro/GetAllLibros");

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<LibroModel>>>();

                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                        {
                            listaDeLibros = apiResponse.Data;
                        }
                        else
                        {
                            ViewBag.ErrorMessage = apiResponse?.Message ?? "Error desconocido desde la API.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se pudo conectar con la API. Código: " + response.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción al procesar la solicitud: {ex.Message}";
            }

            return View(listaDeLibros);
        }

        // GET: LibroController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            LibroModel libro = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/");

                    var response = await client.GetAsync($"Libro/GetLibroById/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LibroModel>>();
                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                        {
                            libro = apiResponse.Data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió una excepción: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (libro == null)
            {
                TempData["ErrorMessage"] = "La categoría solicitada no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(libro);
        }

        // GET: Libro/Create
        // Este método prepara el formulario. Su principal trabajo es obtener la lista de categorías.
        public async Task<IActionResult> Create()
        {
            var categorias = new List<CategoriaModel>();

            try
            {
                // Se usa 'using (var client = new HttpClient())' como en tu CategoriaController.
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/");
                    var categoriasResponse = await client.GetAsync("Categoria/GetAllCategorias");

                    if (categoriasResponse.IsSuccessStatusCode)
                    {
                        var apiResponse = await categoriasResponse.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaModel>>>();
                        categorias = apiResponse?.Data ?? new List<CategoriaModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"No se pudieron cargar las categorías. Error: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            // Creamos el ViewModel y lo pasamos a la vista.
            var viewModel = new LibroCreateViewModel
            {
                Libro = new LibroModel(), // Un libro nuevo y vacío
                CategoriasDisponibles = categorias.Select(c => new SelectListItem
                {
                    Text = c.nombre,
                    Value = c.id.ToString()
                })
            };

            return View(viewModel);
        }

        // POST: Libro/Create
        // Este método recibe los datos del formulario y los envía a la API.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibroCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
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

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7299/api/");

                        var response = await client.PostAsJsonAsync("Libro/AddLibro", addDto);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                            ModelState.AddModelError(string.Empty, errorResponse?.Message ?? "Ocurrió un error al crear el libro.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error de excepción: {ex.Message}");
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7299/api/");
                var categoriasResponse = await client.GetAsync("Categoria/GetAllCategorias");
                if (categoriasResponse.IsSuccessStatusCode)
                {
                    var apiResponse = await categoriasResponse.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaModel>>>();
                    viewModel.CategoriasDisponibles = (apiResponse?.Data ?? new List<CategoriaModel>()).Select(c => new SelectListItem
                    {
                        Text = c.nombre,
                        Value = c.id.ToString()
                    });
                }
            }

            return View(viewModel);
        }

        // GET: Libro/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            LibroModel libro = null;
            var categorias = new List<CategoriaModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/");

                    var libroResponse = await client.GetAsync($"Libro/GetLibroById/{id}");
                    if (libroResponse.IsSuccessStatusCode)
                    {
                        var apiResponse = await libroResponse.Content.ReadFromJsonAsync<ApiResponse<LibroModel>>();
                        libro = apiResponse?.Data;
                    }

                    var categoriasResponse = await client.GetAsync("Categoria/GetAllCategorias");
                    if (categoriasResponse.IsSuccessStatusCode)
                    {
                        var apiResponse = await categoriasResponse.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaModel>>>();
                        categorias = apiResponse?.Data ?? new List<CategoriaModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error de conexión con la API: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (libro == null)
            {
                TempData["ErrorMessage"] = "El libro que intentas editar no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var categoriaActual = categorias.FirstOrDefault(c => c.nombre == libro.nombreCategoria);
            if (categoriaActual != null)
            {
                libro.IDCategoria = categoriaActual.id;
            }

            var viewModel = new LibroEditViewModel
            {
                Libro = libro,
                CategoriasDisponibles = categorias.Select(c => new SelectListItem
                {
                    Text = c.nombre,
                    Value = c.id.ToString()
                })
            };

            return View(viewModel);
        }

        // POST: Libro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibroEditViewModel viewModel)
        {

            if (id != viewModel.Libro.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updateDto = new UpdateLibroDto
                    {
                        Titulo = viewModel.Libro.titulo,
                        Autor = viewModel.Libro.autor,
                        Editorial = viewModel.Libro.editorial,
                        FechaPublicacion = viewModel.Libro.fechaPublicacion,
                        IDCategoria = viewModel.Libro.IDCategoria 
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7299/api/");

                        var response = await client.PutAsJsonAsync($"Libro/UpdateLibro/{id}", updateDto);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                            ModelState.AddModelError(string.Empty, errorResponse?.Message ?? "Ocurrió un error al actualizar el libro.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error de excepción: {ex.Message}");
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7299/api/");
                var categoriasResponse = await client.GetAsync("Categoria/GetAllCategorias");
                if (categoriasResponse.IsSuccessStatusCode)
                {
                    var apiResponse = await categoriasResponse.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaModel>>>();
                    viewModel.CategoriasDisponibles = (apiResponse?.Data ?? new List<CategoriaModel>()).Select(c => new SelectListItem
                    {
                        Text = c.nombre,
                        Value = c.id.ToString()
                    });
                }
            }

            return View(viewModel);
        }

        // GET: LibroController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibroController/Delete/5
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
        }
    }
}
