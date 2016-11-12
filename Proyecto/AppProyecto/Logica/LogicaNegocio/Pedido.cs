using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Pedido : IEntity
    {
        public int Id { get; set; }
        public DateTime FechaRealizado { get; set; }
        public DateTime FechaEntregaSolicitada { get; set; }
        public double PrecioTotal { get; set; }
        public double DescuentoCliente { get; set; }
        public double Iva { get; set; }
        public Usuario Cliente { get; set; }
        public List<ArticuloCantidad> ProductosPedidos { get; set; }
        public string Comentario { get; set; }
        public EstadoPedido Estado { get; set; }
    }
}
