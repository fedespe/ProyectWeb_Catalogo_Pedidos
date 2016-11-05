using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class ProductoCantidad
    {
        public int Cantidad { get; set; }
        public Producto Producto { get; set; }

        public ProductoCantidad(int cantidad, Producto producto) {
            this.Cantidad = cantidad;
            this.Producto = producto;
        }
    }
}
