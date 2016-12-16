using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;

namespace ProyectoWeb.Controllers
{
    public class ArticuloController : Controller
    {
        private ArticuloBL articuloBL = new ArticuloBL();

        public ActionResult Index()
        {
            try
            {
                return View(articuloBL.obtenerTodos());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        public ActionResult Editar(int id = 0)
        {
            try
            {
                return View(id == 0 ? new Articulo() : articuloBL.obtener(id));
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        public ActionResult Guardar(Articulo articulo)
        {
            try
            {
                bool r = true;
                if (articulo.Id > 0)
                {
                    r = articuloBL.actualizar(articulo);
                }
                else {
                    articuloBL.registrar(articulo);
                }
                if (!r)
                {
                    // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                    ViewBag.Mensaje = "Ocurrio un error inesperado";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                return Redirect("~/");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        public ActionResult Inhabilitar(int id)
        {
            try
            {
                var r = articuloBL.inhabilitar(id);

                if (!r)
                {
                    // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                    ViewBag.Mensaje = "Ocurrio un error inesperado";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                return Redirect("~/");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }
        public ActionResult Habilitar(int id)
        {
            try
            {
                var r = articuloBL.habilitar(id);

                if (!r)
                {
                    // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                    ViewBag.Mensaje = "Ocurrio un error inesperado";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                return Redirect("~/");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        public ActionResult Destacar(int id)
        {
            try
            {
                var r = articuloBL.destacar(id);

                if (!r)
                {
                    // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                    ViewBag.Mensaje = "Ocurrio un error inesperado";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                return Redirect("~/");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        public ActionResult QuitarDestacado(int id)
        {
            try
            {
                var r = articuloBL.quitarDestacado(id);

                if (!r)
                {
                    // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                    ViewBag.Mensaje = "Ocurrio un error inesperado";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                return Redirect("~/");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

    }
}