using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ProyectoWeb.ViewModel.CategoriaViewModel
{
    public class CrearViewModel
    {
        public ET.Categoria categoria { get; set; }

        public HttpPostedFileBase Archivo { get; set; }

        [Required]
        [Display(Name = "Nombre Categoría")]
        public string Nombre { get; set; }

        public CrearViewModel()
        {
            this.categoria = new ET.Categoria();
        }

        public void completarCategoria()
        {
            categoria.Nombre = Nombre;
            categoria.Img = Nombre.ToUpper().Replace(" ", "") + ".jpg";
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
            }
            else {
                //Asiganar imagen                   
                File.Copy(System.IO.Path.Combine(ruta, "SinImagen.jpg"), System.IO.Path.Combine(ruta, this.categoria.Img));
            }
        }
    }
}