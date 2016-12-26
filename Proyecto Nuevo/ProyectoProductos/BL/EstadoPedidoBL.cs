using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class EstadoPedidoBL
    {
        private EstadoPedidoDAL estadoPedidoDAL = new EstadoPedidoDAL();

        public EstadoPedido obtener(string estado)
        {
            return estadoPedidoDAL.obtener(estado);
        }
    }
}
