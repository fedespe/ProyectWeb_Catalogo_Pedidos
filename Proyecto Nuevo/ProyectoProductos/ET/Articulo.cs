using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public bool Destacado { get; set; }
        public bool Disponible { get; set; }
        public List<Imagen> Imagenes { get; set; }
        public List<Categoria> Categorias { get; set; }
        public List<Filtro> Filtros { get; set; }

        public Articulo() {
            Imagenes = new List<Imagen>();
            Categorias = new List<Categoria>();
            Filtros = new List<Filtro>();
        }
    }
}
