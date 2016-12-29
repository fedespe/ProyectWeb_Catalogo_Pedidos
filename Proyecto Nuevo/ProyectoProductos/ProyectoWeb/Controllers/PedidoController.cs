using BL;
using ET;
using ProyectoWeb.ViewModel.PedidoViewModel;
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
        private EstadoPedidoBL estadoPedidoBL = new EstadoPedidoBL();

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

        //GET: Pedido/Editar
        public ActionResult Editar(int id = 0)
        {
            if(Session["TipoUsuario"] != null)
            {
                try
                {
                    if (id != 0)
                    {
                        Pedido p = pedidoBL.obtener(id);
                        if(p != null)
                        {
                            if(Session["TipoUsuario"].ToString().Equals("Cliente") && Convert.ToInt32(Session["IdUsuario"]) != p.Cliente.Id) {
                                ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                                return View("~/Views/Shared/_Mensajes.cshtml");
                            }
                            EditarViewModel editVM = new EditarViewModel();
                            editVM.Pedido = p;
                            editVM.completarEditarVM();
                            return View(editVM);
                        }
                        else
                        {
                            ViewBag.Mensaje = "No se encontró el pedido que desea visualizar.";
                            return View("~/Views/Shared/_Mensajes.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.Mensaje = "Debe indicar qué pedido desea visualizar.";
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
                ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                return View("~/Views/Shared/_Mensajes.cshtml");
            }
        }

        //POST: Pedido/Editar
        [HttpPost]
        public ActionResult Editar(EditarViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session["TipoUsuario"].ToString().Equals("Cliente") && editVM.Pedido.Cliente.Id != Convert.ToInt32(Session["IdUsuario"]))
                    {
                        ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }

                    editVM.completarPedido();

                    if (Session["TipoUsuario"].ToString().Equals("Administrador"))
                    {
                        editVM.Pedido.Estado = estadoPedidoBL.obtener("MODIFICADO POR ADMINISTRADOR");
                    }

                    if (Session["TipoUsuario"].ToString().Equals("Cliente"))
                    {
                        editVM.Pedido.Estado = estadoPedidoBL.obtener("MODIFICADO POR CLIENTE");
                    }

                    bool r = pedidoBL.actualizar(editVM.Pedido);

                    if (!r)
                    {
                        // Podemos validar para mostrar un mensaje personalizado, por ahora el aplicativo se caera por el throw que hay en nuestra capa DAL
                        ViewBag.Mensaje = "Ocurrio un error inesperado";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }
                    else
                    {
                        return View(editVM);
                    }
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
    }
}