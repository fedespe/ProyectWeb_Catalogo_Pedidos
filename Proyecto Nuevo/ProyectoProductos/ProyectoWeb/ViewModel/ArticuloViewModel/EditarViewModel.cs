using BL;
using ET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ProyectoWeb.ViewModel.ArticuloViewModel
{
    public class EditarViewModel
    {
        private CategoriaBL categorialBL = new CategoriaBL();
        private FiltroBL filtroBL = new FiltroBL();

        public ET.Articulo Articulo { get; set; }

        [Required]
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }

        [Required]
        [Display(Name = "Nombre Articulo")]
        [DataType(DataType.Password)]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Precio")]
        [Range(0, double.MaxValue)]
        public double Precio { get; set; }

        [Display(Name = "Stock")]
        [Range(0, double.MaxValue)]
        public int Stock { get; set; }

        [Display(Name = "Destacado")]
        public bool Destacado { get; set; }

        [Display(Name = "Disponible")]
        public bool Disponible { get; set; }

        //**********************************************************************
        //Para mirar
        //**********************************************************************
        public HttpPostedFileBase Archivo1 { get; set; }
        public HttpPostedFileBase Archivo2 { get; set; }
        public HttpPostedFileBase Archivo3 { get; set; }
        public HttpPostedFileBase Archivo4 { get; set; }
        public HttpPostedFileBase Archivo5 { get; set; }

        //ver si es una lista de archivos!
        public List<HttpPostedFileBase> Archivos { get; set; }
        public List<Categoria> Categorias { get; set; }
        public List<Filtro> Filtros { get; set; }
        public List<Imagen> Imagenes { get; set; }

        public int IdCategoria { get; set; }
        public int idFiltro { get; set; }
        //**********************************************************************
        //**********************************************************************


        public EditarViewModel()
        {
            Articulo = new Articulo();
            Categorias = categorialBL.obtenerTodos();
            Filtros = filtroBL.obtenerTodos();
        }
        public void completarEditarVM()
        {
            Codigo = Articulo.Codigo;
            Nombre = Articulo.Nombre;
            Descripcion = Articulo.Descripcion;
            Precio = Articulo.Precio;
            Stock = Articulo.Stock;
            Destacado = Articulo.Destacado;
            Disponible = Articulo.Disponible;
            Filtros = Articulo.Filtros;
            Categorias = Articulo.Categorias;
            Imagenes = Articulo.Imagenes;
        }

        public void completarArticulo()
        {
            Articulo.Codigo = Codigo;
            Articulo.Nombre = Nombre;
            Articulo.Descripcion = Descripcion;
            Articulo.Precio = Precio;
            Articulo.Stock = Stock;
            Articulo.Destacado = Destacado;
            Articulo.Disponible = Disponible;
        }
        
        public void guardarArchivo()
        {
            //string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Clientes/");
            //if (Archivo != null)
            //{
            //    //Si no existe directorio se crea             
            //    if (!System.IO.Directory.Exists(ruta))
            //        System.IO.Directory.CreateDirectory(ruta);
            //    //Guardo el nuevo archivo
            //    Archivo.SaveAs(System.IO.Path.Combine(ruta, this.Articulo.Foto));
            //    //Cambia el nombre y cambia la imagen, elimino la imagen anterior
            //    if (ImgAnterior != null && !ImgAnterior.Equals("") && !ImgAnterior.Equals(Articulo.Foto))
            //    {
            //        //Elimino la imagen anterior
            //        File.Delete(System.IO.Path.Combine(ruta, this.ImgAnterior));
            //    }
            //}
            //else {
            //    //Cambia el nombre de usuario y no imagen, actualizo el nombre de la imagen
            //    if (ImgAnterior != null)
            //    {
            //        //Cambiar nombre de imagen
            //        File.Move(System.IO.Path.Combine(ruta, ImgAnterior), System.IO.Path.Combine(ruta, this.Articulo.NombreUsuario.ToUpper().Replace(" ", "") + ".jpg"));
            //    }
            //}
        }

        public void eliminarArchivo()
        {
            //string rutaAnterior = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Clientes/");
            //File.Delete(System.IO.Path.Combine(rutaAnterior, this.Articulo.Foto));
        }
    }
}