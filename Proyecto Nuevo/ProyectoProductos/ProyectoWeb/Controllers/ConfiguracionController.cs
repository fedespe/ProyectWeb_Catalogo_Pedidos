using ET;
using ProyectoWeb.ViewModel.ConfiguracionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class ConfiguracionController : Controller
    {
        //GET: Articulo/Editar
        public ActionResult Editar()
        {
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    EditarViewModel editVM = new EditarViewModel();
                    //editVM.completarEditarVM();
                    return View(editVM);
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }
            else
            {
                try
                {
                    ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }
        }

        //POST: Articulo/Editar
        [HttpPost]
        public ActionResult Editar(EditarViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                try
                {                        
                    editVM.guardarArchivos();
                    editVM = new EditarViewModel();
                    editVM.mensajeSuccess = "Cambios realizados con exito.";
                    return View(editVM);
                }
                catch (ProyectoException ex)
                {
                    editVM.mensajeError = ex.Message;
                    return View(editVM);
                }
            }
            else {
                editVM.mensajeError = "Ocurrio un error, verifique los datos.";
                return View(editVM);
            }
        }
    }
}