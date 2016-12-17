using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;

namespace ProyectoWeb.ViewModel
{
    public class ClienteViewModel
    {
        public Cliente cliente { get; set; }

        [Required]
        public HttpPostedFileBase Archivo { get; set; }
        
        
        public ClienteViewModel()
        {
            this.cliente = new Cliente();
        }

        public bool mapear()
        {
            if (this.Archivo != null)
            {
                if (guardarArchivo(Archivo))
                {
                    return true;
                }
            }
            return false;
        }
        public void colocarRuta() {
            //Lo guardo sin espacios
            this.cliente.Foto = this.cliente.NombreUsuario.ToUpper().Replace(" ", "") + ".jpg";
        }

        private bool guardarArchivo(HttpPostedFileBase archivo)
        {
            if (archivo != null)
            {
                //this.cliente.Foto = this.cliente.NombreUsuario.ToUpper() + ".jpg";
                string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Clientes/");
                if (!System.IO.Directory.Exists(ruta))
                    System.IO.Directory.CreateDirectory(ruta);

                ruta = System.IO.Path.Combine(ruta, this.cliente.Foto);
                archivo.SaveAs(ruta);
                return true;
            }
            else
                return false;
        }
    }
}