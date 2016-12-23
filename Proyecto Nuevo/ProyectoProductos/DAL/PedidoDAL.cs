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

                    SqlCommand cmd = new SqlCommand(@"
                    SELECT

	                    P.Id as Id, P.FechaRealizado, P.FechaEntregaSolicitada,
	                    P.DescuentoCliente as DescuentoCliente, P.Comentario, P.Iva, P.Comentario,

	                    E.Id as IdEstado, E.Nombre as Estado,

	                    C.Id as IdCliente, C.Usuario, C.NombreFantasia, C.Rut, C.RazonSocial,
                        C.Descuento as DescuentoClienteActual, C.DiasDePago, C.Direccion, C.Telefono, C.NombreContacto,
                        C.TelefonoContacto, C.EmailContacto, C.Imagen,

	                    PA.Id as IdPedidoCantidad, PA.Cantidad, PA.PrecioUnitario,

	                    A.Id as IdArticulo, A.Codigo, A.Nombre as NombreArticulo, A.Descripcion as DescripcionArticulo,
                        A.Precio as PrecioArticuloActual, A.Stock, A.Disponible, A.Destacado

                    FROM
	                    PEDIDO P,
	                    ESTADO E,
	                    CLIENTE C,
	                    PEDIDO_ARTICULO PA,
	                    ARTICULO A
                    WHERE
	                    P.IdEstado = E.Id AND
	                    P.IdCliente = C.Id AND
	                    PA.IdPedido = P.Id AND
	                    PA.IdArticulo = A.Id AND
	                    (E.Nombre = 'MODIFICADO POR CLIENTE' OR E.Nombre = 'NUEVO')
                    ORDER BY
	                    Id,
	                    IdArticulo
                    ", con);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int ultimoPedido = 0;
                        Pedido ped = null;
                        ArticuloCantidad ac = null;

                        while (dr.Read())
                        {
                            ultimoPedido = Convert.ToInt32(dr["Id"]);

                            ped = new Pedido
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
                                },

                                Cliente = new Cliente
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    NombreUsuario = dr["Usuario"].ToString(),
                                    NombreFantasia = dr["NombreFantasia"].ToString(),
                                    Rut = dr["Rut"].ToString(),
                                    RazonSocial = dr["RazonSocial"].ToString(),
                                    Descuento = Convert.ToDouble(dr["DescuentoClienteActual"]),
                                    DiasDePago = dr["DiasDePago"].ToString(),
                                    Direccion = dr["Direccion"].ToString(),
                                    Telefono = dr["Telefono"].ToString(),
                                    NombreDeContacto = dr["NombreContacto"].ToString(),
                                    TelefonoDeContacto = dr["TelefonoContacto"].ToString(),
                                    EmailDeContacto = dr["EmailContacto"].ToString(),
                                    Foto = dr["Imagen"].ToString()
                                },
                                ProductosPedidos = new List<ArticuloCantidad>()
                            };

                            ac = new ArticuloCantidad
                            {
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                PrecioUnitario = Convert.ToDouble(dr["PrecioUnitario"]),
                                Articulo = new Articulo
                                {
                                    Id = Convert.ToInt32(dr["IdArticulo"]),
                                    Codigo = dr["Codigo"].ToString(),
                                    Nombre = dr["NombreArticulo"].ToString(),
                                    Descripcion = dr["DescripcionArticulo"].ToString(),
                                    Precio = Convert.ToDouble(dr["PrecioArticuloActual"]),
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    Disponible = (Convert.ToInt32(dr["Disponible"]) == 0 ? false : true),
                                    Destacado = (Convert.ToInt32(dr["Destacado"]) == 0 ? false : true)
                                }
                            };

                            ped.ProductosPedidos.Add(ac);

                            while(dr.Read() && ultimoPedido == Convert.ToInt32(dr["Id"]))
                            {
                                ac = new ArticuloCantidad
                                {
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    PrecioUnitario = Convert.ToDouble(dr["PrecioUnitario"]),
                                    Articulo = new Articulo
                                    {
                                        Id = Convert.ToInt32(dr["IdArticulo"]),
                                        Codigo = dr["Codigo"].ToString(),
                                        Nombre = dr["NombreArticulo"].ToString(),
                                        Descripcion = dr["DescripcionArticulo"].ToString(),
                                        Precio = Convert.ToDouble(dr["PrecioArticuloActual"]),
                                        Stock = Convert.ToInt32(dr["Stock"]),
                                        Disponible = (Convert.ToInt32(dr["Disponible"]) == 0 ? false : true),
                                        Destacado = (Convert.ToInt32(dr["Destacado"]) == 0 ? false : true)
                                    }
                                };

                                ped.ProductosPedidos.Add(ac);

                                ultimoPedido = Convert.ToInt32(dr["Id"]);
                            }

                            double precioTot = 0;

                            foreach (ArticuloCantidad acp in ped.ProductosPedidos)
                            {
                                precioTot += acp.Cantidad * acp.PrecioUnitario;
                            }

                            precioTot -= precioTot * ped.DescuentoCliente / 100;

                            ped.PrecioTotal = precioTot;

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

        public void confirmar(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Pedido SET IdEstado = 4 WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }
    }
}
