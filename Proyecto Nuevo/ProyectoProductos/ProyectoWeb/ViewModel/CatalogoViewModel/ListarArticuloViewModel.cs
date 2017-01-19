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
        private CategoriaBL categoriaBL = new CategoriaBL();

        public List<Articulo> Articulos { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCat { get; set; }
        public List<Categoria> Categorias { get; set; }

        //************************************************************************
        //PROPIEDADES PARA MANEJO DE FILTROS 
        //************************************************************************
        public bool ChkPrecio { get; set; }
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

        public ListarArticuloViewModel()
        {
            FiltrosTotales = filtroBL.obtenerTodos();
            FiltrosAplicados = new List<Filtro>();
            Categorias = categoriaBL.obtenerTodos();
        }
        public void completar(int id, string nombre) {
            IdCategoria = id;
            Articulos = articuloBL.obtenerPorCategoria(id);
            NombreCat = nombre;
        }

        public void cargarArticulosFiltrados()
        {
            cargarFiltros();
            if (ChkPrecio)
            {
                Articulos = articuloBL.obtenerPorCategoriaConFiltrosPorPrecio(IdCategoria, FiltrosAplicados);
            }
            else {
                Articulos = articuloBL.obtenerPorCategoriaConFiltros(IdCategoria, FiltrosAplicados);
            }            
        }
    }
}