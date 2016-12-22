using BL;
using ET;
using ProyectoWeb.ViewModel.AdministradorViewModel;
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

        //GET: Administrador/ListaAdministradores
        public ActionResult ListaAdministradores()
        {
            try {
                return View(administradorBL.obtenerTodos());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //GET: Administrador/Crear
        public ActionResult Crear()
        {
            try
            {
                return View(new CrearViewModel());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //POST: Cliente/Crear
        [HttpPost]
        public ActionResult Crear(CrearViewModel crearVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    crearVM.completarAdministrador();
                    administradorBL.registrar(crearVM.administrador);
                    return RedirectToAction("ListaAdministradores");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }
            else {
                return View(crearVM);
            }
        }

        //GET: Administrador/Editar
        public ActionResult Editar(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    EditarViewModel editVM = new EditarViewModel();
                    editVM.administrador = administradorBL.obtener(id);
                    editVM.completarEditarVM();
                    //editVM.administrador.Password = "validacion";//Es colo para validar el modelo
                    return View(editVM);
                }
                else {
                    return RedirectToAction("Crear", "Administrador");
                }
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //POST: Administrador/Editar
        [HttpPost]
        public ActionResult Editar(EditarViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Le coloco el nombre con cual voy a guardar el archivo  
                    //Para no guardar el archivo por si da problemas al ingresar los datos     
                    editVM.completarAdministrador();
                    bool r = true;

                    r = administradorBL.actualizar(editVM.administrador);

                    if (!r)
                    {
                        // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                        ViewBag.Mensaje = "Ocurrio un error inesperado";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }
                    return RedirectToAction("ListaAdministradores");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }
            else {
                return View(editVM);
            }
        }

        //GET: Administrador/Eliminar
        public ActionResult Eliminar(int id)
        {
            try
            {
                var r = administradorBL.eliminar(id);

                if (!r)
                {
                    // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                    ViewBag.Mensaje = "Ocurrio un error inesperado";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                return Redirect("~/Administrador/ListaAdministradores");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //GET: Administrador/CambiarPass
        public ActionResult CambiarPass()
        {
            try
            {
                return View(new CambiarPassViewModel());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //POST: Administrador/CambiarPass
        [HttpPost]
        public ActionResult CambiarPass(CambiarPassViewModel cambiarPassVM)
        {
            try
            {
                if (cambiarPassVM.PasswordNuevo.Equals(cambiarPassVM.PasswordConfirmacion))
                {
                    Administrador admin = administradorBL.login(cambiarPassVM.NombreUsuario, cambiarPassVM.PasswordActual);
                    if (admin != null)
                    {
                        admin.Password = cambiarPassVM.PasswordNuevo;
                        administradorBL.actualizarPassword(admin);
                        return RedirectToAction("Index", "Home");
                    }
                }
                cambiarPassVM.Mensaje = "Las contraseñas no coinciden. Por favor, inténtelo otra vez.";
                return View(cambiarPassVM);
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //public ActionResult Guardar(Administrador admin)
        //{
        //    try {
        //        bool r = true;
        //        if (admin.Id > 0)
        //        {
        //            r = administradorBL.actualizar(admin);
        //        }
        //        else {
        //            administradorBL.registrar(admin);
        //        }
        //        if (!r)
        //        {
        //            // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
        //            ViewBag.Mensaje = "Ocurrio un error inesperado";
        //            return View("~/Views/Shared/_Mensajes.cshtml");
        //        }

        //        return Redirect("~/");
        //    }
        //    catch (ProyectoException ex)
        //    {
        //        ViewBag.Mensaje = ex.Message;
        //        return View("~/Views/Shared/_Mensajes.cshtml");
        //    }
        //}
    }
}