﻿using BL;
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
            if(Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString() == "Administrador")
            {
                try
                {
                    return View(administradorBL.obtenerTodos());
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
                    return RedirectToAction("Index", "Home");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }
        }

        //GET: Administrador/Crear
        public ActionResult Crear()
        {
            if(Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString() == "Administrador")
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
            else
            {
                try
                {
                    return RedirectToAction("Index", "Home");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
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
            if(Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
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
            else
            {
                try
                {
                    return RedirectToAction("Index", "Home");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
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
            if(Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
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
            else
            {
                try
                {
                    return RedirectToAction("Index", "Home");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }
        }

        //GET: Administrador/CambiarPass
        public ActionResult CambiarPass(int id = 0)
        {
            if(Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                if( id <= 0)
                {
                    ViewBag.Mensaje = "El administrador seleccionado no es válido.";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                try
                {
                    CambiarPassViewModel cambiarPassVM = new CambiarPassViewModel();
                    cambiarPassVM.Administrador = administradorBL.obtener(id);
                    cambiarPassVM.NombreUsuario = cambiarPassVM.Administrador.NombreUsuario;
                    cambiarPassVM.Id = id;
                    return View(cambiarPassVM);
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
                    ViewBag.Mensaje = "No tiene permisos para realizar esta acción.";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }
        }

        //POST: Administrador/CambiarPass
        [HttpPost]
        public ActionResult CambiarPass(CambiarPassViewModel cambiarPassVM)
        {
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (cambiarPassVM.PasswordNuevo.Equals(cambiarPassVM.PasswordConfirmacion))
                        {
                            Administrador admin = administradorBL.obtener(cambiarPassVM.Id);
                            if (admin != null)
                            {
                                admin.Password = cambiarPassVM.PasswordNuevo;
                                administradorBL.actualizarPassword(admin);
                                return RedirectToAction("ListaAdministradores");
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
                else {
                    return View(cambiarPassVM);
                }
            }
            else
            {
                try
                {
                    ViewBag.Mensaje = "No tiene permisos para realizar esta acción.";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
                catch (ProyectoException ex)
                {
                    ViewBag.Mensaje = ex.Message;
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }
            }

        }
    }
}