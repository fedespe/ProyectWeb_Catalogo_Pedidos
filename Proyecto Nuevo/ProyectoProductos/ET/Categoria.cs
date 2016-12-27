using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Nombre { get; set; }
        //En la base no esta la propiedad
        //se usa?
        public string Descripcion { get; set; }
        public List<Articulo> Articulos { get; set; }
        public List<Filtro> Filtros { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Categoria) {//No se por que cuando edito una categoria entra y compara con string vacio que no es una categoria por eso el control
                Categoria c = (Categoria)obj;
                return c.Id == this.Id;
            }
            return false;
        }
    }
}
