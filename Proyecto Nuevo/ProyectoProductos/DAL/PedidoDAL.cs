using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class PedidoDAL
    {
        public List<Pedido> obtenerTodos()
        {
            throw new NotImplementedException();
        }

        public Pedido obtener(int id)
        {
            throw new NotImplementedException();
        }

        public bool actualizar(Pedido ped)
        {
            throw new NotImplementedException();
        }

        public void registrar(Pedido ped)
        {
            throw new NotImplementedException();
        }

        public bool eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public int obtenerCantidadSinConfirmar()
        {
            int pedidos = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pedido P, Estado E WHERE P.IdEstado = E.Id AND (E.Nombre = 'MODIFICADO POR CLIENTE' OR E.Nombre = 'NUEVO')", con);
                    pedidos = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return pedidos;
        }

        public List<Pedido> obtenerSinConfirmar()
        {
            List<Pedido> pedidos = new List<Pedido>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT
                        P.Id as Id, P.FechaRealizado as FechaRealizado, P.FechaEntregaSolicitada as FechaEntregaSolicitada,
                        P.DescuentoCliente as DescuentoCliente, P.Comentario as Comentario, P.Iva as Iva, P.Comentario as Comentario,
                        E.Id as IdEstado, E.Nombre as Estado
                        FROM Pedido P, Estado E WHERE P.IdEstado = E.Id AND (E.Nombre = 'MODIFICADO POR CLIENTE' OR E.Nombre = 'NUEVO')", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Pedido ped = new Pedido
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                FechaRealizado = Convert.ToDateTime(dr["FechaRealizado"]),
                                FechaEntregaSolicitada = Convert.ToDateTime(dr["FechaEntregaSolicitada"]),
                                DescuentoCliente = Convert.ToDouble(dr["DescuentoCliente"]),
                                Comentario = dr["Comentario"].ToString(),
                                Iva = Convert.ToDouble(dr["Iva"]),
                                Estado = new EstadoPedido
                                {
                                    Id = Convert.ToInt32(dr["IdEstado"]),
                                    Nombre = dr["Estado"].ToString(),
                                }
                                //Falta agregar el CLiente, Productos (con sus cantidades) y precio total... Largo por acá porque estoy con sueño ya...
                            };
                            pedidos.Add(ped);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return pedidos;
        }
    }
}
