using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Cliente : Rol
    {
        public string Direccion { get; set; }
        //el descuento va a depender del tipo de cliente que sea
        public double Descuento { get; set; }
        public Cliente(string direccion,double descuento):base(){
            this.Direccion = direccion;
            this.Descuento = descuento;
        }
    }
}
