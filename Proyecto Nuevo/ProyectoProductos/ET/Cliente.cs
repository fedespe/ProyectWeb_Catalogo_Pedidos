using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Cliente : Usuario
    {
        //[Required]
        //[Display(Name = "Nombre Fantasía")]
        public string NombreFantasia { get; set; }

        //[Required]
        //[Display(Name = "Rut")]
        public string Rut { get; set; }

        //[Required]
        //[Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        //[Required]
        //[Display(Name = "Descuento")]
        //[Range(0, double.MaxValue)]
        public double Descuento { get; set; }

        //[Display(Name = "Días de pago")]
        public string DiasDePago { get; set; }

        //[Display(Name = "Dirección")]
        //[DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        //[Display(Name = "Teléfono")]
        //[DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        //[Display(Name = "Nombre de Contacto")]
        public string NombreDeContacto { get; set; }

        //[Display(Name = "Teléfono de Contacto")]
        public string TelefonoDeContacto { get; set; }

        //[RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Correo no válido")]
        public string EmailDeContacto { get; set; }

        public string Foto { get; set; }

        public List<Pedido> Pedidos { get; set; }
    }
}
