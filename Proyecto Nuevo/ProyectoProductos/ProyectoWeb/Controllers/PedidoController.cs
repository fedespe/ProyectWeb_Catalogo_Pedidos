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
            //Hay que controlar que solo puede acceder el admin
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
    }
}