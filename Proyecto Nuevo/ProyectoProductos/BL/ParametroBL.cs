using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ParametroBL
    {
        private ParametroDAL parametroDAL = new ParametroDAL();

        public double obtenerIVA()
        {
            return parametroDAL.obtenerIVA();
        }
    }
}
