using BL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoWeb.ViewModel.CategoriaViewModel;

namespace ProyectoWeb.Controllers
{
    public class CategoriaController : Controller
    {
        private CategoriaBL categoriaBL = new CategoriaBL();

        //GET: Categoria/ListaCategorias
        public ActionResult ListaCategorias()
        {
            try
            {
                return View(categoriaBL.obtenerTodos());
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //GET: Categoria/Crear
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
        //POST: Categoria/Crear
        [HttpPost]
        public ActionResult Crear(CrearViewModel crearVM)
        {
            if (ModelState.IsValid)
            {
                try
                {     
                    crearVM.completarCategoria();
                    categoriaBL.registrar(crearVM.categoria);
                    //Guardo archivo
                    crearVM.guardarArchivo();
                    return RedirectToAction("ListaCategorias");
                }
                catch (ProyectoException ex)
                {
                    crearVM.mensajeError = ex.Message;
                    return View(crearVM);
                }
            }
            else {
                return View(crearVM);
            }
        }

        //GET: Categoria/Editar
        public ActionResult Editar(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    EditarViewModel editVM = new EditarViewModel();
                    editVM.categoria = categoriaBL.obtener(id);
                    editVM.completarEditarVM();
                    return View(editVM);
                }
                else {
                    return RedirectToAction("Crear", "Categoria");
                }
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //POST: Categoria/Editar
        [HttpPost]
        public ActionResult Editar(EditarViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                try
                {     
                    editVM.completarCliente();
                    bool r = true;

                    r = categoriaBL.actualizar(editVM.categoria);

                    if (!r)
                    {
                        // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                        ViewBag.Mensaje = "Ocurrio un error inesperado";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }
                    editVM.guardarArchivo();
                    return RedirectToAction("ListaCategorias");
                }
                catch (ProyectoException ex)
                {
                    editVM.mensajeError = ex.Message;
                    editVM.categoria = categoriaBL.obtener(editVM.categoria.Id);
                    return View(editVM);
                }
            }
            else {
                return View(editVM);
            }
        }

        public ActionResult Eliminar(int id)
        {
            try
            {
                bool r = categoriaBL.eliminar(id);
                if (r)
                {
                    return RedirectToAction("ListaCategorias");
                }
                else
                {
                    // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                    ViewBag.Mensaje = "Ocurrio un error inesperado";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }                
            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }                       
        }


    }
}