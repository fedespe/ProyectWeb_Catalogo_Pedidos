﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.LogicaNegocio
{
    public class Categoria : IEntity
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Articulo> Articulos { get; set; }
        public List<Filtro> Filtros { get; set; }
    }
}
