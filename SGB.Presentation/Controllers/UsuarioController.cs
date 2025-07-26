using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGB.Presentation.Models.Usuario;
using SGB.Application.Wrappers;
using System.Net.Http.Json;

namespace SGB.Presentation.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly string _baseApiUrl = " https://localhost:7299/api";

        // GET: UsuarioController
        public async Task<IActionResult> Index()
        {
            var listaDeUsuarios = new List<UsuarioModel>();

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var response = await client.GetAsync("Usuario/GetAllUsuarios");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<UsuarioModel>>>();
                    if (apiResponse?.IsSuccess == true && apiResponse.Data != null)
                    {
                        listaDeUsuarios = apiResponse.Data;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResponse?.Message ?? "Error desconocido desde la API.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = $"Error al conectar con la API. Código: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Excepción: {ex.Message}";
            }

            return View(listaDeUsuarios);
        }

        // GET: UsuarioController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            UsuarioModel usuario = null;

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var response = await client.GetAsync($"Usuario/GetUsuarioById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UsuarioModel>>();
                    usuario = apiResponse?.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener detalles: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // GET: UsuarioController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var response = await client.PostAsJsonAsync("Usuario/AddUsuario", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                ModelState.AddModelError(string.Empty, errorResponse?.Message ?? "Error al crear usuario.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Excepción: {ex.Message}");
            }

            return View(model);
        }

        // GET: UsuarioController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            UsuarioModel usuario = null;

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var response = await client.GetAsync($"Usuario/GetUsuarioById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UsuarioModel>>();
                    usuario = apiResponse?.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener usuario: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioModel model)
        {
            if (id != model.IDUsuario)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var response = await client.PutAsJsonAsync($"Usuario/UpdateUsuario/{id}", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                ModelState.AddModelError(string.Empty, errorResponse?.Message ?? "Error al actualizar.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Excepción: {ex.Message}");
            }

            return View(model);
        }

        // GET: UsuarioController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            UsuarioModel usuario = null;

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var response = await client.GetAsync($"Usuario/GetUsuarioById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UsuarioModel>>();
                    usuario = apiResponse?.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar usuario: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseApiUrl);

                var response = await client.DeleteAsync($"Usuario/DeleteUsuario/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                TempData["ErrorMessage"] = errorResponse?.Message ?? "Error al eliminar.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Excepción: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

