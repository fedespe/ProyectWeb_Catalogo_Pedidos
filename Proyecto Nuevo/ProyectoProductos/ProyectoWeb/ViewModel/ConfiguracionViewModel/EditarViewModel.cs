using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoWeb.ViewModel.ConfiguracionViewModel
{
    public class EditarViewModel
    {
        public string mensajeError { get; set; }
        public string mensajeSuccess { get; set; }
        //************************************************************************
        //PROPIEDADES PARA MANEJO DE IMAGENES
        //************************************************************************
        public HttpPostedFileBase Archivo1 { get; set; }
        public HttpPostedFileBase Archivo2 { get; set; }
        public HttpPostedFileBase Archivo3 { get; set; }
        public HttpPostedFileBase Archivo4 { get; set; }

        public void guardarArchivos() {
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes/Slider/");
            if (Archivo1 != null)
            {
                guardarUnArchivo(Archivo1, ruta, "Carousel_IMG1.jpg");
            }
            if (Archivo2 != null)
            {
                guardarUnArchivo(Archivo2, ruta, "Carousel_IMG2.jpg");
            }
            if (Archivo3 != null)
            {
                guardarUnArchivo(Archivo3, ruta, "Carousel_IMG3.jpg");
            }
            if (Archivo4 != null)
            {
                guardarUnArchivo(Archivo4, ruta, "Carousel_IMG4.jpg");
            }
        }

        public void guardarUnArchivo(HttpPostedFileBase archivo, string ruta, string nomImg)
        {
            if (archivo != null)
            {
                //Si no existe directorio se crea             
                if (!System.IO.Directory.Exists(ruta))
                    System.IO.Directory.CreateDirectory(ruta);
                //Guardo el nuevo archivo
                archivo.SaveAs(System.IO.Path.Combine(ruta, nomImg));
            }
        }



    }
}