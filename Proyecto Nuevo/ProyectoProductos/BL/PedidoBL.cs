﻿using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PedidoBL
    {
        private PedidoDAL pedidoDAL = new PedidoDAL();

        public List<Pedido> obtenerTodosSinContarEnConstruccion()
        {
            return pedidoDAL.obtenerTodosSinContarEnConstruccion();
        }
        //Obtiene el pedido por id, trae los articulos con todos sus filtros, 
        //incluyendo los no seleccionados
        public Pedido obtener(int id)
        {
            return pedidoDAL.obtener(id);
        }

        //NO ES EFICIENTE NO ESTA HECHA EN LA BASE
        //SE OPTIMIZA A AGREGANDO A LA CONSULTA DE OBTENER POR ID 
        //UN LEFT JOIN CON LA TABLA PED ART FIL
        //Obtiene el pedido por id, trae los articulos con los filtros seleccionados
        public Pedido obtenerPedidoConFiltrosSeleccionados(int id)
        {
            Pedido p = obtener(id);
            List<ArticuloCantidad> listaConFiltros = obtenerFiltrosSeleccionados(p);
            if (p != null) {
                foreach (ArticuloCantidad ac in p.ProductosPedidos)
                {
                    ac.Articulo.Filtros = new List<Filtro>();
                }
                foreach (ArticuloCantidad ac in p.ProductosPedidos)
                {
                    foreach (ArticuloCantidad ac2 in listaConFiltros)
                    {
                        if (ac.Id == ac2.Id)
                        {
                            for(int i=0; i< ac2.Articulo.Filtros.Count; i++)
                            {
                                ac.Articulo.Filtros.Add(ac2.Articulo.Filtros.ElementAt(i));
                            }
                        }
                    }
                }                
            }
            
            return p;
        }
        //Obtiene una lista de las lineas del pedido incluyendo su id
        //y por cada articulo los filtros seleccionados
        public List<ArticuloCantidad> obtenerFiltrosSeleccionados(Pedido pedido)
        {
            return pedidoDAL.obtenerFiltrosSeleccionados(pedido);
        }

        public bool actualizar(Pedido ped)
        {
            return pedidoDAL.actualizar(ped);
        }

        public int registrar(Pedido ped, int idUsuario, string tipoUsuario)
        {
            int idPedidoGenerado = pedidoDAL.registrar(ped);

            if (tipoUsuario.Equals("Administrador"))
            {
                AdministradorBL administradorBL = new AdministradorBL();
                Administrador a = administradorBL.obtener(idUsuario);
                administradorBL.registrarPedidoEnConstruccion(a, idPedidoGenerado);
            }
            else if (tipoUsuario.Equals("Cliente"))
            {
                ClienteBL clienteBL = new ClienteBL();
                Cliente c = clienteBL.obtener(idUsuario);
                clienteBL.registrarPedidoEnConstruccion(c, idPedidoGenerado);
            }

            return idPedidoGenerado;
        }

        public bool eliminar(int id)
        {
            return pedidoDAL.eliminar(id);
        }
        public int obtenerCantidadSinConfirmar()
        {
            return pedidoDAL.obtenerCantidadSinConfirmar();
        }

        public List<Pedido> obtenerSinConfirmar()
        {
            return pedidoDAL.obtenerSinConfirmar();
        }

        public List<Pedido> obtenerPorClienteSinContarEnConstruccion(int id)
        {
            return pedidoDAL.obtenerPorClienteSinContarEnConstruccion(id);
        }

        public void confirmar(int id)
        {
            pedidoDAL.confirmar(id);
        }

        public void setearTotal(Pedido p)
        {
            double total = 0;

            foreach (ArticuloCantidad ac in p.ProductosPedidos)
            {
                total += ac.Cantidad * ac.PrecioUnitario;
            }

            if (p.DescuentoCliente > 0 && total > 0)
            {
                total -= total * p.DescuentoCliente / 100;
            }

            p.PrecioTotal = total;
        }

        public void cancelar(int id)
        {
            pedidoDAL.cancelar(id);
        }
    }
}
