using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class EstadoPedido
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Pedido> Pedidos { get; set; }
    }
}
