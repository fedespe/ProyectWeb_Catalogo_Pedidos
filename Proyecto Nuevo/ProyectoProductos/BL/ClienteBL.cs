using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ClienteBL
    {
        private ClienteDAL clienteDAL = new ClienteDAL();

        public List<Cliente> obtenerTodos()
        {
            return clienteDAL.obtenerTodos();
        }
        public Cliente login(string nombre, string pass)
        {
            return clienteDAL.login(nombre, pass);
        }
        public Cliente obtener(int id)
        {
            return clienteDAL.obtener(id);
        }

        public bool actualizar(Cliente cli)
        {
            return clienteDAL.actualizar(cli);
        }
        public bool actualizarPassword(Cliente cli)
        {
            return clienteDAL.actualizarPassword(cli);
        }

        public void registrar(Cliente cli)
        {
            clienteDAL.registrar(cli);
        }

        public bool eliminar(int id)
        {
            return clienteDAL.eliminar(id);
        }
    }
}
