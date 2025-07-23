using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SGB.Presentation.Models;
using System.Net.Http;

namespace SGB.Presentation.Controllers.PrestamoController
{
    public class PrestamoController : Controller
    {
        // GET: PrestamoController
        public async Task<IActionResult> Index()
        {

            GetAllPrestamoResponse getAllPrestamoResponse ;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7299/api");

                    var response = await client.GetAsync("GetAllPrestamos");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllPrestamoResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllPrestamoResponse>(responseString);
                    }
                    else
                    {
                        getAllPrestamoResponse = new GetAllPrestamoResponse
                        {
                            isSuccess = false,
                            message = "Error al obtener los préstamos",
                          
                        };
                    }
                }


            }

            catch (Exception ex)
            {
                getAllPrestamoResponse = new GetAllPrestamoResponse
                {
                    isSuccess = false,
                    message = ex.Message,
                   
                };
            }



            
            return View();
        }

        // GET: PrestamoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PrestamoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrestamoController/Create
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

        // GET: PrestamoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PrestamoController/Edit/5
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
