using Microsoft.AspNetCore.Mvc;
using SGB.Application.Dtos.ConfiguracionDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models;

namespace SGB.Presentation.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly HttpClient _client;

        public ConfiguracionController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7299/api/")
            };
        }

        public async Task<IActionResult> Index()
        {
            var listaConfiguraciones = new List<ConfiguracionModel>();

            try
            {
                var response = await _client.GetAsync("Admin/GetAllConfiguraciones");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ConfiguracionModel>>>();

                    if (apiResponse?.IsSuccess == true && apiResponse.Data != null)
                    {
                        listaConfiguraciones = apiResponse.Data;
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
                ViewBag.ErrorMessage = $"Ocurrió una excepción: {ex.Message}";
            }

            return View(listaConfiguraciones);
        }

        public async Task<IActionResult> Details(int id)
        {
            ConfiguracionModel config = null;

            try
            {
                var response = await _client.GetAsync($"Admin/GetConfiguracionById?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ConfiguracionModel>>();
                    if (apiResponse?.IsSuccess == true && apiResponse.Data != null)
                    {
                        config = apiResponse.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Excepción al obtener detalles: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            if (config == null)
            {
                TempData["ErrorMessage"] = "Configuración no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(config);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddConfiguracionDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var response = await _client.PostAsJsonAsync("Admin/AddConfiguracion", dto);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Configuración creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                ModelState.AddModelError(string.Empty, error?.Message ?? "Error al crear la configuración.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Excepción: {ex.Message}");
            }

            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _client.GetAsync($"Admin/GetConfiguracionById?id={id}");
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ConfiguracionDto>>();

                if (response.IsSuccessStatusCode && apiResponse?.IsSuccess == true && apiResponse.Data != null)
                {
                    var editModel = new ConfiguracionEditModel
                    {
                        IDConfiguracion = apiResponse.Data.IDConfiguracion,
                        Nombre = apiResponse.Data.Nombre,
                        Valor = apiResponse.Data.Valor,
                        Descripcion = apiResponse.Data.Descripcion,
                        EstaActivo = apiResponse.Data.EstaActivo
                    };

                    return View(editModel);
                }

                TempData["ErrorMessage"] = apiResponse?.Message ?? "Configuración no encontrada.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConfiguracionEditModel model)
        {
            if (id != model.IDConfiguracion)
            {
                TempData["ErrorMessage"] = "El ID proporcionado no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var updateDto = new UpdateConfiguracionDto
                {
                    IDConfiguracion = model.IDConfiguracion,
                    Nombre = model.Nombre,
                    Valor = model.Valor,
                    Descripcion = model.Descripcion,
                    EstaActivo = model.EstaActivo
                };

                var response = await _client.PutAsJsonAsync($"Admin/UpdateConfiguracion?id={id}", updateDto);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                if (response.IsSuccessStatusCode && result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Configuración actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result?.Message ?? "No se pudo actualizar.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Excepción al actualizar: {ex.Message}");
            }

            return View(model);
        }
    }
}
