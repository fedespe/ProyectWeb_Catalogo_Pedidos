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
        public string mensajeError { get; set; }
        public string mensajeSuccess { get; set; }

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

        //[Required]
        [Display(Name = "Rut")]
        public string Rut { get; set; }

        //[Required]
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

        [Display(Name = "Email de Contacto")]
        public string EmailContacto { get; set; }


        public CrearViewModel()
        {
            this.cliente = new ET.Cliente();
        }

        public void completarCliente() {
            cliente.Descuento = Descuento;

            if (DiasDePago != null) cliente.DiasDePago = DiasDePago;
            else cliente.DiasDePago = "";

            if (Direccion != null) cliente.Direccion = Direccion;
            else cliente.Direccion = "";          

            if (NombreDeContacto != null) cliente.NombreDeContacto = NombreDeContacto;
            else cliente.NombreDeContacto = "";

            if (NombreFantasia != null) cliente.NombreFantasia = NombreFantasia;
            else cliente.NombreFantasia = "";

            if (NombreUsuario != null) cliente.NombreUsuario = NombreUsuario;
            else cliente.NombreUsuario = "";

            cliente.Foto = cliente.NombreUsuario.ToUpper().Replace(" ", "") + ".jpg";

            if (Password != null) cliente.Password = Password;
            else cliente.Password = "";

            if (RazonSocial != null) cliente.RazonSocial = RazonSocial;
            else cliente.RazonSocial = "";

            if (Rut != null) cliente.Rut = Rut;
            else cliente.Rut = "";

            if (Telefono != null) cliente.Telefono = Telefono;
            else cliente.Telefono = "";

            if (TelefonoDeContacto != null) cliente.TelefonoDeContacto = TelefonoDeContacto;
            else cliente.TelefonoDeContacto = "";

            if (EmailContacto != null) cliente.EmailDeContacto = EmailContacto;
            else cliente.EmailDeContacto = "";
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
                File.Copy(System.IO.Path.Combine(ruta, "SinImagen.jpg"), System.IO.Path.Combine(ruta, this.cliente.Foto));                
            }
        }
    }
}