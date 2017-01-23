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
        public int IdClienteFiltrado { get; set; }
        public DateTime Fecha1 { get; set; }
        public DateTime Fecha2 { get; set; }

        public HistoricoViewModel()
        {
            Pedidos = pedidoBL.obtenerTodosSinContarEnConstruccion().OrderByDescending(p => p.FechaRealizado).ToList(); 
        }

        public void cargarHistoricoFiltrado() {
            Pedidos = new List<Pedido>();
            List<Pedido> PedidosTotales = pedidoBL.obtenerTodos();
            
            if (!Confirmado && !ConfirmadoPorCliente && !ConfirmadoPorAdmin && !Realizado && !Cancelado && !EnConstruccion && !(IdClienteFiltrado > 0)) {
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
                if (IdClienteFiltrado > 0)
                {
                    if (!Confirmado && !ConfirmadoPorCliente && !ConfirmadoPorAdmin && !Realizado && !Cancelado && !EnConstruccion)
                    {
                        Pedidos = pedidoBL.obtenerTodosSinContarEnConstruccion().OrderByDescending(p => p.FechaRealizado).ToList();
                    }
                    Pedidos = Pedidos.Where(p => p.Cliente.Id == IdClienteFiltrado).OrderByDescending(p => p.FechaRealizado).ToList();
                }
            }
            //Si no coloca fecha de fin le coloco la fecha del dia
            if (Fecha2.Equals(Convert.ToDateTime("01/01/0001"))) {
                Fecha2 = DateTime.Today;
            }
            Pedidos= Pedidos.Where(f => f.FechaRealizado>=Fecha1 && f.FechaRealizado<=Fecha2).OrderByDescending(p => p.FechaRealizado).ToList();
        }

    }
}