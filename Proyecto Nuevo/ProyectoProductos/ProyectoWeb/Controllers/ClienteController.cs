﻿using BL;
using ET;
using ProyectoWeb.ViewModel.ClienteViewModel;
using ProyectoWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class ClienteController : Controller
    {
        private ClienteBL clienteBL = new ClienteBL();

        //GET: Cliente/ListaClientes
        public ActionResult ListaClientes()
        {
            //Hay que controlar que solo puede acceder el admin
            try
            {
                return View(clienteBL.obtenerTodos());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //GET: Cliente/Crear
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
                    //Falta encriptar password
                    //Le coloco el nombre con cual voy a guardar el archivo  
                    //Para no guardar el archivo por si da problemas al ingresar los datos     
                    crearVM.completarCliente();
                    clienteBL.registrar(crearVM.cliente);
                    //Guardo archivo
                    crearVM.guardarArchivo();
                    return Redirect("~/");
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

        //GET: Cliente/Editar
        public ActionResult Editar(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    EditarViewModel editVM = new EditarViewModel();
                    editVM.cliente = clienteBL.obtener(id);
                    editVM.completarEditarVM();//Es para manejo de archivo a la hora de guardar
                    //editVM.cliente.Password = "validacion";//Es colo para validar el modelo
                    return View(editVM);
                }
                else {
                    return RedirectToAction("Crear", "Cliente");
                }
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //POST: Cliente/Editar
        [HttpPost]
        public ActionResult Editar(EditarViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Le coloco el nombre con cual voy a guardar el archivo  
                    //Para no guardar el archivo por si da problemas al ingresar los datos     
                    editVM.completarCliente();
                    bool r = true;

                    r = clienteBL.actualizar(editVM.cliente);

                    if (!r)
                    {
                        // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                        ViewBag.Mensaje = "Ocurrio un error inesperado";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }
                    editVM.guardarArchivo();
                    return Redirect("~/");
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

        

        public ActionResult Eliminar(int id)
        {
            try {
                //para poder borrar la imagen
                EditarViewModel editVM = new EditarViewModel();
                editVM.cliente = clienteBL.obtener(id);

                bool r = clienteBL.eliminar(id);

                if (r) {
                    editVM.eliminarArchivo();
                }else
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

        //GET: Cliente/LogIn
        public ActionResult Login()
        {
            try
            {
                return View(new Cliente());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //POST: Usuario/LogIn
        [HttpPost]
        public ActionResult Login(Cliente cliente)
        {
            try
            {
                //Falta codificacion de password
                Cliente cli= clienteBL.login(cliente.NombreUsuario,cliente.Password);
                if (cli != null)
                {
                        Session["UsuarioId"] = cli.Id;
                        Session["UsuarioNombre"] = cli.NombreUsuario;
                        return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Login", "Cliente");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }


        //public ActionResult Index()
        //{
        //    try
        //    {
        //        return View(clienteBL.obtenerTodos());
        //    }
        //    catch (ProyectoException ex)
        //    {
        //        ViewBag.Mensaje = ex.Message;
        //        return View("~/Views/Shared/_Mensajes.cshtml");
        //    }
        //}
        //GET: Cliente/Crear
        //public ActionResult Editar(int id = 0)
        //{
        //    try
        //    {
        //        if (id != 0)
        //        {
        //            ClienteViewModel cliVM = new ClienteViewModel();
        //            cliVM.cliente = clienteBL.obtener(id);
        //            return View(cliVM);
        //        }
        //        else {
        //            return RedirectToAction("Crear", "Cliente");
        //        }
        //    }
        //    catch (ProyectoException ex)
        //    {
        //        ViewBag.Mensaje = ex.Message;
        //        return View("~/Views/Shared/_Mensajes.cshtml");
        //    }
        //}
        //POST: Cliente/Guardar
        //Se utiliza para la creacion como para la edicion
        //public ActionResult Guardar(ClienteViewModel cliVM)
        //{
        //    try
        //    {
        //        //Le coloco el nombre con cual voy a guardar el archivo  
        //        //Para no guardar el archivo por si da problemas al ingresar los datos     
        //        cliVM.colocarRuta();
        //        bool r = true;
        //        if (cliVM.cliente.Id > 0)
        //        {
        //            r = clienteBL.actualizar(cliVM.cliente);
        //        }
        //        else {
        //            clienteBL.registrar(cliVM.cliente);
        //        }
        //        if (!r)
        //        {
        //            // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
        //            ViewBag.Mensaje = "Ocurrio un error inesperado";
        //            return View("~/Views/Shared/_Mensajes.cshtml");
        //        }
        //        cliVM.mapear();
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