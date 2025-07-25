using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGB.Presentation.Models;
using SGB.Presentation.Models.PenalizacionModels;
using System.Text.Json;
using System.Text;


namespace SGB.Presentation.Controllers.PenalizacionController
{
    public class PenalizacionController : Controller
    {

        private readonly string _baseApiUrl = "https://localhost:7299/api/";



        // GET: PenalizacionController
        
        public async Task<IActionResult> Index()
        {
            List<PenalizacionModel> penalizaciones;

            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(_baseApiUrl);
                    var response = await client.GetAsync("Penalizacion/GetPenalizaciones");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        penalizaciones = System.Text.Json.JsonSerializer.Deserialize<List<PenalizacionModel>>(responseString);

                    }
                    else
                    {
                        penalizaciones = new List<PenalizacionModel>();
                    }


                }
                
            }
            catch(Exception ex)
            {
                penalizaciones = new List<PenalizacionModel>();
            }

            return View(penalizaciones);
        }









        // GET: PenalizacionController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            PenalizacionModel penalizacion = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseApiUrl);
                    var response = await client.GetAsync($"Penalizacion/GetPenalizacionById?idPenalizacion={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        penalizacion = System.Text.Json.JsonSerializer.Deserialize<PenalizacionModel>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo detalles de la penalización: {ex.Message}");
            }

            if (penalizacion == null)
                return NotFound();

            return View(penalizacion);
        }







        // GET: PenalizacionController/Create
        public ActionResult Create()
        {
            return View();
        }









        // POST: PenalizacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenalizacionCreateModel model)
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
                    IDPrestamo = model.IDPrestamo,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin,
                    Monto = model.Monto,
                    Motivo = model.Motivo
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Penalizacion/AddPenalizacion", jsonContent);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Error al crear la penalización.");
            }
            catch
            {
                ModelState.AddModelError("", "Error inesperado al crear la penalización.");
            }

            return View(model);
        }








        // GET: PenalizacionController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            PenalizacionModel penalizacion = null;

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);
                var response = await client.GetAsync($"Penalizacion/GetPenalizacionById?idPenalizacion={id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    penalizacion = JsonSerializer.Deserialize<PenalizacionModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch { }

            if (penalizacion == null) return NotFound();

            var editModel = new PenalizacionEditModel
            {
                
               
                FechaFin = penalizacion.fechaFin,
                Motivo = penalizacion.motivo,
               Monto = penalizacion.monto,
            };

            return View(editModel);
        }









        // POST: PenalizacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PenalizacionEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var dto = new
                {
                  
                  
                    FechaFin = model.FechaFin,
                    Motivo = model.Motivo,
                    Monto = model.Monto
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Penalizacion/UpdatePenalizacion?idPenalizacion={model.IDPenalizacion}", jsonContent);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Error al actualizar la penalización.");
            }
            catch
            {
                ModelState.AddModelError("", "Error inesperado al actualizar la penalización.");
            }

            return View(model);
        }







        //nolo necesitaremos

        /*
        // GET: PenalizacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PenalizacionController/Delete/5
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
