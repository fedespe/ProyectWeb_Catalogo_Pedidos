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
        private ClienteBL clienteBL = new ClienteBL();
        private AdministradorBL administradorBL = new AdministradorBL();
        private ArticuloBL articuloBL = new ArticuloBL();
        private ParametroBL parametroBL = new ParametroBL();

        //LOGICA REVISADA 12/01/17
        //GET Pedido/Create
        public ActionResult Create(int id = 0, int cantidad = 0)
        {
            //Si no está logueado, le doy aviso de que no tiene permisos
            if(Session["TipoUsuario"] == null)
            {
                ViewBag.Mensaje = "No tiene permisos para realizar esta acción.";
                return View("~/Views/Shared/_Mensajes.cshtml");
            }

            //Si no hace llegar un artículo o una cantidad, le doy aviso de que debe indicarlos
            if (id <= 0 || cantidad <= 0)
            {
                ViewBag.Mensaje = "Debe indicar el artículo y una cantidad mayor a 0.";
                return View("~/Views/Shared/_Mensajes.cshtml"); //Hay que ver cómo hacer para quedarse en el mismo lugar en el que está, no moverlo de página...
            }

            //Si no existe un artículo con el ID que llega, doy aviso, si existe, ya me queda guardado
            Articulo a = articuloBL.obtener(id);
            if (a == null)
            {
                ViewBag.Mensaje = "No se encontró un Artículo con el identificador especificado.";
                return View("~/Views/Shared/_Mensajes.cshtml"); //Hay que ver cómo hacer para quedarse en el mismo lugar en el que está, no moverlo de página...
            }

            //Si el Usuario logueado tiene un pedido en construcción, me quedo con el ID del mismo, sino me queda en 0
            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);
            int idEnConstruccion = 0;
            if (Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                idEnConstruccion = administradorBL.obtenerPedidoEnContruccion(idUsuario);
            }
            else if (Session["TipoUsuario"].ToString().Equals("Cliente"))
            {
                idEnConstruccion = clienteBL.obtenerPedidoEnContruccion(idUsuario);
            }

            Pedido pedidoEnConstruccion = null;

            ArticuloCantidad ac = new ArticuloCantidad
            {
                Articulo = a,
                Cantidad = cantidad,
                PrecioUnitario = a.Precio
            };

            if (idEnConstruccion > 0)
            {
                //Si el ID del pedido en construcción es distinto de 0, me lo guardo.
                pedidoEnConstruccion = pedidoBL.obtener(idEnConstruccion);

                //Si pedidoEnConstruccion == null, debo obtener el pedidoEnConstruccion pero con otra consulta, sin artículos

                //Si en el Pedido en construcción ya se encuentra un ArticuloCantidad con el artículo seleccionado, se avisa que se debe modificar el pedido en construcción (Carrito)
                //if (pedidoEnConstruccion.ProductosPedidos!=null && pedidoEnConstruccion.ProductosPedidos.Contains(ac))
                //{
                //    ViewBag.Mensaje = "En su carrito ya se encuentra el artículo seleccionado, modifique el mismo.";
                //    return View("~/Views/Shared/_Mensajes.cshtml"); //Hay que ver cómo hacer para quedarse en el mismo lugar en el que está, no moverlo de página...
                //}

                if (pedidoEnConstruccion.ProductosPedidos == null)
                {
                    pedidoEnConstruccion.ProductosPedidos = new List<ArticuloCantidad>();
                }

                pedidoEnConstruccion.ProductosPedidos.Add(ac);

                pedidoBL.actualizar(pedidoEnConstruccion);
            }
            else
            {
                Cliente c = null;
                EstadoPedido ep = estadoPedidoBL.obtener("EN CONSTRUCCION");
                double iva = parametroBL.obtenerIVA();

                if (Session["TipoUsuario"].ToString().Equals("Administrador"))
                {
                    int idPrimerCliente = clienteBL.obtenerPrimerClienteHabilitado();
                    if(idPrimerCliente == 0)
                    {
                        ViewBag.Mensaje = "Debe existir al menos un cliente registrado y habilitado para poder construir un pedido.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }

                    c = clienteBL.obtener(idPrimerCliente);
                }
                else if (Session["TipoUsuario"].ToString().Equals("Cliente"))
                {
                    c = clienteBL.obtener(Convert.ToInt32(Session["IdUsuario"]));
                }

                pedidoEnConstruccion = new Pedido
                {
                    FechaRealizado = new DateTime(),
                    FechaEntregaSolicitada = new DateTime(),
                    ProductosPedidos = new List<ArticuloCantidad>(),
                    Comentario = "",
                    Estado = ep,
                    Iva = iva,
                    Cliente = c
                };

                pedidoEnConstruccion.ProductosPedidos.Add(ac);

                pedidoEnConstruccion.Id = pedidoBL.registrar(pedidoEnConstruccion, idUsuario, Session["TipoUsuario"].ToString());
            }

            Session["IdPedidoEnConstruccion"] = pedidoEnConstruccion.Id;

            return RedirectToAction("Editar", new { id = pedidoEnConstruccion.Id }); //Hay que ver cómo hacer para quedarse en el mismo lugar en el que está, no moverlo de página...
        }

        //LOGICA REVISADA 12/01/17
        //GET: Pedido/SinConfirmar
        public ActionResult SinConfirmar()
        {
            if (Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    List<Pedido> pedidos = pedidoBL.obtenerSinConfirmar();

                    if (pedidos.Count > 0)
                    {
                        return View(pedidos);
                    }
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

        //LOGICA REVISADA 12/01/17
        //GET: Pedido/Historico
        public ActionResult Historico()
        {
            if (Session["TipoUsuario"].ToString().Equals("Administrador"))
            {
                try
                {
                    List<Pedido> pedidos = pedidoBL.obtenerTodosSinContarEnConstruccion();

                    if (pedidos.Count > 0)
                        return View(pedidos);
                    else
                    {
                        ViewBag.Mensaje = "No existen pedidos.";
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

        //LOGICA REVISADA 12/01/17
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

                        if(ped!= null)
                        {
                            if (Session["TipoUsuario"].ToString().Equals("Administrador") || (Session["TipoUsuario"].ToString().Equals("Cliente") && (Session["NombreUsuario"].ToString().Equals(ped.Cliente.NombreUsuario))))
                            {
                                if (!ped.Estado.Nombre.Equals("EN CONSTRUCCION"))
                                {
                                    return View(ped);
                                }
                                else
                                {
                                    ViewBag.Mensaje = "Unicamente puede ver un pedido en construcción propio, seleccione la opción Mi Carrito";
                                    return View("~/Views/Shared/_Mensajes.cshtml");
                                }
                            }
                            else
                            {
                                ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                                return View("~/Views/Shared/_Mensajes.cshtml");
                            }
                        }
                        else
                        {
                            ViewBag.Mensaje = "No existe el pedido indicado.";
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

        //LOGICA REVISADA 12/01/17
        //GET: Pedido/ListarPorCliente
        public ActionResult ListarPorCliente(int id)
        {
            if (Session["TipoUsuario"].ToString().Equals("Administrador") || (Session["TipoUsuario"].ToString().Equals("Cliente") && Convert.ToInt32(Session["IdUsuario"]) == id))
            {
                try
                {
                    List<Pedido> lista = pedidoBL.obtenerPorClienteSinContarEnConstruccion(id);
                    if (lista.Count!=0)
                    {
                        return View(lista);
                    }
                    else {
                        ViewBag.Mensaje = "No tiene pedidos asociados.";
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

        //LOGICA REVISADA 12/01/17
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

        //LOGICA REVISADA 12/01/17
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

                            if(p.Estado.Nombre.Equals("REALIZADO") || p.Estado.Nombre.Equals("CANCELADO"))
                            {
                                ViewBag.Mensaje = "El pedido no se encuentra en un estado que permita modificarlo.";
                                return View("~/Views/Shared/_Mensajes.cshtml");
                            }

                            if (p.Estado.Nombre.Equals("EN CONSTRUCCION"))
                            {
                                int pedidoEnConstruccion = 0;

                                if (Session["TipoUsuario"].ToString().Equals("Administrador"))
                                {
                                    pedidoEnConstruccion = administradorBL.obtenerPedidoEnContruccion(Convert.ToInt32(Session["IdUsuario"]));
                                }
                                else if (Session["TipoUsuario"].ToString().Equals("Cliente"))
                                {
                                    pedidoEnConstruccion = clienteBL.obtenerPedidoEnContruccion(Convert.ToInt32(Session["IdUsuario"]));
                                }

                                if(pedidoEnConstruccion != id)
                                {
                                    ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                                    return View("~/Views/Shared/_Mensajes.cshtml");
                                }
                            }

                            if (p.Estado.Nombre.Equals("CONFIRMADO POR EL CLIENTE") && Session["TipoUsuario"].ToString().Equals("Cliente"))
                            {
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

        //LOGICA REVISADA 12/01/17
        //POST: Pedido/Editar
        [HttpPost]
        public ActionResult Editar(EditarViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    editVM.completarPedido(Session["TipoUsuario"].ToString());

                    if (Session["TipoUsuario"].ToString().Equals("Cliente") && editVM.Pedido.Cliente.Id != Convert.ToInt32(Session["IdUsuario"]))
                    {
                        ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }

                    if (editVM.Pedido.Estado.Nombre.Equals("REALIZADO") || editVM.Pedido.Estado.Nombre.Equals("CANCELADO") || (Session["TipoUsuario"].ToString().Equals("Cliente") && editVM.Pedido.Estado.Nombre.Equals("CONFIRMADO POR CLIENTE")))
                    {
                        ViewBag.Mensaje = "El pedido no se encuentra en un estado que permita modificarlo.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }

                    if (editVM.Pedido.Estado.Nombre.Equals("EN CONSTRUCCION"))
                    {
                        if (editVM.Pedido.Id == Convert.ToInt32(Session["IdPedidoEnConstruccion"]))
                        {
                            if (editVM.RealizarPedido)
                            {
                                if (Session["TipoUsuario"].ToString().Equals("Administrador"))
                                {
                                    editVM.Pedido.Estado = estadoPedidoBL.obtener("MODIFICADO POR ADMINISTRADOR");
                                    editVM.Pedido.FechaRealizado = DateTime.Today;
                                    Session["IdPedidoEnConstruccion"] = 0;
                                }
                                else if (Session["TipoUsuario"].ToString().Equals("Cliente"))
                                {
                                    editVM.Pedido.Estado = estadoPedidoBL.obtener("CONFIRMADO POR CLIENTE");
                                    editVM.Pedido.FechaRealizado = DateTime.Today;
                                    Session["IdPedidoEnConstruccion"] = 0;
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                            return View("~/Views/Shared/_Mensajes.cshtml");
                        }
                    }
                    else
                    {
                        if (Session["TipoUsuario"].ToString().Equals("Administrador"))
                        {
                            editVM.Pedido.Estado = estadoPedidoBL.obtener("MODIFICADO POR ADMINISTRADOR");
                        }
                        else if (Session["TipoUsuario"].ToString().Equals("Cliente"))
                        {
                            editVM.Pedido.Estado = estadoPedidoBL.obtener("CONFIRMADO POR CLIENTE");
                        }
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
                        if (Session["TipoUsuario"].ToString().Equals("Administrador"))
                        {
                            Session["PedidosSinConfirmar"] = pedidoBL.obtenerCantidadSinConfirmar();
                        }

                        if (editVM.RealizarPedido)
                        {
                            if (Session["TipoUsuario"].ToString().Equals("Administrador"))
                            {
                                Administrador a = administradorBL.obtener(Convert.ToInt32(Session["IdUsuario"]));
                                administradorBL.registrarPedidoEnConstruccion(a, 0);
                            }
                            else if (Session["TipoUsuario"].ToString().Equals("Cliente"))
                            {
                                clienteBL.registrarPedidoEnConstruccion(editVM.Pedido.Cliente, 0);
                            }
                        }
                            
                        editVM.completarEditarVM();
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

        //LOGICA REVISADA 12/01/17
        //GET: Pedido/Cancelar
        public ActionResult Cancelar(int id)
        {
            if (id != 0)
            {
                if (Session["TipoUsuario"] == null)
                {
                    ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                    return View("~/Views/Shared/_Mensajes.cshtml");
                }

                Pedido p = pedidoBL.obtener(id);

                if (p != null)
                {
                    if (Session["TipoUsuario"].ToString().Equals("Cliente"))
                    {
                        if(p.Cliente.Id != Convert.ToInt32(Session["IdUsuario"]))
                        {
                            ViewBag.Mensaje = "No tiene permisos para relalizar esta acción.";
                            return View("~/Views/Shared/_Mensajes.cshtml");
                        }
                    }

                    if (p.Estado.Nombre.Equals("CANCELADO") || p.Estado.Nombre.Equals("REALIZADO") || p.Estado.Nombre.Equals("EN CONSTRUCCION"))
                    {
                        ViewBag.Mensaje = "El pedido no se encuetra en un estado que permita cancelarlo.";
                        return View("~/Views/Shared/_Mensajes.cshtml");
                    }

                    pedidoBL.cancelar(p.Id);
                    Session["PedidosSinConfirmar"] = pedidoBL.obtenerCantidadSinConfirmar();

                    return RedirectToAction("Detalles", new { id = p.Id});
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
    }
}