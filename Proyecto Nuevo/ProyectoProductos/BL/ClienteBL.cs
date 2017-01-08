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
            validar(cli);
            return clienteDAL.actualizar(cli);
        }
        public bool actualizarPassword(Cliente cli)
        {
            return clienteDAL.actualizarPassword(cli);
        }

        public void registrar(Cliente cli)
        {
            validar(cli);
            clienteDAL.registrar(cli);
        }

        public bool eliminar(int id)
        {
            return clienteDAL.eliminar(id);
        }

        public int obtenerPedidoEnContruccion(int id)
        {
            return clienteDAL.obtenerPedidoEnContruccion(id);
        }

        internal void registrarPedidoEnConstruccion(Cliente c, int idPedidoGenerado)
        {
            c.IdPedidoEnConstruccion = idPedidoGenerado;
            clienteDAL.actualizar(c);
        }

        private void validar(Cliente cliente)
        {
            //falta validar que son unicos: nombre de usuario, rut, razon social (lo controla la base por ahora) 
            if (cliente.Foto == null)
                throw new ProyectoException("Error: el cliente debe tener al menos una imagen.");
            if (cliente.NombreUsuario == "" || cliente.NombreUsuario.Length > 20)
                throw new ProyectoException("Error: el nombre de usuario es requerido y menor a 20 caracteres.");
            if (cliente.Password == "")
                throw new ProyectoException("Error: la contraseña es requerido.");
            if (cliente.NombreFantasia.Length > 100)
                throw new ProyectoException("Error: el nombre de fantasía debe ser menor a 100 caracteres.");
            if (cliente.Rut.Length > 50)
                throw new ProyectoException("Error: el RUT debe ser menor a 50 caracteres.");
            if (cliente.RazonSocial.Length > 50)
                throw new ProyectoException("Error: la razón social debe ser menor a 50 caracteres.");
            if (cliente.Descuento >=100)
                throw new ProyectoException("Error: el descuento debe ser menor a 100 caracteres.");
            if (cliente.DiasDePago.Length > 50)
                throw new ProyectoException("Error: los dias de pagos debe ser menor a 50 caracteres.");
            if (cliente.Direccion.Length > 100)
                throw new ProyectoException("Error: la dirección debe ser menor a 100 caracteres.");
            if (cliente.Telefono.Length > 30)
                throw new ProyectoException("Error: el teléfono debe ser menor a 30 caracteres.");
            if (cliente.NombreDeContacto.Length > 50)
                throw new ProyectoException("Error: el nombre de contacto debe ser menor a 50 caracteres.");
            if (cliente.EmailDeContacto.Length > 50)
                throw new ProyectoException("Error: el email debe ser menor a 50 caracteres.");
            if (cliente.TelefonoDeContacto.Length > 30)
                throw new ProyectoException("Error: el teléfono de contacto debe ser menor a 20 caracteres.");
        }

        public int obtenerPrimerCliente()
        {
            return clienteDAL.obtenerPrimerCliente();
        }
    }
}
