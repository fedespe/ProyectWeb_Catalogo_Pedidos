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
        public List<Articulo> obtenerConFiltros(List<Filtro> Filtros) {
            return articuloDAL.obtenerConFiltros(Filtros);
        }

        private void validar(Articulo articulo)
        {
            //throw new NotImplementedException();
        }

    }
}
