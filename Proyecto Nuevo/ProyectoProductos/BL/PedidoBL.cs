using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PedidoBL
    {
        private PedidoDAL pedidoDAL = new PedidoDAL();

        public List<Pedido> obtenerTodos()
        {
            return pedidoDAL.obtenerTodos();
        }

        public Pedido obtener(int id)
        {
            return pedidoDAL.obtener(id);
        }

        public bool actualizar(Pedido ped)
        {
            return pedidoDAL.actualizar(ped);
        }

        public void registrar(Pedido ped)
        {
            pedidoDAL.registrar(ped);
        }

        public bool eliminar(int id)
        {
            return pedidoDAL.eliminar(id);
        }
        public int obtenerCantidadSinConfirmar()
        {
            return pedidoDAL.obtenerCantidadSinConfirmar();
        }

        public List<Pedido> obtenerSinConfirmar()
        {
            return pedidoDAL.obtenerSinConfirmar();
        }

        public List<Pedido> obtenerPorCliente(int id)
        {
            return pedidoDAL.obtenerPorCliente(id);
        }

        public void confirmar(int id)
        {
            pedidoDAL.confirmar(id);
        }

        public void setearTotal(Pedido p)
        {
            double total = 0;

            foreach (ArticuloCantidad ac in p.ProductosPedidos)
            {
                total += ac.Cantidad * ac.PrecioUnitario;
            }

            if (p.DescuentoCliente > 0 && total > 0)
            {
                total -= total * p.DescuentoCliente / 100;
            }

            p.PrecioTotal = total;
        }
    }
}
