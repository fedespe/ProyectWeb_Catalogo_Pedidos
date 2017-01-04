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
        public string mensajeError { get; set; }
        public string mensajeSuccess { get; set; }

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

        //************************************************************************
        //PROPIEDADES PARA MANEJO DE IMAGENES
        //************************************************************************
        public HttpPostedFileBase Archivo1 { get; set; }
        public HttpPostedFileBase Archivo2 { get; set; }
        public HttpPostedFileBase Archivo3 { get; set; }
        public HttpPostedFileBase Archivo4 { get; set; }
        public HttpPostedFileBase Archivo5 { get; set; }

        public String Img1Anterior { get; set; }
        public String Img2Anterior { get; set; }
        public String Img3Anterior { get; set; }
        public String Img4Anterior { get; set; }
        public String Img5Anterior { get; set; }

        public bool EliminarArchivo1 { get; set; }
        public bool EliminarArchivo2 { get; set; }
        public bool EliminarArchivo3 { get; set; }
        public bool EliminarArchivo4 { get; set; }
        public bool EliminarArchivo5 { get; set; }
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

        public EditarViewModel()
        {
            Articulo = new Articulo();
            Categorias = categorialBL.obtenerTodos();
            Filtros = filtroBL.obtenerTodos();
        }
        //Para cargar las imagenes que ya tenia en la base si hay errores
        public void cargarImagenesSeleccionas() {
            if (Img1Anterior != null)
                Articulo.Imagenes.Add(new Imagen() { Img = Img1Anterior });
            if (Img2Anterior != null)
                Articulo.Imagenes.Add(new Imagen() { Img = Img2Anterior });
            if (Img3Anterior != null)
                Articulo.Imagenes.Add(new Imagen() { Img = Img3Anterior });
            if (Img4Anterior != null)
                Articulo.Imagenes.Add(new Imagen() { Img = Img4Anterior });
            if (Img5Anterior != null)
                Articulo.Imagenes.Add(new Imagen() { Img = Img5Anterior });
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
            int c = 1;
            foreach (Imagen i in Articulo.Imagenes) {
                if (c == 1)Img1Anterior = i.Img;
                if (c == 2)Img2Anterior = i.Img;
                if (c == 3)Img3Anterior = i.Img;
                if (c == 4)Img4Anterior = i.Img;
                if (c == 5)Img5Anterior = i.Img;
                c++;
            }
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
            String nombreImg = Articulo.Codigo.ToUpper().Replace(" ", "") + "_IMG";
            if (!EliminarArchivo1 && (Archivo1 != null || Img1Anterior != null))
                Articulo.Imagenes.Add(new Imagen() { Img = nombreImg + 1 + ".jpg" });
            if (!EliminarArchivo2 && (Archivo2 != null || Img2Anterior != null))
                Articulo.Imagenes.Add(new Imagen() { Img = nombreImg + 2 + ".jpg" });
            if (!EliminarArchivo3 && (Archivo3 != null || Img3Anterior != null))
                Articulo.Imagenes.Add(new Imagen() { Img = nombreImg + 3 + ".jpg" });
            if (!EliminarArchivo4 && (Archivo4 != null || Img4Anterior != null))
                Articulo.Imagenes.Add(new Imagen() { Img = nombreImg + 4 + ".jpg" });
            if (!EliminarArchivo5 && (Archivo5 != null || Img5Anterior != null))
                Articulo.Imagenes.Add(new Imagen() { Img = nombreImg + 5 + ".jpg" });
        }

        public void guardarArchivo()
        {
            String nombreImg = Articulo.Codigo.ToUpper().Replace(" ", "") + "_IMG";
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Articulos/" + nombreImg);
            if (Archivo1 != null || Img1Anterior != null) {
                if (!EliminarArchivo1) {
                    guardarUnArchivo(Archivo1, nombreImg + 1 + ".jpg", Img1Anterior);
                }
                else {
                    if (Img1Anterior != null)
                        eliminarArchivo(Img1Anterior);
                }         
            }
            if (Archivo2 != null || Img2Anterior != null)
            {
                if (!EliminarArchivo2)
                {
                    guardarUnArchivo(Archivo2, nombreImg + 2 + ".jpg", Img2Anterior);
                }
                else {
                    if (Img2Anterior != null)
                        eliminarArchivo(Img2Anterior);
                }
            }
            if (Archivo3 != null || Img3Anterior != null)
            {
                if (!EliminarArchivo3)
                {
                    guardarUnArchivo(Archivo3, nombreImg + 3 + ".jpg", Img3Anterior);
                }
                else {
                    if (Img3Anterior != null)
                        eliminarArchivo(Img3Anterior);
                }
            }
            if (Archivo4 != null || Img4Anterior != null)
            {
                if (!EliminarArchivo4)
                {
                    guardarUnArchivo(Archivo4, nombreImg + 4 + ".jpg", Img4Anterior);
                }
                else {
                    if (Img4Anterior != null)
                        eliminarArchivo(Img4Anterior);
                }
            }
            if (Archivo5 != null || Img5Anterior != null)
            {
                if (!EliminarArchivo5)
                {
                    guardarUnArchivo(Archivo5, nombreImg + 5 + ".jpg", Img5Anterior);
                }
                else {
                    if (Img5Anterior != null)
                        eliminarArchivo(Img5Anterior);
                }
            }
        }
        public void guardarUnArchivo(HttpPostedFileBase archivo, String nombreImagen, String imgAnterior) {
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Articulos/");
            if (archivo != null)
            {
                //Si no existe directorio se crea             
                if (!System.IO.Directory.Exists(ruta))
                    System.IO.Directory.CreateDirectory(ruta);
                //Guardo el nuevo archivo
                archivo.SaveAs(System.IO.Path.Combine(ruta, nombreImagen));
                //Cambia el nombre y cambia la imagen, elimino la imagen anterior
                if (imgAnterior != null && !imgAnterior.Equals("") && !imgAnterior.Equals(nombreImagen))
                {
                    //Elimino la imagen anterior
                    File.Delete(System.IO.Path.Combine(ruta, imgAnterior));
                }
            }
            else {
                //Cambia el nombre de usuario y no imagen, actualizo el nombre de la imagen
                if (imgAnterior != null)
                {
                    //Cambiar nombre de imagen
                    File.Move(System.IO.Path.Combine(ruta, imgAnterior), System.IO.Path.Combine(ruta, nombreImagen));
                }
            }
        }

        public void eliminarArchivo(String img)
        {
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Articulos/");
            File.Delete(System.IO.Path.Combine(ruta, img));
        }
    }
}