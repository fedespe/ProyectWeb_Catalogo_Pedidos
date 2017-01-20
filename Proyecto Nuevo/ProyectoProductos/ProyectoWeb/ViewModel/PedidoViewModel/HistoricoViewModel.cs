using BL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoWeb.ViewModel.PedidoViewModel
{
    public class HistoricoViewModel
    {
        private PedidoBL pedidoBL = new PedidoBL();
        public List<Pedido> Pedidos { get; set; }
        public bool Confirmado { get; set; }
        public bool ConfirmadoPorCliente { get; set; }
        public bool ConfirmadoPorAdmin { get; set; }
        public bool Realizado { get; set; }
        public bool Cancelado { get; set; }
        public bool EnConstruccion { get; set; }

        public HistoricoViewModel()
        {
            Pedidos = pedidoBL.obtenerTodosSinContarEnConstruccion().OrderByDescending(p => p.FechaRealizado).ToList(); 
        }

        public void cargarHistoricoFiltrado() {
            Pedidos = new List<Pedido>();
            List<Pedido> PedidosTotales = pedidoBL.obtenerTodos();
            
            if (!Confirmado && !ConfirmadoPorCliente && !ConfirmadoPorAdmin && !Realizado && !Cancelado && !EnConstruccion) {
                Pedidos = pedidoBL.obtenerTodosSinContarEnConstruccion().OrderByDescending(p => p.FechaRealizado).ToList();
            }
            else {
                //ESTO SE TENDRIA QUE HACER EN LA LOGICA EN PEDIDOBL PERO POR EL MOMENTO NO ESTAN HECHAS LAS
                //CONSULTAS POR SEPARADO Y ES MAS EFICIENTE HACERLO ASI YA QUE VOY UNA VEZ SOLA A LA BASE
                if (Confirmado){
                    Pedidos.AddRange(PedidosTotales.Where(p => p.Estado.Nombre.Equals("CONFIRMADO")).OrderByDescending(p => p.FechaRealizado).ToList());
                }
                if (ConfirmadoPorAdmin) {
                    Pedidos.AddRange(PedidosTotales.Where(p => p.Estado.Nombre.Equals("MODIFICADO POR ADMINISTRADOR")).OrderByDescending(p => p.FechaRealizado).ToList());
                }
                if (ConfirmadoPorCliente)
                {
                    Pedidos.AddRange(PedidosTotales.Where(p => p.Estado.Nombre.Equals("CONFIRMADO POR CLIENTE")).OrderByDescending(p => p.FechaRealizado).ToList());
                }
                if (Realizado)
                {
                    Pedidos.AddRange(PedidosTotales.Where(p => p.Estado.Nombre.Equals("REALIZADO")).OrderByDescending(p => p.FechaRealizado).ToList());
                }
                if (Cancelado)
                {
                    Pedidos.AddRange(PedidosTotales.Where(p => p.Estado.Nombre.Equals("CANCELADO")).OrderByDescending(p => p.FechaRealizado).ToList());
                }
                if (EnConstruccion)
                {
                    Pedidos.AddRange(PedidosTotales.Where(p => p.Estado.Nombre.Equals("EN CONSTRUCCION")).OrderByDescending(p => p.FechaRealizado).ToList());
                }
            }
        }

    }
}