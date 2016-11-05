using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Usuario:IEntity
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public Rol Rol { get; set; }

        public Usuario(string nombreUsuario, string password,Rol rol) {
            this.NombreUsuario = nombreUsuario;
            this.Password = password;
            this.Rol = rol;
        }
    }
}
