using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWeb.ViewModel.PedidoViewModel
{
    public class EditarViewModel
    {
        private ParametroBL parametroBL = new ParametroBL();
        private ClienteBL clienteBL = new ClienteBL();
        private EstadoPedidoBL estadoPedidoBL = new EstadoPedidoBL();
        //private ArticuloBL articuloBL = new ArticuloBL();

        public ET.Pedido Pedido { get; set; }
        public ET.Cliente Cliente { get; set; }
        public ET.EstadoPedido Estado { get; set; }

        [Required]
        [Display(Name = "Fecha Realizado")]
        public DateTime FechaRealizado { get; set; }

        [Required]
        [Display(Name = "Fecha Entrega Solicitada")]
        public DateTime FechaEntregaSolicitada { get; set; }

        [Required]
        [Display(Name = "Precio Total")]
        public double PrecioTotal { get; set; }

        [Required]
        [Display(Name = "Descuento Cliente Preferencial")]
        [Range(0, 100)]
        public double DescuentoCliente { get; set; }

        [Required]
        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Display(Name = "Comentario")]
        public string Comentario { get; set; }


        [Required]
        [Display(Name = "IdCLiente")]
        public int IdCLiente { get; set; }

        [Display(Name = "TipoUsuarioEditor")]
        public string TipoUsuarioEditor { get; set; }

        [Display(Name = "Estado")]
        public string EstadoPedido { get; set; }

        public List<ET.ArticuloCantidad> ProductosPedidos { get; set; }

        public string CadenaArticulos { get; set; }


        public EditarViewModel(string estadoPedido)
        {
            ProductosPedidos = new List<ET.ArticuloCantidad>();
            EstadoPedido = estadoPedido;

        }

        public void completarEditarVM()
        {
            FechaRealizado = Pedido.FechaRealizado;
            FechaEntregaSolicitada = Pedido.FechaEntregaSolicitada;
            PrecioTotal = Pedido.PrecioTotal;
            DescuentoCliente = Pedido.Cliente.Descuento;
            Iva = parametroBL.obtenerIVA();
            Comentario = Pedido.Comentario;
        }

        public void completarPedido()
        {
            cargarCLiente();
            cargarEstado();
            Pedido.Comentario = Comentario;
            Pedido.DescuentoCliente = Cliente.Descuento;
            Pedido.FechaEntregaSolicitada = FechaEntregaSolicitada;
            Pedido.FechaRealizado = FechaRealizado;
            Pedido.Iva = Iva;
            Pedido.PrecioTotal = PrecioTotal;
            cargarProductosPedidos();
        }

        private void cargarCLiente()
        {
            Cliente = clienteBL.obtener(IdCLiente);
        }
        private void cargarEstado()
        {
            string estado = EstadoPedido;

            if (EstadoPedido.Equals(""))
            {
                estado = "NUEVO";
            }
            Estado = estadoPedidoBL.obtener(estado);
        }

        private void cargarProductosPedidos()
        {
            if (CadenaArticulos != null)
            {
                CadenaArticulos = CadenaArticulos.Trim();
                char c1 = ' ';
                char c2 = ';';
                string[] substrings = CadenaArticulos.Split(c1);
                for (int i = 0; i < substrings.Length; i++)
                {
                    //string[] substrings2 = substrings[i].Split(c2);
                    //ArticuloBL p = new Categoria { Id = Convert.ToInt32(substrings2[0]) };
                    //if (substrings2[1] == "true")
                    //{
                    //    Articulo.Categorias.Remove(c);
                    //    Articulo.Categorias.Add(c);
                    //}
                    //else {
                    //    Articulo.Categorias.Remove(c);
                    //}
                }
            }
        }
    }
}
