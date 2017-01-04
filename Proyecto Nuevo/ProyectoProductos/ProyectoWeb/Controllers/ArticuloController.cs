using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;
using ProyectoWeb.ViewModel.ArticuloViewModel;

namespace ProyectoWeb.Controllers
{
    public class ArticuloController : Controller
    {
        private ArticuloBL articuloBL = new ArticuloBL();

        //GET: Articulo/ListaArticulo
        public ActionResult ListaArticulos()
        {
            try
            {
                return View(new ListarArticuloViewModel());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }
        //POST: Articulo/ListaArticulo
        [HttpPost]
        public ActionResult ListaArticulos(ListarArticuloViewModel ListarVM)
        {
            try
            {
                ListarVM.cargarArticulosFiltrados();
                return View(ListarVM);
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //GET: Articulo/Crear
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
        //POST: Articulo/Crear
        [HttpPost]
        public ActionResult Crear(CrearViewModel crearVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Le coloco los nombres con cual voy a guardar los archivo  
                    //Para no guardar el archivo por si da problemas al ingresar los datos     
                    crearVM.completarArticulo();
                    articuloBL.registrar(crearVM.Articulo);
                    //Guardo archivo
                    crearVM.guardarArchivo();
                    return RedirectToAction("ListaArticulos");
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

        //GET: Articulo/Editar
        public ActionResult Editar(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    EditarViewModel editVM = new EditarViewModel();
                    editVM.Articulo = articuloBL.obtener(id);
                    editVM.completarEditarVM();//Es para manejo de archivo a la hora de guardar
                    //editVM.cliente.Password = "validacion";//Es colo para validar el modelo
                    return View(editVM);
                }
                else {
                    return RedirectToAction("Crear", "Articulo");
                }
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
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
                    //Le coloco el nombre con cual voy a guardar el archivo  
                    //Para no guardar el archivo por si da problemas al ingresar los datos     
                    editVM.completarArticulo();
                    bool r = true;

                    r = articuloBL.actualizar(editVM.Articulo);

                    if (!r)
                    {
                        // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                        ViewBag.Mensaje = "Ocurrio un error inesperado";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }
                    editVM.guardarArchivo();
                    return RedirectToAction("ListaArticulos");
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
                //por si hay errores, cargo las imagenes que ya tenia en la base
                editVM.cargarImagenesSeleccionas();
                return View(editVM);
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

                return RedirectToAction("ListaArticulos");
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

                return RedirectToAction("ListaArticulos");
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

                return RedirectToAction("ListaArticulos");
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

                return RedirectToAction("ListaArticulos");
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

    }
}