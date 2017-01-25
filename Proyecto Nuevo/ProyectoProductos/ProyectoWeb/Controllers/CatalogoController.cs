using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;
using ProyectoWeb.ViewModel.CatalogoViewModel;

namespace ProyectoWeb.Controllers
{
    public class CatalogoController : Controller
    {
        private ArticuloBL articuloBL = new ArticuloBL();
        private CategoriaBL categoriaBL = new CategoriaBL();

        //GET: Articulo/ListaArticulo
        public ActionResult ListaArticulos(int Id=0, string Nombre="")
        {
            //int idCat = 2;
            try
            {
                if (Id != 0 && Nombre!="")
                {
                    ListarArticuloViewModel listaVM = new ListarArticuloViewModel();
                    listaVM.completar(Id, Nombre);              
                    return View(listaVM);
                }
                else {
                    return RedirectToAction("ListaCategorias");
                }

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

        //GET: Articulo/ListaCategorias
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

        //GET: Catalogo/DetalleArticulo
        public ActionResult DetalleArticulo(int Id = 0)
        {
            //int idCat = 2;
            try
            {
                if (Id != 0)
                {
                    Articulo a = articuloBL.obtener(Id);
                    if(a != null)
                    {
                        return View(a);
                    }
                    else
                    {
                        ViewBag.Mensaje = "No existe el artículo especificado.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }
                }
                else {
                    return RedirectToAction("ListaCategorias");
                }

            }
            catch (ProyectoException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        public ActionResult Buscar(string codigo = "")
        {
            try
            {
                if (codigo != "")
                {
                    Articulo articulo = articuloBL.obtenerPorCodigo(codigo);
                    if (articulo != null)
                    {
                        return View("DetalleArticulo", articulo);
                    }
                    else {
                        ViewBag.Mensaje = "No existe ningun articulo con dicho código.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }
                    
                }
                else {
                    ViewBag.Mensaje = "No existe ningun articulo con dicho código.";
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