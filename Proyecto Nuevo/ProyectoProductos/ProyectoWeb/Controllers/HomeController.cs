using BL;
using ET;
using ProyectoWeb.ViewModel.HomeViewModel;
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
        private CategoriaBL categoriaBL = new CategoriaBL();

        public ActionResult Index()
        {
            IndexViewModel indexVM = new IndexViewModel();
            indexVM.ArticulosDestacados = articuloBL.obtenerDestacados();
            indexVM.Categorias = categoriaBL.obtenerCategoriasDestacadas();
            return View(indexVM);
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