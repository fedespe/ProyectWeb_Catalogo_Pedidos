using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ET;

namespace BL
{
    public class ArticuloBL
    {
        private ArticuloDAL articuloDAL = new ArticuloDAL();

        public List<Articulo> obtenerTodos()
        {
            return articuloDAL.obtenerTodos();
        }
        public Articulo obtener(int id)
        {
            return articuloDAL.obtener(id);
        }

        public bool actualizar(Articulo articulo)
        {
            this.validar(articulo); 
            return articuloDAL.actualizar(articulo);
        }

        public void registrar(Articulo articulo)
        {
            this.validar(articulo);
            articuloDAL.registrar(articulo);
        }

        public bool inhabilitar(int id)
        {
            return articuloDAL.inhabilitar(id);
        }
        public bool habilitar(int id)
        {
            return articuloDAL.habilitar(id);
        }
        public bool destacar(int id)
        {
            return articuloDAL.destacar(id);
        }
        public bool quitarDestacado(int id)
        {
            return articuloDAL.quitarDestacado(id);
        }
        //Todos los articulo con los filtros elegidos
        public List<Articulo> obtenerConFiltros(List<Filtro> filtros) {
            return articuloDAL.obtenerConFiltros(filtros);
        }
        //todos los articulos de una categoria
        public List<Articulo> obtenerPorCategoria(int idCategoria) {
            return articuloDAL.obtenerPorCategoria(idCategoria);
        }
        public List<Articulo> obtenerPorCategoriaConFiltros(int idCategoria, List<Filtro> filtros)
        {
            return articuloDAL.obtenerPorCategoriaConFiltros(idCategoria, filtros);
        }

        private void validar(Articulo articulo)
        {
            //Si no tiene imagen el obtener todos no trae el articulo por eso
            //validar que tenga al menos una imagen para guardar
            if (articulo.Imagenes.Count==0) 
                throw new ProyectoException("Error: el articulo debe tener al menos una imagen.");
            if(articulo.Codigo=="" || articulo.Codigo.Length>20)
                throw new ProyectoException("Error: el codigo del articulo es requerido y menor a 20 caracteres.");
        }

    }
}
