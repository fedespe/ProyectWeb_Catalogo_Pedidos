using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FiltroBL
    {
        private FiltroDAL filtroDAL = new FiltroDAL();

        public List<Filtro> obtenerTodos()
        {
            return filtroDAL.obtenerTodos();
        }

        public Filtro obtener(int id)
        {
            return filtroDAL.obtener(id);
        }
         
    }
}
