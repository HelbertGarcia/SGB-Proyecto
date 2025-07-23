using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SGB.Presentation.Controllers.PenalizacionController
{
    public class PenalizacionController : Controller
    {
        // GET: PenalizacionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PenalizacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PenalizacionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PenalizacionController/Create
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

        // GET: PenalizacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PenalizacionController/Edit/5
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
