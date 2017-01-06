using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ProyectoWeb.ViewModel.CategoriaViewModel
{
    public class EditarViewModel
    {
        public string mensajeError { get; set; }
        public string mensajeSuccess { get; set; }

        public ET.Categoria categoria { get; set; }

        public HttpPostedFileBase Archivo { get; set; }

        public String ImgAnterior { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre Categoría")]
        public string Nombre { get; set; }


        public EditarViewModel()
        {
            this.categoria = new ET.Categoria();
        }
        public void completarEditarVM()
        {
            ImgAnterior = categoria.Img;
            Nombre = categoria.Nombre;
        }

        public void completarCliente()
        {
            if (Nombre != null) categoria.Nombre = Nombre;
            else categoria.Nombre = "";

            categoria.Img = categoria.Nombre.ToUpper().Replace(" ", "") + ".jpg";
        }

        public void guardarArchivo()
        {
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Categorias/");
            if (Archivo != null)
            {
                //Si no existe directorio se crea             
                if (!System.IO.Directory.Exists(ruta))
                    System.IO.Directory.CreateDirectory(ruta);
                //Guardo el nuevo archivo
                Archivo.SaveAs(System.IO.Path.Combine(ruta, this.categoria.Img));
                //Cambia el nombre y cambia la imagen, elimino la imagen anterior
                if (ImgAnterior != null && !ImgAnterior.Equals("") && !ImgAnterior.Equals(categoria.Img))
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
                    File.Move(System.IO.Path.Combine(ruta, ImgAnterior), System.IO.Path.Combine(ruta, this.categoria.Nombre.ToUpper().Replace(" ", "") + ".jpg"));
                }
            }
        }

        public void eliminarArchivo()
        {
            string rutaAnterior = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Categorias/");
            File.Delete(System.IO.Path.Combine(rutaAnterior, this.categoria.Img));
        }
    }
}