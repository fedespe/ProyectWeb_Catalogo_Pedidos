using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ET;
using BL;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;

namespace ProyectoWeb.ViewModel.ArticuloViewModel
{
    public class CrearViewModel
    {
        private CategoriaBL categorialBL= new CategoriaBL();
        private FiltroBL filtroBL = new FiltroBL();

        public Articulo Articulo { get; set; }
        //ver si es una lista de archivos!
        public List<HttpPostedFileBase> Archivos { get; set; }
        public List<Categoria> Categorias { get; set; }
        public List<Filtro> Filtros { get; set; }

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

        public HttpPostedFileBase Archivo1 { get; set; }
        public HttpPostedFileBase Archivo2 { get; set; }
        public HttpPostedFileBase Archivo3 { get; set; }
        public HttpPostedFileBase Archivo4 { get; set; }
        public HttpPostedFileBase Archivo5 { get; set; }


        public int IdCategoria { get; set; }
        public int idFiltro { get; set; }

        public CrearViewModel()
        {
            Articulo = new Articulo();
            Archivos = new List<HttpPostedFileBase>();
            Categorias = categorialBL.obtenerTodos();
            Filtros = filtroBL.obtenerTodos();
        }

        public void completarArticulo()
        {
            Articulo.Codigo = Codigo;
            Articulo.Descripcion = Descripcion;
            Articulo.Destacado = Destacado;
            Articulo.Disponible = Disponible;
            Articulo.Nombre = Nombre;
            Articulo.Precio = Precio;
            Articulo.Stock = Stock;
            cargarImagenes();
            cargarFiltros();
            cargarCategorias();
        }

        private void cargarCategorias()
        {

        }

        private void cargarFiltros()
        {
            
        }

        private void cargarImagenes()
        {
            if (Archivo1 != null)
                Archivos.Add(Archivo1);
            if (Archivo2 != null)
                Archivos.Add(Archivo2);
            if (Archivo3 != null)
                Archivos.Add(Archivo3);
            if (Archivo4 != null)
                Archivos.Add(Archivo4);
            if (Archivo5 != null)
                Archivos.Add(Archivo5);
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
            //}
            //else {
            //    //Asiganar imagen                   
            //    File.Copy(System.IO.Path.Combine(ruta, "SinImagen.jpg"), System.IO.Path.Combine(ruta, this.Articulo.Foto));
            //}
        }
    }

}