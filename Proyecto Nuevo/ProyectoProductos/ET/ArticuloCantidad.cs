using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ArticuloCantidad
    {
        public int Cantidad { get; set; }
        public Articulo Articulo { get; set; }
        public double PrecioUnitario { get; set; }

        public override bool Equals(object obj)
        {
            ArticuloCantidad ac = (ArticuloCantidad)obj;

            return this.Articulo.Equals(ac.Articulo);
        }
    }
}
