using BL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class HomeController : Controller
    {
        private ArticuloBL articuloBL = new ArticuloBL();
        public ActionResult Index()
        {
            List<Articulo> articulosDestacados = articuloBL.obtenerDestacados();
            return View(articulosDestacados);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Gestion()
        {
            return View();
        }
    }
}