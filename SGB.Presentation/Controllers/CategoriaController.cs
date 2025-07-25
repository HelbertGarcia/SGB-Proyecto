using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGB.Application.Dtos.LibrosDto.CategoriaDto;
using SGB.Application.Wrappers;
using SGB.Domain.Entities.Categoria;
using SGB.Presentation.Models.Categoria;

namespace SGB.Presentation.Controllers
{
    public class CategoriaController : Controller
    {
        // GET: CategoriaController
        public async Task<IActionResult> Index()
        {
            var listaDeCategorias = new List<CategoriaModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/");

                    var response = await client.GetAsync("Categoria/GetAllCategorias");

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaModel>>>();

                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                        {
                            listaDeCategorias = apiResponse.Data;
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

            return View(listaDeCategorias);
        }

        // GET: CategoriaController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CategoriaModel categoria = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/");

                    var response = await client.GetAsync($"Categoria/GetCategoriaById/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CategoriaModel>>();
                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                        {
                            categoria = apiResponse.Data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió una excepción: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (categoria == null)
            {
                TempData["ErrorMessage"] = "La categoría solicitada no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // GET: Categoria/Create
        // Este método simplemente muestra el formulario vacío.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        // Este método recibe los datos del formulario y los envía a la API.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaModel categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addDto = new AddCategoriaDto { Nombre = categoria.nombre };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7299/api/");

                        var response = await client.PostAsJsonAsync("Categoria/AddCategoria", addDto);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                            ModelState.AddModelError(string.Empty, errorResponse?.Message ?? "Ocurrió un error al crear la categoría.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error de excepción: {ex.Message}");
                }
            }

            return View(categoria);
        }

        // GET: CategoriaController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            CategoriaModel categoria = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/");

                    var response = await client.GetAsync($"Categoria/GetCategoriaById/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CategoriaModel>>();
                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                        {
                            categoria = apiResponse.Data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar la categoría: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (categoria == null)
            {
                TempData["ErrorMessage"] = "La categoría que intentas editar no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaModel categoria)
        {
            if (id != categoria.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updateDto = new UpdateCategoriaDto { Nombre = categoria.nombre };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7299/api/");

                        var response = await client.PutAsJsonAsync($"Categoria/UpdateCategoria/{id}", updateDto);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar la categoría.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error de excepción: {ex.Message}");
                }
            }
            return View(categoria);
        }

        // GET: CategoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
