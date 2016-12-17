using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ET;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace ProyectoWeb.ViewModel.ClienteViewModel
{
    public class EditarViewModel
    {
        public ET.Cliente cliente { get; set; }

        public HttpPostedFileBase Archivo { get; set; }

        public String ImgAnterior { get; set; }

        [Required]
        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        //[Required]
        //[Display(Name = "Password")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

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


        public EditarViewModel()
        {
            this.cliente = new ET.Cliente();
        }
        public void completarEditarVM()
        {
            ImgAnterior = cliente.Foto;
            Descuento = cliente.Descuento;
            DiasDePago = cliente.DiasDePago;
            Direccion = cliente.Direccion;
            NombreUsuario = cliente.Foto;
            NombreDeContacto = cliente.NombreDeContacto;
            NombreFantasia = cliente.NombreFantasia;
            NombreUsuario = cliente.NombreUsuario;
            RazonSocial = cliente.RazonSocial;
            Rut = cliente.Rut;
            Telefono = cliente.Telefono;
            TelefonoDeContacto = cliente.TelefonoDeContacto;
        }

        public void completarCliente()
        {
            cliente.Descuento = Descuento;
            cliente.DiasDePago = DiasDePago;
            cliente.Direccion = Direccion;
            cliente.Foto = NombreUsuario.ToUpper().Replace(" ", "") + ".jpg";
            cliente.NombreDeContacto = NombreDeContacto;
            cliente.NombreFantasia = NombreFantasia;
            cliente.NombreUsuario = NombreUsuario;
            //cliente.Password = Password;
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
                //Cambia el nombre y cambia la imagen, elimino la imagen anterior
                if (ImgAnterior != null && !ImgAnterior.Equals("") && !ImgAnterior.Equals(cliente.Foto))
                {
                    //Elimino la imagen anterior
                    File.Delete(System.IO.Path.Combine(ruta, this.ImgAnterior));
                }
            }
            else {
                //Cambia el nombre de usuario y no imagen, actualizo el nombre de la imagen
                if (ImgAnterior != null)
                {
                    //Cambiar nombre de imagen
                    File.Move(System.IO.Path.Combine(ruta, ImgAnterior), System.IO.Path.Combine(ruta, this.cliente.NombreUsuario.ToUpper().Replace(" ", "") + ".jpg"));
                }
            }
        }

        public void eliminarArchivo()
        {
            string rutaAnterior = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Clientes/");
            File.Delete(System.IO.Path.Combine(rutaAnterior, this.cliente.Foto));
        }
    }
}