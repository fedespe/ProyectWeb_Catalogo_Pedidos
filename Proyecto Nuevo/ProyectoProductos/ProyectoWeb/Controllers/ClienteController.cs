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
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
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

        //GET: Cliente/Crear
        public ActionResult Crear()
        {
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
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
                    //ViewBag.Mensaje = ex.Message;
                    //return View("~/Views/Shared/_Mensajes.cshtml");
                    crearVM.mensajeError = ex.Message;
                    return View(crearVM);
                }
            }
            else {
                return View(crearVM);
            }


        }

        //GET: Cliente/Editar
        public ActionResult Editar(int id = 0)
        {
            if (Session["TipoUsuario"] != null && (Session["TipoUsuario"].ToString().Equals("Administrador") || (Session["TipoUsuario"].ToString().Equals("Cliente")) && (Convert.ToInt32(Session["IdUsuario"]) == id)))
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
                    if (Session["TipoUsuario"].ToString().Equals("Administrador")) {
                        return RedirectToAction("ListaClientes");
                    }
                    else {
                        Session["NombreUsuario"] = editVM.cliente.NombreUsuario;
                        return RedirectToAction("Index","Home");
                    }
                    
                }
                catch (ProyectoException ex)
                {
                    //ViewBag.Mensaje = ex.Message;
                    //return View("~/Views/Shared/_Mensajes.cshtml");
                    editVM.mensajeError = ex.Message;
                    return View(editVM);
                }
            }
            else {
                return View(editVM);
            }
        }

        public ActionResult Eliminar(int id)
        {
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    //para poder borrar la imagen
                    EditarViewModel editVM = new EditarViewModel();
                    editVM.cliente = clienteBL.obtener(id);

                    bool r = clienteBL.eliminar(id);

                    if (r)
                    {
                        editVM.eliminarArchivo();
                    }
                    else
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

        //GET: Cliente/CambiarPass
        public ActionResult CambiarPass(int id = 0)
        {
            if (Session["TipoUsuario"] != null && ((Session["TipoUsuario"].ToString().Equals("Cliente") && Convert.ToInt32(Session["IdUsuario"]) == id) || Session["TipoUsuario"].ToString().Equals("Administrador")))
            {
                try
                {
                    CambiarPassViewModel cambiarPassVM = new CambiarPassViewModel();
                    cambiarPassVM.Cliente = clienteBL.obtener(id);                    
                    cambiarPassVM.NombreUsuario = cambiarPassVM.Cliente.NombreUsuario;
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

        //POST: Cliente/CambiarPass
        [HttpPost]
        public ActionResult CambiarPass(CambiarPassViewModel cambiarPassVM)
        {
            if (ModelState.IsValid)
            {
                if (Session["TipoUsuario"] != null && ((Session["TipoUsuario"].ToString().Equals("Cliente") && Convert.ToInt32(Session["IdUsuario"]) == cambiarPassVM.Id) || Session["TipoUsuario"].ToString().Equals("Administrador")))
                {
                    try
                    {
                        if (cambiarPassVM.PasswordNuevo.Equals(cambiarPassVM.PasswordConfirmacion))
                        {
                            Cliente cli = clienteBL.login(cambiarPassVM.NombreUsuario, cambiarPassVM.PasswordActual);
                            if (cli != null)
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
            else {
                return View(cambiarPassVM);
            }
        }

        public ActionResult Ver(int Id)
        {
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    return View(clienteBL.obtener(Id));
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

        public ActionResult Inhabilitar(int id)
        {
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    var r = clienteBL.inhabilitar(id);

                    if (!r)
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
        public ActionResult Habilitar(int id)
        {
            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    var r = clienteBL.habilitar(id);

                    if (!r)
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

        //Prueba para DDL Cliente
        public JsonResult ObtenerTodosHabilitados()
        {
            List<Cliente> model = new List<Cliente>();

            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    model = clienteBL.obtenerTodosHabilitados();
                }
                catch (ProyectoException ex)
                {
                    //En caso de que haya excepción no necesitaría hacer nada en un principio, ya que me va a retornar el model vacío
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet); //Para que es el AllowGet?
        }

        public JsonResult ObtenerTodos()
        {
            List<Cliente> model = new List<Cliente>();

            if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    model = clienteBL.obtenerTodos();
                }
                catch (ProyectoException ex)
                {
                    //En caso de que haya excepción no necesitaría hacer nada en un principio, ya que me va a retornar el model vacío
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet); //Para que es el AllowGet?
        }
    }
}