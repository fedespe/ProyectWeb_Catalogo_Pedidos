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
	                    PA.IdArticulo = A.Id
                    ORDER BY
	                    Id,
	                    IdArticulo,
                        FechaRealizado desc,
                        FechaEntregaSolicitada desc
                    ;", con);

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
                                },

                                Cliente = new Cliente
                                {
                                    Id = Convert.ToInt32(dr["IdCliente"]),
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

                            ArticuloCantidad ac = new ArticuloCantidad
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

                            pedidos.Add(ped);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            if (pedidos.Count() >= 1)
            {
                int i = 1;
                while (i < pedidos.Count())
                {
                    if (pedidos[i].Id == pedidos[i - 1].Id)
                    {
                        pedidos[i - 1].ProductosPedidos.Add(pedidos[i].ProductosPedidos[0]);
                        pedidos.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            foreach (Pedido p in pedidos)
            {
                double precioTot = 0;

                foreach (ArticuloCantidad acp in p.ProductosPedidos)
                {
                    precioTot += acp.Cantidad * acp.PrecioUnitario;
                }

                precioTot -= precioTot * p.DescuentoCliente / 100;

                p.PrecioTotal = precioTot;
            }

            return pedidos;
        }

        public Pedido obtener(int id)
        {
            List<Pedido> pedidos = new List<Pedido>();
            Pedido pedido = null;

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
                        C.TelefonoContacto, C.EmailContacto, C.Imagen as ImagenCliente,

                        PA.Id as IdPedidoCantidad, PA.Cantidad, PA.PrecioUnitario,

                        A.Id as IdArticulo, A.Codigo, A.Nombre as NombreArticulo, A.Descripcion as DescripcionArticulo,
                        A.Precio as PrecioArticuloActual, A.Stock, A.Disponible, A.Destacado,

                        I.Imagen as ImagenArticulo

                    FROM
                        PEDIDO P,
                        ESTADO E,
                        CLIENTE C,
                        PEDIDO_ARTICULO PA,
                        ARTICULO A,
                        IMAGEN I
                    WHERE
                        P.IdEstado = E.Id AND
                        P.IdCliente = C.Id AND
                        PA.IdPedido = P.Id AND
                        PA.IdArticulo = A.Id AND
                        I.IdArticulo = A.Id AND
                        P.Id = @Id
                    ORDER BY
                        IdPedidoCantidad
                    ", con);

                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            List<Imagen> imagenes = new List<Imagen>();
                            Imagen img = new Imagen
                            {
                                Img = dr["ImagenArticulo"].ToString(),
                            };
                            imagenes.Add(img);

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
                                },

                                Cliente = new Cliente
                                {
                                    Id = Convert.ToInt32(dr["IdCliente"]),
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
                                    Foto = dr["ImagenCliente"].ToString()
                                },
                                ProductosPedidos = new List<ArticuloCantidad>()
                            };

                            ArticuloCantidad ac = new ArticuloCantidad
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
                                    Destacado = (Convert.ToInt32(dr["Destacado"]) == 0 ? false : true),
                                    Imagenes = imagenes
                                }
                            };

                            ped.ProductosPedidos.Add(ac);

                            pedidos.Add(ped);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            if (pedidos.Count() >= 1)
            {
                int i = 1;
                while (i < pedidos.Count())
                {
                    if (pedidos[i].ProductosPedidos[0].Articulo.Id == pedidos[i - 1].ProductosPedidos[0].Articulo.Id)
                    {
                        pedidos.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            if (pedidos.Count() >= 1)
            {
                int i = 1;
                while (i < pedidos.Count())
                {
                    if (pedidos[i].Id == pedidos[i - 1].Id)
                    {
                        pedidos[i - 1].ProductosPedidos.Add(pedidos[i].ProductosPedidos[0]);
                        pedidos.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            foreach (Pedido p in pedidos)
            {
                double precioTot = 0;

                foreach (ArticuloCantidad acp in p.ProductosPedidos)
                {
                    precioTot += acp.Cantidad * acp.PrecioUnitario;
                }

                precioTot -= precioTot * p.DescuentoCliente / 100;

                p.PrecioTotal = precioTot;
            }

            if(pedidos.Count > 0)
                pedido = pedidos[0];

            return pedido;
        }

        public List<Pedido> obtenerPorCliente(int id)
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
	                    C.Id = @Id
                    ORDER BY
	                    Id,
	                    IdArticulo
                    ", con);

                    cmd.Parameters.AddWithValue("@Id", id);

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
                                },

                                Cliente = new Cliente
                                {
                                    Id = Convert.ToInt32(dr["IdCliente"]),
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

                            ArticuloCantidad ac = new ArticuloCantidad
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

                            pedidos.Add(ped);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            if (pedidos.Count() >= 1)
            {
                int i = 1;
                while (i < pedidos.Count())
                {
                    if (pedidos[i].Id == pedidos[i - 1].Id)
                    {
                        pedidos[i - 1].ProductosPedidos.Add(pedidos[i].ProductosPedidos[0]);
                        pedidos.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            foreach (Pedido p in pedidos)
            {
                double precioTot = 0;

                foreach (ArticuloCantidad acp in p.ProductosPedidos)
                {
                    precioTot += acp.Cantidad * acp.PrecioUnitario;
                }

                precioTot -= precioTot * p.DescuentoCliente / 100;

                p.PrecioTotal = precioTot;
            }

            return pedidos;
        }

        public bool actualizar(Pedido ped)
        {
            string cadenaDeletePedidoArticulo = "DELETE FROM PEDIDO_ARTICULO WHERE IdPedido = @Id;";
            string cadenaInsertPedidoArticulo = "INSERT INTO PEDIDO_ARTICULO VALUES (@IdPedido, @IdArticulo, @Cantidad, @PrecioUnitario);";
            string cadenaUpdatePedido = "UPDATE PEDIDO SET FechaRealizado = @FechaRealizado, FechaEntregaSolicitada = @FechaEntregaSolicitada, DescuentoCliente = @DescuentoCliente, Iva = @Iva, IdCliente = @IdCliente, Comentario = @Comentario, IdEstado = @IdEstado WHERE Id = @Id;";
            SqlTransaction trn = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(cadenaDeletePedidoArticulo, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", ped.Id);
                        con.Open();
                        trn = con.BeginTransaction();
                        cmd.Transaction = trn;

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.CommandText = cadenaInsertPedidoArticulo;
                        foreach (ArticuloCantidad ac in ped.ProductosPedidos)
                        {
                            cmd.Parameters.AddWithValue("@IdPedido", ped.Id);
                            cmd.Parameters.AddWithValue("@IdArticulo", ac.Articulo.Id);
                            cmd.Parameters.AddWithValue("@Cantidad", ac.Cantidad);
                            cmd.Parameters.AddWithValue("@PrecioUnitario", ac.Articulo.Precio);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        cmd.Parameters.Clear();
                        cmd.CommandText = cadenaUpdatePedido;
                        cmd.Parameters.AddWithValue("@Id", ped.Id);
                        cmd.Parameters.AddWithValue("@FechaRealizado", ped.FechaRealizado);
                        cmd.Parameters.AddWithValue("@FechaEntregaSolicitada", ped.FechaEntregaSolicitada);
                        cmd.Parameters.AddWithValue("@DescuentoCliente", ped.Cliente.Descuento);
                        cmd.Parameters.AddWithValue("@Iva", ped.Iva);
                        cmd.Parameters.AddWithValue("@IdCliente", ped.Cliente.Id);
                        cmd.Parameters.AddWithValue("@Comentario", ped.Comentario);
                        cmd.Parameters.AddWithValue("@IdEstado", ped.Estado.Id);
                        cmd.ExecuteNonQuery();

                        trn.Commit();
                        trn.Dispose();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //if (trn != null)
                //    trn.Rollback();
                
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public int registrar(Pedido ped)
        {
            //Registrar el Pedido, retornar el ID generado para agregarle al usuario Logueado el pedido en construcción
            string cadenaInsertPedido = @"INSERT INTO PEDIDO VALUES(
                @FechaRealizado, @FechaEntregaSolicitada, @DescuentoCliente, @Iva, @IdCliente, @Comentario, @IdEstado); SELECT CAST(Scope_Identity() AS INT);";
            string cadenaInsertPedidoArticulo = "INSERT INTO PEDIDO_ARTICULO VALUES (@IdPedido, @IdArticulo, @Cantidad, @PrecioUnitario);";
            int idPedidoGenerado = 0;

            SqlTransaction trn = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(cadenaInsertPedido, con))
                    {
                        cmd.Parameters.AddWithValue("@FechaRealizado", ped.FechaRealizado);
                        if(ped.FechaEntregaSolicitada == new DateTime())
                        {
                            cmd.Parameters.AddWithValue("@FechaEntregaSolicitada", "17530101");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@FechaEntregaSolicitada", ped.FechaEntregaSolicitada);
                        }
                        cmd.Parameters.AddWithValue("@DescuentoCliente", ped.Cliente.Descuento);
                        cmd.Parameters.AddWithValue("@Iva", ped.Iva);
                        cmd.Parameters.AddWithValue("@IdCliente", ped.Cliente.Id);
                        if (ped.Comentario.Equals(""))
                        {
                            cmd.Parameters.AddWithValue("@Comentario", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Comentario", ped.Comentario);
                        }
                        
                        cmd.Parameters.AddWithValue("@IdEstado", ped.Estado.Id);
                        con.Open();
                        trn = con.BeginTransaction();
                        cmd.Transaction = trn;

                        idPedidoGenerado = (int)cmd.ExecuteScalar();
                        cmd.Parameters.Clear();

                        cmd.CommandText = cadenaInsertPedidoArticulo;
                        foreach (ArticuloCantidad ac in ped.ProductosPedidos)
                        {
                            cmd.Parameters.AddWithValue("@IdPedido", idPedidoGenerado);
                            cmd.Parameters.AddWithValue("@IdArticulo", ac.Articulo.Id);
                            cmd.Parameters.AddWithValue("@Cantidad", ac.Cantidad);
                            cmd.Parameters.AddWithValue("@PrecioUnitario", ac.Articulo.Precio);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        trn.Commit();
                        trn.Dispose();
                        return idPedidoGenerado;
                    }
                }
            }
            catch (Exception ex)
            {
                //if (trn != null)
                //    trn.Rollback();

                throw new ProyectoException("Error: " + ex.Message);
            }
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
                                },

                                Cliente = new Cliente
                                {
                                    Id = Convert.ToInt32(dr["IdCliente"]),
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

                            ArticuloCantidad ac = new ArticuloCantidad
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

                            pedidos.Add(ped);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            if(pedidos.Count() >= 1)
            {
                int i = 1;
                while(i < pedidos.Count())
                {
                    if(pedidos[i].Id == pedidos[i - 1].Id)
                    {
                        pedidos[i - 1].ProductosPedidos.Add(pedidos[i].ProductosPedidos[0]);
                        pedidos.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            foreach(Pedido p in pedidos)
            {
                double precioTot = 0;

                foreach (ArticuloCantidad acp in p.ProductosPedidos)
                {
                    precioTot += acp.Cantidad * acp.PrecioUnitario;
                }

                precioTot -= precioTot * p.DescuentoCliente / 100;

                p.PrecioTotal = precioTot;
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
