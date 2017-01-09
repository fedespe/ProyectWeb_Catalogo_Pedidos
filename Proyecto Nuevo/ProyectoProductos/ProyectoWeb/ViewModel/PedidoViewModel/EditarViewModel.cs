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
        private PedidoBL pedidoBL = new PedidoBL();

        //[Required]
        [Display(Name = "IdPedido")]
        public int IdPedido { get; set; }

        //[Required]
        [Display(Name = "IdCliente")]
        public int IdCliente { get; set; }

        //[Required]
        [Display(Name = "Fecha Realizado")]
        public DateTime FechaRealizado { get; set; }

        //[Required]
        [Display(Name = "Fecha Entrega Solicitada")]
        public DateTime FechaEntregaSolicitada { get; set; }

        public string CadenaArticulos { get; set; }
        
        //[Display(Name = "Comentario")]
        public string Comentario { get; set; }

        //[Required]
        [Display(Name = "Descuento Cliente Preferencial (%)")]
        public double Descuento { get; set; }

        public ET.Pedido Pedido { get; set; }

        public IList<SelectListItem> Clientes { get; set; }

        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Display(Name = "Estado")]
        public string EstadoPedido { get; set; }

        public bool RealizarPedido { get; set; }

        public EditarViewModel()
        {
            this.Clientes = clienteBL.obtenerTodos().Select(
                c => new SelectListItem()
                {
                    Text = c.NombreFantasia,
                    Value = c.Id.ToString()
                }).ToList();
        }

        public void completarEditarVM()
        {
            pedidoBL.setearTotal(Pedido);
            FechaRealizado = Pedido.FechaRealizado;
            FechaEntregaSolicitada = Pedido.FechaEntregaSolicitada;
            Iva = parametroBL.obtenerIVA();
            Comentario = Pedido.Comentario;
            EstadoPedido = Pedido.Estado.Nombre;
            Descuento = Pedido.Cliente.Descuento;
            IdCliente = Pedido.Cliente.Id; //Ver que aparte de cargar el IdCliente, tengo que ajustar el DDL para que quede seleccionado el que corresponde
            IdPedido = Pedido.Id;
        }

        public void completarPedido()
        {
            cargarPedido();
            cargarCliente(); //Ver si en caso de modificar el DDL cuando lo edita un administrador, me deja el Cliente que seleccionó
            Pedido.Comentario = Comentario;
            Pedido.FechaRealizado = FechaRealizado;
            Pedido.FechaEntregaSolicitada = FechaEntregaSolicitada;
            cargarProductosPedidos();
            pedidoBL.setearTotal(Pedido);
        }

        private void cargarCliente()
        {
            Pedido.Cliente = clienteBL.obtener(IdCliente);
        }
        private void cargarPedido()
        {
            Pedido = pedidoBL.obtener(IdPedido);
        }

        private void cargarProductosPedidos()
        {
            if (CadenaArticulos != null)
            {
                Pedido.ProductosPedidos = new List<ET.ArticuloCantidad>();

                CadenaArticulos = CadenaArticulos.Trim();
                char c1 = ' ';
                char c2 = ';';
                string[] substrings = CadenaArticulos.Split(c1);
                for (int i = 0; i < substrings.Length; i++)
                {
                    string[] substrings2 = substrings[i].Split(c2);
                    ET.Articulo a = articuloBL.obtener(Convert.ToInt32(substrings2[0]));
                    ET.ArticuloCantidad ac = new ET.ArticuloCantidad()
                    {
                        Articulo = a,
                        PrecioUnitario = a.Precio,
                        Cantidad = Convert.ToInt32(substrings2[1])
                    };

                    Pedido.ProductosPedidos.Add(ac);
                }
            }
        }
    }
}
