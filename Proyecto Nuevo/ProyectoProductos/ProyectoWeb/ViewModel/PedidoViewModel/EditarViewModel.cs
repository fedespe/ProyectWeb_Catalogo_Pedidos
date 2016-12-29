using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProyectoWeb.ViewModel.PedidoViewModel
{
    public class EditarViewModel
    {
        private ParametroBL parametroBL = new ParametroBL();
        private ClienteBL clienteBL = new ClienteBL();
        private EstadoPedidoBL estadoPedidoBL = new EstadoPedidoBL();
        private ArticuloBL articuloBL = new ArticuloBL();


        [Required]
        [Display(Name = "IdCliente")]
        public int IdCliente { get; set; }

        [Required]
        [Display(Name = "Fecha Realizado")]
        public DateTime FechaRealizado { get; set; }

        [Required]
        [Display(Name = "Fecha Entrega Solicitada")]
        public DateTime FechaEntregaSolicitada { get; set; }

        public string CadenaArticulos { get; set; }
        
        [Display(Name = "Comentario")]
        public string Comentario { get; set; }



        //Estos están para ver si son realmente necesarios
        //***************************************************************
        public ET.Pedido Pedido { get; set; }
        public ET.Cliente Cliente { get; set; }
        public ET.EstadoPedido Estado { get; set; }
        public IList<SelectListItem> Clientes { get; set; }

        [Required]
        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Display(Name = "Estado")]
        public string EstadoPedido { get; set; }

        public List<ET.ArticuloCantidad> ProductosPedidos { get; set; }
        //***************************************************************




        public EditarViewModel()
        {
            ProductosPedidos = new List<ET.ArticuloCantidad>();
            this.Clientes = clienteBL.obtenerTodos().Select(
                c => new SelectListItem()
                {
                    Text = c.NombreFantasia,
                    Value = c.Id.ToString()
                }).ToList();
        }

        public void completarEditarVM()
        {
            Pedido.setearTotal();
            FechaRealizado = Pedido.FechaRealizado;
            FechaEntregaSolicitada = Pedido.FechaEntregaSolicitada;
            Iva = parametroBL.obtenerIVA();
            Comentario = Pedido.Comentario;
            EstadoPedido = Pedido.Estado.Nombre;
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
            Pedido.setearTotal();
            cargarProductosPedidos();
        }

        private void cargarCLiente()
        {
            Cliente = clienteBL.obtener(IdCliente);
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
                    string[] substrings2 = substrings[i].Split(c2);
                    ET.Articulo a = articuloBL.obtener(Convert.ToInt32(substrings2[0]));
                    ET.ArticuloCantidad ac = new ET.ArticuloCantidad();
                    ac.Articulo = a;
                    ac.PrecioUnitario = a.Precio;
                    ac.Cantidad = Convert.ToInt32(substrings2[1]);
                    ProductosPedidos.Add(ac);
                }
            }
        }
    }
}
