using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using System.IO;

namespace ProyectoWeb.ViewModel
{
    public class ClienteViewModel
    {
        public Cliente cliente { get; set; }

        public HttpPostedFileBase Archivo { get; set; }

        public String ImgAnterior { get; set; }


        public ClienteViewModel()
        {
            this.cliente = new Cliente();
        }

        public void colocarRuta() {
            //Lo guardo sin espacios
            this.cliente.Foto = this.cliente.NombreUsuario.ToUpper().Replace(" ", "") + ".jpg";
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
                if (ImgAnterior!=null && !ImgAnterior.Equals("") && !ImgAnterior.Equals(cliente.Foto))
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
                //Si esta creando un usuario y no elige foto le asigo una
                else {
                    //Asiganar imagen                   
                    File.Copy(System.IO.Path.Combine(ruta, "NuevoMuestra.jpg"), System.IO.Path.Combine(ruta, this.cliente.Foto));
                }
            }            
        }
        public void eliminarArchivo() {
            string rutaAnterior = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Clientes/");
            File.Delete(System.IO.Path.Combine(rutaAnterior, this.cliente.Foto));
        }
    }
}