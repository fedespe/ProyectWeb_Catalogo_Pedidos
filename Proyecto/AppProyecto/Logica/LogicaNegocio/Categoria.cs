using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Categoria : IEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Imagen Imagen { get; set; }
        public List<Producto> Productos { get; set; }       

        public Categoria(string nombre, Imagen imagen) {
            this.Nombre = nombre;
            this.Imagen = imagen;
            this.Productos= new List<Producto>();
        }

    }
}
