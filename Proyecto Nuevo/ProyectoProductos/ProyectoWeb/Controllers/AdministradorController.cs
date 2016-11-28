using BL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class AdministradorController : Controller
    {
        private AdministradorBL administradorBL = new AdministradorBL();

        public ActionResult Index()
        {
            return View(administradorBL.obtenerTodos());
        }

        public ActionResult Editar(int id = 0)
        {
            return View(id == 0 ? new Administrador() : administradorBL.obtener(id));
        }

        public ActionResult Guardar(Administrador admin)
        {
            bool r = true;
            if (admin.Id > 0)
            {
                r=administradorBL.actualizar(admin);
            }
            else {
                administradorBL.registrar(admin);
            }
            if (!r)
            {
                // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                ViewBag.Mensaje = "Ocurrio un error inesperado";
                return View("~/Views/Shared/_Mensajes.cshtml");
            }

            return Redirect("~/");
        }

        //    public ActionResult Eliminar(int id)
        //    {
        //        var r = usuarioBL.Eliminar(id);

        //        if (!r)
        //        {
        //            // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
        //            ViewBag.Mensaje = "Ocurrio un error inesperado";
        //            return View("~/Views/Shared/_Mensajes.cshtml");
        //        }

        //        return Redirect("~/");
        //    }    
    }
}