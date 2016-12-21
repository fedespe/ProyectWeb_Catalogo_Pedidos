using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CategoriaBL
    {
        private CategoriaDAL categoriaDAL = new CategoriaDAL();

        public List<Categoria> obtenerTodos()
        {
            return categoriaDAL.obtenerTodos();
        }
        public Categoria obtener(int id)
        {
            return categoriaDAL.obtener(id);
        }

        public bool actualizar(Categoria cat)
        {
            return categoriaDAL.actualizar(cat);
        }

        public void registrar(Categoria cat)
        {
            categoriaDAL.registrar(cat);
        }

        public bool eliminar(int id)
        {
            return categoriaDAL.eliminar(id);
        }
    }
}
