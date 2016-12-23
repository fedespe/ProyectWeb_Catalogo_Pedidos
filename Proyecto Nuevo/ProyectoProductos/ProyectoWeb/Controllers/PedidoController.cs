using BL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class PedidoController : Controller
    {
        private PedidoBL pedidoBL = new PedidoBL();

        //GET: Pedido/SinConfirmar
        public ActionResult SinConfirmar()
        {
            if (Session["TipoUsuario"].ToString() == "Administrador")
            {
                try
                {
                    return View(pedidoBL.obtenerSinConfirmar());
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

        //GET: Cliente/Editar
        //public ActionResult Editar(int id = 0)
        //{
        //    if (Session["TipoUsuario"].ToString() == "Administrador" || (Session["TipoUsuario"].ToString() == "Cliente") && (Convert.ToInt32(Session["IdUsuario"]) == id))
        //    {
        //        try
        //        {
        //            if (id != 0)
        //            {
        //                EditarViewModel editVM = new EditarViewModel();
        //                editVM.cliente = clienteBL.obtener(id);
        //                editVM.completarEditarVM();//Es para manejo de archivo a la hora de guardar
        //                                           //editVM.cliente.Password = "validacion";//Es colo para validar el modelo
        //                return View(editVM);
        //            }
        //            else {
        //                return RedirectToAction("Crear", "Cliente");
        //            }
        //        }
        //        catch (ProyectoException ex)
        //        {
        //            ViewBag.Mensaje = ex.Message;
        //            return View("~/Views/Shared/_Mensajes.cshtml");
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        catch (ProyectoException ex)
        //        {
        //            ViewBag.Mensaje = ex.Message;
        //            return View("~/Views/Shared/_Mensajes.cshtml");
        //        }
        //    }
        //}

        public ActionResult Confirmar(int id)
        {
            if (Session["TipoUsuario"].ToString() == "Administrador")
            {
                try
                {
                    pedidoBL.confirmar(id);

                    return RedirectToAction("SinConfirmar");
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
    }
}