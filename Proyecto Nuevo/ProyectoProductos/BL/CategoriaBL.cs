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
    }
}
