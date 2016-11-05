using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Producto:IEntity
    {
        public int Id { get; set; }
        public int CodigoProducto { get; set; }
        public string Nombre { get; set; }
        public Imagen Imagen { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }

        public Producto(int codigoProd, string nombre, int stock,Imagen imagen, string descripcion) {
            this.CodigoProducto = codigoProd;
            this.Nombre = nombre;
            this.Imagen = imagen;
            this.Descripcion = descripcion;
            this.Stock = stock;
        }
    }
}
