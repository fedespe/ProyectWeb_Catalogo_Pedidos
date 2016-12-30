using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AdministradorBL
    {
        private AdministradorDAL administradorDAL = new AdministradorDAL();

        public List<Administrador> obtenerTodos()
        {
            return administradorDAL.obtenerTodos();
        }
        public Administrador login(string nombre, string pass)
        {
            return administradorDAL.login(nombre,pass);
        }
        public Administrador obtener(int id)
        {
            return administradorDAL.obtener(id);
        }

        public bool actualizar(Administrador admin)
        {
            return administradorDAL.actualizar(admin);
        }
        public bool actualizarPassword(Administrador admin)
        {
            return administradorDAL.actualizarPassword(admin);
        }

        public void registrar(Administrador admin)
        {
            administradorDAL.registrar(admin);
        }

        public bool eliminar(int id)
        {
            return administradorDAL.eliminar(id);
        }

        public int obtenerPedidoEnContruccion(int id)
        {
            return administradorDAL.obtenerPedidoEnContruccion(id);
        }
    }
}
