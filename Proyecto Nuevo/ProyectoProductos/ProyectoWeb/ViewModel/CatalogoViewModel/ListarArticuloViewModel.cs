using BL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoWeb.ViewModel.CatalogoViewModel
{
    public class ListarArticuloViewModel
    {
        private FiltroBL filtroBL = new FiltroBL();
        private ArticuloBL articuloBL = new ArticuloBL();

        public List<Articulo> Articulos { get; set; }
        public int IdCategoria { get; set; }

        //************************************************************************
        //PROPIEDADES PARA MANEJO DE FILTROS 
        //************************************************************************
        public List<Filtro> FiltrosTotales { get; set; }
        public List<Filtro> FiltrosAplicados { get; set; }

        public String CadenaFiltros { get; set; }

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
                        FiltrosAplicados.Remove(f);
                        FiltrosAplicados.Add(f);
                    }
                    else {
                        FiltrosAplicados.Remove(f);
                    }
                }
            }
        }
        //************************************************************************
        //FIN PROPIEDADES PARA MANEJO DE FILTROS 
        //************************************************************************

        public ListarArticuloViewModel()//(int idCat)
        {
            //Articulos = articuloBL.obtenerPorCategoria(IdCategoria);
            //Articulos = articuloBL.obtenerTodos();
            FiltrosTotales = filtroBL.obtenerTodos();
            FiltrosAplicados = new List<Filtro>();
            //IdCategoria = idCat;
        }

        public void cargarArticulosFiltrados()
        {
            cargarFiltros();
            //Articulos = articuloBL.obtenerConFiltros(FiltrosAplicados);
            Articulos = articuloBL.obtenerPorCategoriaConFiltros(IdCategoria,FiltrosAplicados);
        }
    }
}