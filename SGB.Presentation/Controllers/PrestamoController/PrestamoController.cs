using Microsoft.AspNetCore.Mvc;
using SGB.Presentation.Models;
using SGB.Presentation.Models.PrestamoModels;
using System.Text;
using System.Text.Json;


namespace SGB.Presentation.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly string _baseApiUrl = "https://localhost:7299/api/";
        // Consistente HTTPS

        // GET: PrestamoController

        public async Task<IActionResult> Index()
        {
            List<PrestamoModel> prestamos;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseApiUrl);
                    var response = await client.GetAsync("Prestamo/GetPrestamos");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        prestamos = System.Text.Json.JsonSerializer.Deserialize<List<PrestamoModel>>(responseString);
                    }
                    else
                    {
                        prestamos = new List<PrestamoModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                prestamos = new List<PrestamoModel>();
            }

            return View(prestamos);
        }






        // GET: PrestamoController/Details/5
        // Acción para detalles de un préstamo por ID
        public async Task<IActionResult> Details(int id)
        {
            PrestamoModel prestamo = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseApiUrl);
                    var response = await client.GetAsync($"Prestamo/GetPrestamosById?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        prestamo = System.Text.Json.JsonSerializer.Deserialize<PrestamoModel>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (prestamo == null)
                return NotFound();

            return View(prestamo);
        }








        // GET: PrestamoController/Create
        public ActionResult Create()
        {
            return View();
        }









        // POST: PrestamoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestamoCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var dto = new
                {
                    UsuarioId = model.UsuarioId,
                    ISBN = model.ISBN,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Prestamo/AddPrestamo", jsonContent);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error al crear el préstamo: {errorContent}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado al crear el préstamo: {ex.Message}");
            }

            return View(model);
        }









        // GET: PrestamoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            PrestamoModel prestamo = null;
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);
                var response = await client.GetAsync($"Prestamo/GetPrestamosById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    prestamo = JsonSerializer.Deserialize<PrestamoModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch { }

            if (prestamo == null) return NotFound();

            // Mapear PrestamoModel a PrestamoEditModel para la vista
            var editModel = new PrestamoEditModel
            {
                IDPrestamo = prestamo.id,
                FechaInicio = prestamo.fechaInicio,
                FechaFin = prestamo.fechaFin
            };

            return View(editModel);
        }














        // POST: PrestamoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrestamoEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var dto = new
                {
                    IDPrestamo = model.IDPrestamo,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Prestamo/UpdatePrestamo?id={model.IDPrestamo}", jsonContent);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error al actualizar el préstamo: {errorContent}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado al actualizar el préstamo: {ex.Message}");
            }

            return View(model);
        }







        // GET: PrestamoController/Devolver/5
        public async Task<IActionResult> Devolver(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            PrestamoModel prestamo = null;

            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(_baseApiUrl) };
                var response = await client.GetAsync($"Prestamo/GetPrestamosById?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    prestamo = JsonSerializer.Deserialize<PrestamoModel>(
                        responseString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener préstamo: {ex.Message}";
            }

            if (prestamo == null)
                return NotFound("No se encontró el préstamo.");

            var model = new PrestamoDevolucionModel
            {
                IdPrestamo = prestamo.id,
                FechaDevolucion = DateTime.Now
            };

            return View(model);
        }


        // POST: PrestamoController/Devolver
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(PrestamoDevolucionModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var dto = new
                {
                    idPrestamo = model.IdPrestamo,
                    fechaDevolucion = model.FechaDevolucion
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Prestamo/Registrar-devolucion", jsonContent);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Error al registrar la devolución.");
            }
            catch
            {
                ModelState.AddModelError("", "Error inesperado al devolver el préstamo.");
            }

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
