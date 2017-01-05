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
        public string mensajeError { get; set; }
        public string mensajeSuccess { get; set; }

        private CategoriaBL categorialBL= new CategoriaBL();
        private FiltroBL filtroBL = new FiltroBL();

        public Articulo Articulo { get; set; }
        
        [Required]
        [Display(Name = "Código")]
        [StringLength(20)]
        public string Codigo { get; set; }

        [Required]
        [Display(Name = "Nombre Articulo")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [StringLength(250)]
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


        //************************************************************************
        //PROPIEDADES PARA MANEJO DE IMAGENES
        //************************************************************************
        public HttpPostedFileBase Archivo1 { get; set; }
        public HttpPostedFileBase Archivo2 { get; set; }
        public HttpPostedFileBase Archivo3 { get; set; }
        public HttpPostedFileBase Archivo4 { get; set; }
        public HttpPostedFileBase Archivo5 { get; set; }

        public bool EliminarArchivo1 { get; set; }
        public bool EliminarArchivo2 { get; set; }
        public bool EliminarArchivo3 { get; set; }
        public bool EliminarArchivo4 { get; set; }
        public bool EliminarArchivo5 { get; set; }
        public List<HttpPostedFileBase> Archivos { get; set; }
        //************************************************************************
        //FIN PROPIEDADES PARA MANEJO DE IMAGENES
        //************************************************************************

        //************************************************************************
        //PROPIEDADES PARA MANEJO DE FILTROS Y CATEGORIAS
        //************************************************************************
        public List<Categoria> Categorias { get; set; }
        public List<Filtro> Filtros { get; set; }

        public String CadenaCategorias { get; set; }
        public String CadenaFiltros { get; set; }
        //************************************************************************
        //FIN PROPIEDADES PARA MANEJO DE FILTROS Y CATEGORIAS
        //************************************************************************

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
            if (CadenaCategorias != null)
            {
                CadenaCategorias = CadenaCategorias.Trim();
                Char c1 = ' ';
                Char c2 = ';';
                String[] substrings = CadenaCategorias.Split(c1);
                for (int i = 0; i < substrings.Length; i++)
                {
                    String[] substrings2 = substrings[i].Split(c2);
                    Categoria c = new Categoria { Id = Convert.ToInt32(substrings2[0]) };
                    if (substrings2[1] == "true")
                    {
                        Articulo.Categorias.Remove(c);
                        Articulo.Categorias.Add(c);
                    }
                    else {
                        Articulo.Categorias.Remove(c);
                    }
                }
            }
        }

        private void cargarFiltros()
        {
            if (CadenaFiltros != null)
            {
                CadenaFiltros = CadenaFiltros.Trim();
                Char c1 = ' ';
                Char c2 = ';';
                String[] substrings = CadenaFiltros.Split(c1);
                for (int i = 0; i < substrings.Length; i++)
                {
                    String[] substrings2 = substrings[i].Split(c2);
                    Filtro f = new Filtro { Id = Convert.ToInt32(substrings2[0]) };
                    if (substrings2[1] == "true")
                    {
                        Articulo.Filtros.Remove(f);
                        Articulo.Filtros.Add(f);
                    }
                    else {
                        Articulo.Filtros.Remove(f);
                    }
                }
            }
        }

        private void cargarImagenes()
        {
            if (Archivo1 != null && !EliminarArchivo1)
                Archivos.Add(Archivo1);
            if (Archivo2 != null && !EliminarArchivo2)
                Archivos.Add(Archivo2);
            if (Archivo3 != null && !EliminarArchivo3)
                Archivos.Add(Archivo3);
            if (Archivo4 != null && !EliminarArchivo4)
                Archivos.Add(Archivo4);
            if (Archivo5 != null && !EliminarArchivo5)
                Archivos.Add(Archivo5);

            String nombreImg = Articulo.Codigo.ToUpper().Replace(" ", "") + "_IMG";
            for (int i = 1; i <= Archivos.Count; i++) {
                Articulo.Imagenes.Add(new Imagen() { Img= nombreImg + i + ".jpg" });
            }
        }

        public void guardarArchivo()
        {
            String nombreImg = Articulo.Codigo.ToUpper().Replace(" ", "") ;
            //string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Articulos/"+nombreImg);
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Articulos/");
            int i = 1;
            //Si no existe directorio se crea             
            if (!System.IO.Directory.Exists(ruta))
                System.IO.Directory.CreateDirectory(ruta);

            if (Archivos.Count>0)
            {                
                foreach (HttpPostedFileBase a in Archivos)
                {                   
                    if (a != null)
                    {                   
                        //Guardo el nuevo archivo
                        a.SaveAs(System.IO.Path.Combine(ruta, nombreImg + "_IMG" + i + ".jpg"));
                        i++;
                    }
                }          
            }
            else {
                //Asiganar imagen                   
                File.Copy(System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Articulos/"), "SinImagen.jpg"), System.IO.Path.Combine(ruta, nombreImg + "_IMG" + i + ".jpg"));
            }

        }
    }
}