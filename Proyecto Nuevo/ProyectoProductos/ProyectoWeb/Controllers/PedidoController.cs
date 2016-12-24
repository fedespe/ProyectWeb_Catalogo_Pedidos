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
            if (Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    List<Pedido> pedidos = pedidoBL.obtenerSinConfirmar();

                    if (pedidos.Count > 0)
                        return View(pedidos);
                    else
                    {
                        ViewBag.Mensaje = "No hay pedidos sin confirmar.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
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
                        {
                            if(ped!= null)
                                return View(ped);
                            else
                            {
                                ViewBag.Mensaje = "No existe el pedido indicado.";
                                return View("~/Views/Shared/_Mensajes.cshtml");
                            }
                        }
                        else
                        {
                            ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                            return View("~/Views/Shared/_Mensajes.cshtml");
                        }
                    }
                    else {
                        ViewBag.Mensaje = "Debe indicar un pedido para ver los detalles.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
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

        //GET: Pedido/ListarPorCliente
        public ActionResult ListarPorCliente(int id)
        {
            if (Session["TipoUsuario"].ToString().Equals("Administrador") || (Session["TipoUsuario"].ToString().Equals("Cliente") && Convert.ToInt32(Session["IdUsuario"]) == id))
            {
                try
                {
                    return View(pedidoBL.obtenerPorCliente(id));
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

        //GET: Pedido/Confirmar
        public ActionResult Confirmar(int id)
        {
            if (Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    pedidoBL.confirmar(id);

                    Session["PedidosSinConfirmar"] = pedidoBL.obtenerCantidadSinConfirmar();

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
    }
}