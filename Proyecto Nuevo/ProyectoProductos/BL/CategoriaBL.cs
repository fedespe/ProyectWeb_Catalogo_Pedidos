using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CategoriaBL
    {
        private CategoriaDAL categoriaDAL = new CategoriaDAL();

        public List<Categoria> obtenerTodos()
        {
            return categoriaDAL.obtenerTodos();
        }
        public Categoria obtener(int id)
        {
            return categoriaDAL.obtener(id);
        }

        public bool actualizar(Categoria cat)
        {
            validar(cat);
            return categoriaDAL.actualizar(cat);
        }

        public void registrar(Categoria cat)
        {
            validar(cat);
            categoriaDAL.registrar(cat);
        }

        public bool eliminar(int id)
        {
            return categoriaDAL.eliminar(id);
        }
        private void validar(Categoria categoria)
        {
            if (categoria.Img == null)
                throw new ProyectoException("Error: la categoría debe tener al menos una imagen.");
            if (categoria.Nombre == "" || categoria.Nombre.Length > 50)
                throw new ProyectoException("Error: el noimbre de la categoría es requerido y menor a 50 caracteres.");
            //falta ver que el nombre sea unico           
        }

    }
}
