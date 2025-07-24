using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Libro;

namespace SGB.Presentation.Controllers
{
    public class LibroController : Controller
    {
        // GET: LibroController
        public async Task<IActionResult> Index()
        {
            ApiResponse<List<LibroModel>> GetAllLibrosResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api/"); 
                    var response = await client.GetAsync("Libro/GetAllLibros");
                }
            }
            catch (Exception ex)
            {
                
            }
            return View();
        }

        // GET: LibroController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibroController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibroController/Create
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

        // GET: LibroController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibroController/Edit/5
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
