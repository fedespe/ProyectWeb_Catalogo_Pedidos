using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Articulo : IEntity
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public bool Destacado { get; set; }
        public List<Imagen> Fotos { get; set; }
        public List<Categoria> Categorias { get; set; }
        public List<Filtro> Filtros { get; set; }
    }
}
