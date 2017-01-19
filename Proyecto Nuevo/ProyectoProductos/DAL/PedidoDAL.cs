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
        //LOGICA REVISADA 12/01/17
        public List<Pedido> obtenerTodosSinContarEnConstruccion()
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
                        C.TelefonoContacto, C.EmailContacto, C.Imagen, C.EnConstruccion,

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
	                    E.Nombre <> 'EN CONSTRUCCION'
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

                            string comentario = "";
                            int enConstruccion = 0;
                            string nombreFantasia = "";
                            string rut = "";
                            string razonSocial = "";
                            string diasDePago = "";
                            string direccion = "";
                            string telefono = "";
                            string nombreContacto = "";
                            string telefonoContacto = "";
                            string emailContacto = "";
                            DateTime fechaRealizado = new DateTime();
                            DateTime fechaEntregaSolicitada = new DateTime();

                            if (dr["Comentario"] != DBNull.Value)
                            {
                                comentario = dr["Comentario"].ToString();
                            }
                            if (dr["NombreFantasia"] != DBNull.Value)
                            {
                                nombreFantasia = dr["NombreFantasia"].ToString();
                            }
                            if (dr["Rut"] != DBNull.Value)
                            {
                                rut = dr["Rut"].ToString();
                            }
                            if (dr["RazonSocial"] != DBNull.Value)
                            {
                                razonSocial = dr["RazonSocial"].ToString();
                            }
                            if (dr["DiasDePago"] != DBNull.Value)
                            {
                                diasDePago = dr["DiasDePago"].ToString();
                            }
                            if (dr["Direccion"] != DBNull.Value)
                            {
                                direccion = dr["Direccion"].ToString();
                            }
                            if (dr["Telefono"] != DBNull.Value)
                            {
                                telefono = dr["Telefono"].ToString();
                            }
                            if (dr["NombreContacto"] != DBNull.Value)
                            {
                                nombreContacto = dr["NombreContacto"].ToString();
                            }
                            if (dr["TelefonoContacto"] != DBNull.Value)
                            {
                                telefonoContacto = dr["TelefonoContacto"].ToString();
                            }
                            if (dr["EmailContacto"] != DBNull.Value)
                            {
                                emailContacto = dr["EmailContacto"].ToString();
                            }
                            if (dr["EnConstruccion"] != DBNull.Value)
                            {
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);
                            }
                            if (dr["FechaRealizado"] != DBNull.Value)
                            {
                                fechaRealizado = Convert.ToDateTime(dr["FechaRealizado"]);
                            }
                            if (dr["FechaEntregaSolicitada"] != DBNull.Value)
                            {
                                fechaEntregaSolicitada = Convert.ToDateTime(dr["FechaEntregaSolicitada"]);
                            }

                            Pedido ped = new Pedido
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                FechaRealizado = fechaRealizado,
                                FechaEntregaSolicitada = fechaEntregaSolicitada,
                                DescuentoCliente = Convert.ToDouble(dr["DescuentoCliente"]),
                                Comentario = comentario,
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
                                    NombreFantasia = nombreFantasia,
                                    Rut = rut,
                                    RazonSocial = razonSocial,
                                    Descuento = Convert.ToDouble(dr["DescuentoClienteActual"]),
                                    DiasDePago = diasDePago,
                                    Direccion = direccion,
                                    Telefono = telefono,
                                    NombreDeContacto = nombreContacto,
                                    TelefonoDeContacto = telefonoContacto,
                                    EmailDeContacto = emailContacto,
                                    Foto = dr["Imagen"].ToString(),
                                    IdPedidoEnConstruccion = enConstruccion
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

        public object obtenerCantidadProductos(int id)
        {
            try
            {
                string obtenerCantidadProductos = "SELECT COUNT(*) FROM PEDIDO P, PEDIDO_ARTICULO PA WHERE P.Id = PA.IdPedido AND P.Id = @Id; ";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(obtenerCantidadProductos, con);

                    cmd.Parameters.AddWithValue("@Id", id);

                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        //LOGICA REVISADA 12/01/17
        public void cancelar(int id)
        {
            try
            {
                string cadenaObtenerIdEstadoCancelado = "SELECT Id FROM ESTADO WHERE Nombre = 'CANCELADO';";
                string cadenaUpdate = "UPDATE Pedido SET IdEstado = @IdEstado WHERE Id = @Id;";
                int idEstado = 0;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(cadenaObtenerIdEstadoCancelado, con);

                    idEstado = (int)cmd.ExecuteScalar();

                    cmd.CommandText = cadenaUpdate;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@IdEstado", idEstado);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        //LOGICA 14/01/17
        public Pedido obtener(int id)
        {
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
                        C.TelefonoContacto, C.EmailContacto, C.Imagen as ImagenCliente, C.EnConstruccion,

                        PA.Id as IdPedidoCantidad, PA.Cantidad, PA.PrecioUnitario,

                        A.Id as IdArticulo, A.Codigo, A.Nombre as NombreArticulo, A.Descripcion as DescripcionArticulo,
                        A.Precio as PrecioArticuloActual, A.Stock, A.Disponible, A.Destacado,

                        I.Imagen as ImagenArticulo,

                        F.Id as IdFiltro, F.Nombre as NombreFiltro, F.Color as Color

                    FROM
                        PEDIDO P,
                        ESTADO E,
                        CLIENTE C,
                        PEDIDO_ARTICULO PA,
                        ARTICULO A,
                        IMAGEN I,
                        FILTRO F,
                        FILTRO_ARTICULO FA
                    WHERE
                        P.IdEstado = E.Id AND
                        P.IdCliente = C.Id AND
                        PA.IdPedido = P.Id AND
                        PA.IdArticulo = A.Id AND
                        I.IdArticulo = A.Id AND
                        FA.IdArticulo = A.Id AND
                        F.Id = FA.IdFiltro AND
                        P.Id = @Id
                    ORDER BY
                        IdPedidoCantidad, IdFiltro
                    ", con);

                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int ultimoIdLinea = 0;
                        int ultimoIdFiltro = 0;
                        bool primera = true;
                        while (dr.Read())                           
                        {
                            if (primera) {
                                string comentario = "";
                                int enConstruccion = 0;
                                string nombreFantasia = "";
                                string rut = "";
                                string razonSocial = "";
                                string diasDePago = "";
                                string direccion = "";
                                string telefono = "";
                                string nombreContacto = "";
                                string telefonoContacto = "";
                                string emailContacto = "";
                                DateTime fechaRealizado = new DateTime();
                                DateTime fechaEntregaSolicitada = new DateTime();

                                if (dr["Comentario"] != DBNull.Value)
                                {
                                    comentario = dr["Comentario"].ToString();
                                }
                                if (dr["NombreFantasia"] != DBNull.Value)
                                {
                                    nombreFantasia = dr["NombreFantasia"].ToString();
                                }
                                if (dr["Rut"] != DBNull.Value)
                                {
                                    rut = dr["Rut"].ToString();
                                }
                                if (dr["RazonSocial"] != DBNull.Value)
                                {
                                    razonSocial = dr["RazonSocial"].ToString();
                                }
                                if (dr["DiasDePago"] != DBNull.Value)
                                {
                                    diasDePago = dr["DiasDePago"].ToString();
                                }
                                if (dr["Direccion"] != DBNull.Value)
                                {
                                    direccion = dr["Direccion"].ToString();
                                }
                                if (dr["Telefono"] != DBNull.Value)
                                {
                                    telefono = dr["Telefono"].ToString();
                                }
                                if (dr["NombreContacto"] != DBNull.Value)
                                {
                                    nombreContacto = dr["NombreContacto"].ToString();
                                }
                                if (dr["TelefonoContacto"] != DBNull.Value)
                                {
                                    telefonoContacto = dr["TelefonoContacto"].ToString();
                                }
                                if (dr["EmailContacto"] != DBNull.Value)
                                {
                                    emailContacto = dr["EmailContacto"].ToString();
                                }
                                if (dr["EnConstruccion"] != DBNull.Value)
                                {
                                    enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);
                                }
                                if (dr["FechaRealizado"] != DBNull.Value)
                                {
                                    fechaRealizado = Convert.ToDateTime(dr["FechaRealizado"]);
                                }
                                if (dr["FechaEntregaSolicitada"] != DBNull.Value)
                                {
                                    fechaEntregaSolicitada = Convert.ToDateTime(dr["FechaEntregaSolicitada"]);
                                }

                                List<Imagen> imagenes = new List<Imagen>();
                                Imagen img = new Imagen
                                {
                                    Img = dr["ImagenArticulo"].ToString(),
                                };
                                imagenes.Add(img);

                                pedido = new Pedido
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    FechaRealizado = fechaRealizado,
                                    FechaEntregaSolicitada = fechaEntregaSolicitada,
                                    DescuentoCliente = Convert.ToDouble(dr["DescuentoCliente"]),
                                    Comentario = comentario,
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
                                        NombreFantasia = nombreFantasia,
                                        Rut = rut,
                                        RazonSocial = razonSocial,
                                        Descuento = Convert.ToDouble(dr["DescuentoClienteActual"]),
                                        DiasDePago = diasDePago,
                                        Direccion = direccion,
                                        Telefono = telefono,
                                        NombreDeContacto = nombreContacto,
                                        TelefonoDeContacto = telefonoContacto,
                                        EmailDeContacto = emailContacto,
                                        Foto = dr["ImagenCliente"].ToString(),
                                        IdPedidoEnConstruccion = enConstruccion
                                    },
                                    ProductosPedidos = new List<ArticuloCantidad>()
                                };

                                ArticuloCantidad ac = new ArticuloCantidad
                                {
                                    Id= Convert.ToInt32(dr["IdPedidoCantidad"]),
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

                                pedido.ProductosPedidos.Add(ac);
                                ultimoIdLinea = Convert.ToInt32(dr["IdPedidoCantidad"]);
                                primera = false;
                            }

                            

                            if (!primera && ultimoIdLinea != Convert.ToInt32(dr["IdPedidoCantidad"]))
                            {                               
                                List<Imagen> imagenes = new List<Imagen>();
                                Imagen img = new Imagen
                                {
                                    Img = dr["ImagenArticulo"].ToString(),
                                };
                                imagenes.Add(img);                               

                                ArticuloCantidad ac = new ArticuloCantidad
                                {
                                    Id = Convert.ToInt32(dr["IdPedidoCantidad"]),
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

                                pedido.ProductosPedidos.Add(ac);                
                            }

                            if (ultimoIdFiltro != Convert.ToInt32(dr["IdFiltro"]) || ultimoIdLinea != Convert.ToInt32(dr["IdPedidoCantidad"]))
                            {
                                Filtro f = new Filtro
                                {
                                    Id = Convert.ToInt32(dr["IdFiltro"]),
                                    Nombre = Convert.ToString(dr["NombreFiltro"]),
                                    Color = Convert.ToBoolean(Convert.ToInt32(dr["Color"]))
                                };
                                pedido.ProductosPedidos.ElementAt(pedido.ProductosPedidos.Count - 1).Articulo.Filtros.Add(f);
                                if (ultimoIdFiltro != Convert.ToInt32(dr["IdFiltro"]))
                                {
                                    ultimoIdFiltro = Convert.ToInt32(dr["IdFiltro"]);
                                }
                            }
                            if (!primera && ultimoIdLinea != Convert.ToInt32(dr["IdPedidoCantidad"]))
                            {
                                ultimoIdLinea = Convert.ToInt32(dr["IdPedidoCantidad"]);
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
            return pedido;
        }

        public List<ArticuloCantidad> obtenerFiltrosSeleccionados(Pedido pedido) {
            List<ArticuloCantidad> lista = new List<ArticuloCantidad>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(@"Select paf.* from Pedido_Articulo_Filtro paf, PEDIDO_ARTICULO pa where paf.IdPedidoArticulo=pa.Id and pa.idPedido=@idPedido;", con))
                    {
                        
                        if (pedido != null)
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@idPedido", pedido.Id);
                            SqlDataReader dr = cmd.ExecuteReader();
                            int ultimoId = 0;
                            while (dr.Read())
                            {
                                if (ultimoId != Convert.ToInt32(dr["idPedidoArticulo"]))
                                {
                                    List<Filtro> filtros = new List<Filtro>();
                                    Filtro f = new Filtro
                                    {
                                        Id = Convert.ToInt32(dr["idFiltro"])
                                    };
                                    filtros.Add(f);
                                    ArticuloCantidad ac = new ArticuloCantidad
                                    {
                                        Id = Convert.ToInt32(dr["idPedidoArticulo"]),
                                        Articulo = new Articulo
                                        {
                                            Filtros = filtros
                                        }
                                    };
                                    lista.Add(ac);
                                    ultimoId = Convert.ToInt32(dr["idPedidoArticulo"]);
                                }
                                else {
                                    Filtro f = new Filtro
                                    {
                                        Id = Convert.ToInt32(dr["idFiltro"])
                                    };
                                    lista.ElementAt(lista.Count - 1).Articulo.Filtros.Add(f);
                                }
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ProyectoException("Error: " + ex.Message);
            }
            return lista;
        }
           
        //LOGICA REVISADA 12/01/17
        public List<Pedido> obtenerPorClienteSinContarEnConstruccion(int id)
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
                        C.TelefonoContacto, C.EmailContacto, C.Imagen, C.EnConstruccion,

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
	                    E.Nombre <> 'EN CONSTRUCCION' AND
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
                            string comentario = "";
                            int enConstruccion = 0;
                            string nombreFantasia = "";
                            string rut = "";
                            string razonSocial = "";
                            string diasDePago = "";
                            string direccion = "";
                            string telefono = "";
                            string nombreContacto = "";
                            string telefonoContacto = "";
                            string emailContacto = "";
                            DateTime fechaRealizado = new DateTime();
                            DateTime fechaEntregaSolicitada = new DateTime();

                            if (dr["Comentario"] != DBNull.Value)
                            {
                                comentario = dr["Comentario"].ToString();
                            }
                            if (dr["NombreFantasia"] != DBNull.Value)
                            {
                                nombreFantasia = dr["NombreFantasia"].ToString();
                            }
                            if (dr["Rut"] != DBNull.Value)
                            {
                                rut = dr["Rut"].ToString();
                            }
                            if (dr["RazonSocial"] != DBNull.Value)
                            {
                                razonSocial = dr["RazonSocial"].ToString();
                            }
                            if (dr["DiasDePago"] != DBNull.Value)
                            {
                                diasDePago = dr["DiasDePago"].ToString();
                            }
                            if (dr["Direccion"] != DBNull.Value)
                            {
                                direccion = dr["Direccion"].ToString();
                            }
                            if (dr["Telefono"] != DBNull.Value)
                            {
                                telefono = dr["Telefono"].ToString();
                            }
                            if (dr["NombreContacto"] != DBNull.Value)
                            {
                                nombreContacto = dr["NombreContacto"].ToString();
                            }
                            if (dr["TelefonoContacto"] != DBNull.Value)
                            {
                                telefonoContacto = dr["TelefonoContacto"].ToString();
                            }
                            if (dr["EmailContacto"] != DBNull.Value)
                            {
                                emailContacto = dr["EmailContacto"].ToString();
                            }
                            if (dr["EnConstruccion"] != DBNull.Value)
                            {
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);
                            }
                            if (dr["FechaRealizado"] != DBNull.Value)
                            {
                                fechaRealizado = Convert.ToDateTime(dr["FechaRealizado"]);
                            }
                            if (dr["FechaEntregaSolicitada"] != DBNull.Value)
                            {
                                fechaEntregaSolicitada = Convert.ToDateTime(dr["FechaEntregaSolicitada"]);
                            }

                            Pedido ped = new Pedido
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                FechaRealizado = fechaRealizado,
                                FechaEntregaSolicitada = fechaEntregaSolicitada,
                                DescuentoCliente = Convert.ToDouble(dr["DescuentoCliente"]),
                                Comentario = comentario,
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
                                    NombreFantasia = nombreFantasia,
                                    Rut = rut,
                                    RazonSocial = razonSocial,
                                    Descuento = Convert.ToDouble(dr["DescuentoClienteActual"]),
                                    DiasDePago = diasDePago,
                                    Direccion = direccion,
                                    Telefono = telefono,
                                    NombreDeContacto = nombreContacto,
                                    TelefonoDeContacto = telefonoContacto,
                                    EmailDeContacto = emailContacto,
                                    Foto = dr["Imagen"].ToString(),
                                    IdPedidoEnConstruccion = enConstruccion
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

        //LOGICA REVISADA 12/01/17
        public bool actualizar(Pedido ped)//VER QUE NO SE ACTUALIZA EL IVA CON EL VALOR DEL MOMENTO (BD) SINO QUE SE UTILIZA EL QUE VENIA EN EL PEDIDO
        {
            string cadenaDeletePedidoArticulo = "DELETE FROM PEDIDO_ARTICULO WHERE IdPedido = @Id;";
            string cadenaInsertPedidoArticulo = "INSERT INTO PEDIDO_ARTICULO VALUES (@IdPedido, @IdArticulo, @Cantidad, @PrecioUnitario); SELECT CAST(Scope_Identity() AS INT);";
            string cadenaUpdatePedido = "UPDATE PEDIDO SET FechaRealizado = @FechaRealizado, FechaEntregaSolicitada = @FechaEntregaSolicitada, DescuentoCliente = @DescuentoCliente, Iva = @Iva, IdCliente = @IdCliente, Comentario = @Comentario, IdEstado = @IdEstado WHERE Id = @Id;";
            string cadenaSelectIdPedidoArticulo = @"select paf.id from PEDIDO_ARTICULO_FILTRO paf, PEDIDO_ARTICULO pa, PEDIDO p
                                                    where paf.IdPedidoArticulo=pa.Id and pa.IdPedido=p.Id and p.Id=@IdPedido";
            string cadenaDeletePedidoArticuloFiltro = "DELETE FROM PEDIDO_ARTICULO_FILTRO WHERE Id = @Id;";
            string cadenaInsertPedidoArticuloFiltro = "INSERT INTO PEDIDO_ARTICULO_FILTRO VALUES (@IdPedidoArticulo, @IdFiltro);";
            SqlTransaction trn = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(cadenaSelectIdPedidoArticulo, con))
                    {
                        
                        con.Open();
                        trn = con.BeginTransaction();
                        cmd.Transaction = trn;

                        cmd.Parameters.AddWithValue("@IdPedido", ped.Id);
                        SqlDataReader dr = cmd.ExecuteReader();
                        List<int> listaIdPedidoArticulo = new List<int>();
                        while (dr.Read()) {
                            listaIdPedidoArticulo.Add(Convert.ToInt32(dr["id"]));                            
                        }
                        dr.Close();
                        cmd.CommandText = cadenaDeletePedidoArticuloFiltro;
                        foreach (int i in listaIdPedidoArticulo) {                            
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Id", i);
                            cmd.ExecuteNonQuery();
                        }

                        cmd.CommandText = cadenaDeletePedidoArticulo;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Id", ped.Id);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.CommandText = cadenaInsertPedidoArticulo;
                        foreach (ArticuloCantidad ac in ped.ProductosPedidos)
                        {
                            cmd.Parameters.AddWithValue("@IdPedido", ped.Id);
                            cmd.Parameters.AddWithValue("@IdArticulo", ac.Articulo.Id);
                            cmd.Parameters.AddWithValue("@Cantidad", ac.Cantidad);
                            cmd.Parameters.AddWithValue("@PrecioUnitario", ac.Articulo.Precio);
                            ac.Id = (int)cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                        }

                        cmd.Parameters.Clear();
                        cmd.CommandText = cadenaUpdatePedido;
                        cmd.Parameters.AddWithValue("@Id", ped.Id);
                        if(ped.Estado.Nombre.Equals("EN CONSTRUCCION"))
                        {
                            cmd.Parameters.AddWithValue("@FechaRealizado", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@FechaRealizado", ped.FechaRealizado);
                        }

                        if (ped.FechaEntregaSolicitada == new DateTime())
                        {
                            cmd.Parameters.AddWithValue("@FechaEntregaSolicitada", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@FechaEntregaSolicitada", ped.FechaEntregaSolicitada);
                        }
                        cmd.Parameters.AddWithValue("@DescuentoCliente", ped.Cliente.Descuento);
                        cmd.Parameters.AddWithValue("@Iva", ped.Iva);
                        cmd.Parameters.AddWithValue("@IdCliente", ped.Cliente.Id);
                        if (ped.Comentario == null || ped.Comentario.Equals(""))
                        {
                            cmd.Parameters.AddWithValue("@Comentario", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Comentario", ped.Comentario);
                        }
                        cmd.Parameters.AddWithValue("@IdEstado", ped.Estado.Id);
                        cmd.ExecuteNonQuery();


                        

                        cmd.CommandText = cadenaInsertPedidoArticuloFiltro;
                        cmd.Parameters.Clear();
                        foreach (ArticuloCantidad ac in ped.ProductosPedidos)
                        {
                            foreach (Filtro f in ac.Articulo.Filtros) {
                                cmd.Parameters.AddWithValue("@IdPedidoArticulo", ac.Id);
                                cmd.Parameters.AddWithValue("@IdFiltro", f.Id);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }                                                      
                        }

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

        //LOGICA REVISADA 12/01/17
        public int registrar(Pedido ped)
        {
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
                        cmd.Parameters.AddWithValue("@FechaRealizado", DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaEntregaSolicitada", DBNull.Value);
                        cmd.Parameters.AddWithValue("@DescuentoCliente", ped.Cliente.Descuento);
                        cmd.Parameters.AddWithValue("@Iva", ped.Iva);
                        cmd.Parameters.AddWithValue("@IdCliente", ped.Cliente.Id);
                        cmd.Parameters.AddWithValue("@Comentario", DBNull.Value);
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
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        //NO IMPLEMENTADO
        public bool eliminar(int id)
        {
            throw new NotImplementedException();
        }

        //LOGICA REVISADA 12/01/17
        public int obtenerCantidadSinConfirmar()
        {
            int pedidos = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pedido P, Estado E WHERE P.IdEstado = E.Id AND E.Nombre = 'CONFIRMADO POR CLIENTE';", con);
                    pedidos = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return pedidos;
        }

        //LOGICA REVISADA 12/01/17
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
                        C.TelefonoContacto, C.EmailContacto, C.Imagen, C.EnConstruccion,

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
	                    E.Nombre = 'CONFIRMADO POR CLIENTE'
                    ORDER BY
	                    Id,
	                    IdArticulo
                    ", con);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            string comentario = "";
                            int enConstruccion = 0;
                            string nombreFantasia = "";
                            string rut = "";
                            string razonSocial = "";
                            string diasDePago = "";
                            string direccion = "";
                            string telefono = "";
                            string nombreContacto = "";
                            string telefonoContacto = "";
                            string emailContacto = "";
                            DateTime fechaRealizado = new DateTime();
                            DateTime fechaEntregaSolicitada = new DateTime();

                            if (dr["Comentario"] != DBNull.Value)
                            {
                                comentario = dr["Comentario"].ToString();
                            }
                            if (dr["NombreFantasia"] != DBNull.Value)
                            {
                                nombreFantasia = dr["NombreFantasia"].ToString();
                            }
                            if (dr["Rut"] != DBNull.Value)
                            {
                                rut = dr["Rut"].ToString();
                            }
                            if (dr["RazonSocial"] != DBNull.Value)
                            {
                                razonSocial = dr["RazonSocial"].ToString();
                            }
                            if (dr["DiasDePago"] != DBNull.Value)
                            {
                                diasDePago = dr["DiasDePago"].ToString();
                            }
                            if (dr["Direccion"] != DBNull.Value)
                            {
                                direccion = dr["Direccion"].ToString();
                            }
                            if (dr["Telefono"] != DBNull.Value)
                            {
                                telefono = dr["Telefono"].ToString();
                            }
                            if (dr["NombreContacto"] != DBNull.Value)
                            {
                                nombreContacto = dr["NombreContacto"].ToString();
                            }
                            if (dr["TelefonoContacto"] != DBNull.Value)
                            {
                                telefonoContacto = dr["TelefonoContacto"].ToString();
                            }
                            if (dr["EmailContacto"] != DBNull.Value)
                            {
                                emailContacto = dr["EmailContacto"].ToString();
                            }
                            if (dr["EnConstruccion"] != DBNull.Value)
                            {
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);
                            }
                            if (dr["FechaRealizado"] != DBNull.Value)
                            {
                                fechaRealizado = Convert.ToDateTime(dr["FechaRealizado"]);
                            }
                            if (dr["FechaEntregaSolicitada"] != DBNull.Value)
                            {
                                fechaEntregaSolicitada = Convert.ToDateTime(dr["FechaEntregaSolicitada"]);
                            }

                            Pedido ped = new Pedido
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                FechaRealizado = fechaRealizado,
                                FechaEntregaSolicitada = fechaEntregaSolicitada,
                                DescuentoCliente = Convert.ToDouble(dr["DescuentoCliente"]),
                                Comentario = comentario,
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
                                    NombreFantasia = nombreFantasia,
                                    Rut = rut,
                                    RazonSocial = razonSocial,
                                    Descuento = Convert.ToDouble(dr["DescuentoClienteActual"]),
                                    DiasDePago = diasDePago,
                                    Direccion = direccion,
                                    Telefono = telefono,
                                    NombreDeContacto = nombreContacto,
                                    TelefonoDeContacto = telefonoContacto,
                                    EmailDeContacto = emailContacto,
                                    Foto = dr["Imagen"].ToString(),
                                    IdPedidoEnConstruccion = enConstruccion
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

        //REVISADO 12/01/17
        public void confirmar(int id)
        {
            try
            {
                string cadenaObtenerIdEstadoRealizado = "SELECT Id FROM ESTADO WHERE Nombre = 'REALIZADO';";
                string cadenaUpdate = "UPDATE Pedido SET IdEstado = @IdEstado WHERE Id = @Id;";
                int idEstado = 0;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(cadenaObtenerIdEstadoRealizado, con);
                    
                    idEstado = (int)cmd.ExecuteScalar();

                    cmd.CommandText = cadenaUpdate;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@IdEstado", idEstado);
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
