using BL;
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
                    //Le coloco el nombre con cual voy a guardar el archivo  
                    //Para no guardar el archivo por si da problemas al ingresar los datos     
                    crearVM.completarCliente();
                    clienteBL.registrar(crearVM.cliente);
                    //Guardo archivo
                    crearVM.guardarArchivo();
                    return RedirectToAction("ListaClientes");
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
                    return RedirectToAction("ListaClientes");
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

                return RedirectToAction("ListaClientes");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //GET: Cliente/CambiarPass
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

        //POST: Cliente/CambiarPass
        [HttpPost]
        public ActionResult CambiarPass(CambiarPassViewModel cambiarPassVM)
        {
            try
            {
                if (cambiarPassVM.PasswordNuevo.Equals(cambiarPassVM.PasswordConfirmacion)) {
                    Cliente cli = clienteBL.login(cambiarPassVM.NombreUsuario, cambiarPassVM.PasswordActual);
                    if (cli!=null)
                    {
                        cli.Password = cambiarPassVM.PasswordNuevo;
                        clienteBL.actualizarPassword(cli);
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                cambiarPassVM.Mensaje = "Datos erróneos. Por favor, inténtelo otra vez.";
                return View(cambiarPassVM);
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

    }
}