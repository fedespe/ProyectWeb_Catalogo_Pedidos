using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoWeb.ViewModel.HomeViewModel
{
    public class IndexViewModel
    {
        public List<Articulo> ArticulosDestacados { get; set; }
        public List<Categoria> Categorias { get; set; }
    }
}