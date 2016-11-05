using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Pedido:IEntity
    {
        public int Id { get; set; }
        public Usuario Cliente { get; set; }//quien lo realiza
        public Usuario Administrador { get; set; }//quien lo gestiona
        public List<ProductoCantidad> ProductosPedidos { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaRealizado { get; set; }
        public Estado Estado { get; set; }

        public Pedido(Usuario cliente, string comentario, DateTime fecha) {
            this.Cliente = cliente;
            this.ProductosPedidos = new List<ProductoCantidad>();
            this.Comentario = comentario;
            this.FechaPedido = fecha;
        }
    }
}
