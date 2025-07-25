using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using SGB.Application.Dtos.AdministracionDto;
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

        // ✅ LISTAR TODAS LAS CONFIGURACIONES
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("Admin/Get_Configuraciones");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Error al obtener la lista.";
                return View(new List<ConfiguracionModel>());
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ConfiguracionModel>>>();
            return View(result?.Data ?? new List<ConfiguracionModel>());
        }

        // ✅ VER DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var config = await ObtenerConfiguracionPorId(id);
            if (config == null)
            {
                TempData["ErrorMessage"] = "Configuración no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(config);
        }

        // ✅ CREAR CONFIGURACIÓN
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddConfiguracionDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var response = await _client.PostAsJsonAsync("Admin/Agregar_Configuraciones", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Configuración creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al crear configuración.");
            return View(dto);
        }

        // ✅ EDITAR CONFIGURACIÓN
        public async Task<IActionResult> Edit(int id)
        {
            var config = await ObtenerConfiguracionPorId(id);
            if (config == null)
            {
                TempData["ErrorMessage"] = "Configuración no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var dto = new UpdateConfiguracionDto
            {
                IDConfiguracion = config.idConfiguracion,
                Nombre = config.nombre,
                Valor = config.valor,
                Descripcion = config.descripcion,
                EstaActivo = config.estaActivo
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateConfiguracionDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var response = await _client.PutAsJsonAsync($"Admin/Actualizar_configuraciones?id={dto.IDConfiguracion}", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Configuración actualizada.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al actualizar.");
            return View(dto);
        }

        // ✅ ELIMINAR CONFIGURACIÓN
        public async Task<IActionResult> Delete(int id)
        {
            var config = await ObtenerConfiguracionPorId(id);
            if (config == null)
            {
                TempData["ErrorMessage"] = "Configuración no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(config);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _client.DeleteAsync($"Admin/Delete_Configuraciones?id={id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Configuración eliminada.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ MÉTODO REUTILIZABLE
        private async Task<ConfiguracionModel?> ObtenerConfiguracionPorId(int id)
        {
            var response = await _client.GetAsync($"Admin/Get_Configuraciones_By_Id?id={id}");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ConfiguracionModel>>();
            return result?.IsSuccess == true ? result.Data : null;
        }
    }
}
