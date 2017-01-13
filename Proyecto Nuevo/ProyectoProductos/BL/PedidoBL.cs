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

        public List<Pedido> obtenerTodosSinContarEnConstruccion()
        {
            return pedidoDAL.obtenerTodosSinContarEnConstruccion();
        }

        public Pedido obtener(int id)
        {
            return pedidoDAL.obtener(id);
        }

        public bool actualizar(Pedido ped)
        {
            return pedidoDAL.actualizar(ped);
        }

        public int registrar(Pedido ped, int idUsuario, string tipoUsuario)
        {
            int idPedidoGenerado = pedidoDAL.registrar(ped);

            if (tipoUsuario.Equals("Administrador"))
            {
                AdministradorBL administradorBL = new AdministradorBL();
                Administrador a = administradorBL.obtener(idUsuario);
                administradorBL.registrarPedidoEnConstruccion(a, idPedidoGenerado);
            }
            else if (tipoUsuario.Equals("Cliente"))
            {
                ClienteBL clienteBL = new ClienteBL();
                Cliente c = clienteBL.obtener(idUsuario);
                clienteBL.registrarPedidoEnConstruccion(c, idPedidoGenerado);
            }

            return idPedidoGenerado;
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
