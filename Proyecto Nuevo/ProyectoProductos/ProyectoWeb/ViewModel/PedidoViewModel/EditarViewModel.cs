using BL;
using ET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        public List<ArticuloCantidad> FiltrosSeleccionados { get; set; }

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
        public string ComentarioAnterior { get; set; }

        //[Display(Name = "Comentario")]
        public string Comentario { get; set; }

        //[Required]
        [Display(Name = "Descuento Cliente Preferencial (%)")]
        public double Descuento { get; set; }

        public ET.Pedido Pedido { get; set; }        

        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Display(Name = "Estado")]
        public string EstadoPedido { get; set; }

        public bool RealizarPedido { get; set; }

        public List<Cliente> Clientes { get; set; }
        public int idClienteSeleccionado { get; set; }
        //*********************************************************
        //Filtros
        //*********************************************************
        public String CadenaFiltros { get; set; }

        private void cargarFiltros()
        {
            foreach (ArticuloCantidad a in Pedido.ProductosPedidos) {
                a.Articulo.Filtros = new List<Filtro>();
            }
            if (CadenaFiltros != null)
            {
                CadenaFiltros = CadenaFiltros.Trim();
                Char c1 = ' ';
                Char c2 = ';';
                String[] substrings = CadenaFiltros.Split(c1);
                for (int i = 0; i < substrings.Length; i++)
                {
                    String[] substrings2 = substrings[i].Split(c2);
                    Filtro f = new Filtro { Id = Convert.ToInt32(substrings2[0]) };
                    if (substrings2[2] == "true")
                    {
                        for (int j = 0; j < Pedido.ProductosPedidos.Count; j++) {
                            if (Pedido.ProductosPedidos.ElementAt(j).Id == Convert.ToInt32(substrings2[1])) {
                                Pedido.ProductosPedidos.ElementAt(j).Articulo.Filtros.Remove(f);
                                Pedido.ProductosPedidos.ElementAt(j).Articulo.Filtros.Add(f);
                                j = Pedido.ProductosPedidos.Count;
                            }
                        }
                    }
                    else {
                        for (int h = 0; h < Pedido.ProductosPedidos.Count; h++)
                        {
                            if (Pedido.ProductosPedidos.ElementAt(h).Id == Convert.ToInt32(substrings2[1]))
                            {
                                Pedido.ProductosPedidos.ElementAt(h).Articulo.Filtros.Remove(f);
                                h = Pedido.ProductosPedidos.Count;
                            }
                        }
                    }
                }
            }
        }

        //*********************************************************
        //Filtros Fin
        //*********************************************************

        public EditarViewModel()
        {
            this.Clientes = clienteBL.obtenerTodos();                
        }

        public void completarEditarVM()
        {
            pedidoBL.setearTotal(Pedido);

            FechaRealizado = Pedido.FechaRealizado;
            FechaEntregaSolicitada = Pedido.FechaEntregaSolicitada;

            Iva = parametroBL.obtenerIVA();
            ComentarioAnterior = Pedido.Comentario;
            EstadoPedido = Pedido.Estado.Nombre;
            Descuento = Pedido.Cliente.Descuento;
            RealizarPedido = false;
            IdCliente = Pedido.Cliente.Id; //Ver que aparte de cargar el IdCliente, tengo que ajustar el DDL para que quede seleccionado el que corresponde
            IdPedido = Pedido.Id;
            FiltrosSeleccionados = pedidoBL.obtenerFiltrosSeleccionados(Pedido);
        }

        public void completarPedido(string tipoUsuario)
        {
            cargarPedido();
            cargarCliente(); //Ver si en caso de modificar el DDL cuando lo edita un administrador, me deja el Cliente que seleccionó
            if (Comentario != null && !Comentario.Trim().Equals(""))
            {
                string comentario = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + tipoUsuario + ": " + Comentario.Trim();
                if (Pedido.Comentario != null && !Pedido.Comentario.Trim().Equals(""))
                    Pedido.Comentario = Pedido.Comentario.Trim() + "|" + comentario;
                else
                    Pedido.Comentario = comentario;
            }

            if(tipoUsuario.Equals("Administrador"))
                Pedido.FechaRealizado = FechaRealizado;

            Pedido.FechaEntregaSolicitada = FechaEntregaSolicitada;

            cargarProductosPedidos();
            pedidoBL.setearTotal(Pedido);
            cargarFiltros();//en el post
        }

        private void cargarCliente()
        {            
            if (idClienteSeleccionado != 0)
            {
                Pedido.Cliente = clienteBL.obtener(idClienteSeleccionado);
            }
            else {
                Pedido.Cliente = clienteBL.obtener(IdCliente);
            }
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
                        Id= Convert.ToInt32(substrings2[1]),
                        Articulo = a,
                        PrecioUnitario = a.Precio,
                        Cantidad = Convert.ToInt32(substrings2[2])
                    };

                    Pedido.ProductosPedidos.Add(ac);
                }
            }
        }
    }
}
