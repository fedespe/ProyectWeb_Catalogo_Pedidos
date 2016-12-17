using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ET;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace ProyectoWeb.ViewModel.ClienteViewModel
{
    public class CrearViewModel
    {
        public ET.Cliente cliente { get; set; }

        public HttpPostedFileBase Archivo { get; set; }

        [Required]
        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Nombre Fantasía")]
        public string NombreFantasia { get; set; }

        [Required]
        [Display(Name = "Rut")]
        public string Rut { get; set; }

        [Required]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [Required]
        [Display(Name = "Descuento")]
        [Range(0, double.MaxValue)]
        public double Descuento { get; set; }

        [Display(Name = "Días de pago")]
        public string DiasDePago { get; set; }

        [Display(Name = "Dirección")]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Display(Name = "Nombre de Contacto")]
        public string NombreDeContacto { get; set; }

        [Display(Name = "Teléfono de Contacto")]
        public string TelefonoDeContacto { get; set; }


        public CrearViewModel()
        {
            this.cliente = new ET.Cliente();
        }

        public void completarCliente() {
            cliente.Descuento = Descuento;
            cliente.DiasDePago = DiasDePago;
            cliente.Direccion = Direccion;
            cliente.Foto = NombreUsuario.ToUpper().Replace(" ", "") + ".jpg";
            cliente.NombreDeContacto = NombreDeContacto;
            cliente.NombreFantasia = NombreFantasia;
            cliente.NombreUsuario = NombreUsuario;
            cliente.Password = Password;
            cliente.RazonSocial = RazonSocial;
            cliente.Rut = Rut;
            cliente.Telefono = Telefono;
            cliente.TelefonoDeContacto = TelefonoDeContacto;
        }

        public void guardarArchivo()
        {
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Clientes/");
            if (Archivo != null)
            {
                //Si no existe directorio se crea             
                if (!System.IO.Directory.Exists(ruta))
                    System.IO.Directory.CreateDirectory(ruta);
                //Guardo el nuevo archivo
                Archivo.SaveAs(System.IO.Path.Combine(ruta, this.cliente.Foto));
            }
            else {
                //Asiganar imagen                   
                File.Copy(System.IO.Path.Combine(ruta, "NuevoMuestra.jpg"), System.IO.Path.Combine(ruta, this.cliente.Foto));                
            }
        }
    }
}