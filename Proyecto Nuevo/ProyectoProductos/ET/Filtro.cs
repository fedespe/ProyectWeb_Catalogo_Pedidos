using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Filtro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Color { get; set; }
        public List<Articulo> Articulos { get; set; }
    }
}
