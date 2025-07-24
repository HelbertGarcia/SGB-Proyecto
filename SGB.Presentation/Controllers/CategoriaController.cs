using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGB.Application.Wrappers;
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

                    // La ruta de la API es correcta según tu implementación
                    var response = await client.GetAsync("Categoria/GetAllCategorias");

                    if (response.IsSuccessStatusCode)
                    {
                        // --- CORRECCIÓN CLAVE ---
                        // 2. Se deserializa la respuesta en el modelo ApiResponse<T>
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoriaModel>>>();

                        // 3. Se comprueba el estado de la operación y si hay datos
                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                        {
                            listaDeCategorias = apiResponse.Data;
                        }
                        else
                        {
                            // Si la API reportó un fallo, se puede guardar el mensaje para mostrarlo al usuario
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: CategoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: CategoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
