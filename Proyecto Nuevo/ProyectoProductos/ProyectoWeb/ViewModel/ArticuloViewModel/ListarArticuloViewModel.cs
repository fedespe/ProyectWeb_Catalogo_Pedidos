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
    public class ListarArticuloViewModel
    {
        private FiltroBL filtroBL = new FiltroBL();
        private ArticuloBL articuloBL = new ArticuloBL();
        public string Codigo { get; set; }

        public List<Articulo> Articulos { get; set; }

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

        public ListarArticuloViewModel() {
            Articulos = articuloBL.obtenerTodos();
            FiltrosTotales = filtroBL.obtenerTodos();
            FiltrosAplicados = new List<Filtro>();
        }

        public void cargarArticulosFiltrados() {
            if (Codigo == null || Codigo.Equals(""))
            {
                cargarFiltros();
                if (ChkPrecio)
                {
                    Articulos = articuloBL.obtenerConFiltrosPorPrecio(FiltrosAplicados);
                }
                else {
                    Articulos = articuloBL.obtenerConFiltros(FiltrosAplicados);
                }
            }
            else {
                Articulos = new List<Articulo>();
                Articulo a = articuloBL.obtenerPorCodigo(Codigo);
                if (a != null) {
                    Articulos.Add(a);
                }               
            }
            
        }
        

    }
}