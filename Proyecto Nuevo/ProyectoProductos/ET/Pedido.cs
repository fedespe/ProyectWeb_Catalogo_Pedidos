using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaRealizado { get; set; }
        public DateTime FechaEntregaSolicitada { get; set; }
        public double PrecioTotal { get; set; }
        public double DescuentoCliente { get; set; }
        public double Iva { get; set; }
        public Cliente Cliente { get; set; }
        public List<ArticuloCantidad> ProductosPedidos { get; set; }
        public string Comentario { get; set; }
        public EstadoPedido Estado { get; set; }

        public void setearTotal()
        {
            double total = 0;

            foreach(ArticuloCantidad ac in ProductosPedidos)
            {
                total += ac.Cantidad * ac.PrecioUnitario;
            }

            if(DescuentoCliente > 0 && total > 0)
            {
                total -= total * DescuentoCliente / 100;
            }

            PrecioTotal = total;
        }
    }
}