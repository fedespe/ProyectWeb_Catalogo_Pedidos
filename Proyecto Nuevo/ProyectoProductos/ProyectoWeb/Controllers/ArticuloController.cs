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

        public ActionResult ListaArticulos()
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
            //SOLO PARA PRUEBA
            //*********************************************
            //List<Imagen> imagenes = new List<Imagen>();
            //Imagen i = new Imagen {
            //    Img = "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"
            //};
            //imagenes.Add(i);
            //List<Filtro> filtros = new List<Filtro>();
            //Filtro f = new Filtro
            //{
            //    Id = 1,               
            //};
            //filtros.Add(f);
            //List<Categoria> categorias = new List<Categoria>();
            //Categoria c = new Categoria
            //{
            //    Id = 1,
            //};
            //categorias.Add(c);
            //articulo.Imagenes = imagenes;
            //articulo.Categorias = categorias;
            //articulo.Filtros = filtros;
            //Prueba para articulo nuevo
            //articulo.Id = 0;
            //*********************************************
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