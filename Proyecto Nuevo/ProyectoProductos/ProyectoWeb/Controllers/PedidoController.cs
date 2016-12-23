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

        //GET: Pedido/Detalles
        public ActionResult Detalles(int id = 0)
        {
            if (Session["TipoUsuario"] != null)
            {
                try
                {
                    if (id != 0)
                    {
                        Pedido ped = pedidoBL.obtener(id);

                        if(Session["TipoUsuario"].ToString().Equals("Administrador") || (Session["TipoUsuario"].ToString().Equals("Cliente") && (Session["NombreUsuario"].ToString().Equals(ped.Cliente.NombreUsuario))))
                            return View(ped);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                    else {
                        return RedirectToAction("Index", "Home");
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