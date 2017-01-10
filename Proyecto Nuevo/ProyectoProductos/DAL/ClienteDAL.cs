using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClienteDAL
    {
        public List<Cliente> obtenerTodos()
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int enConstruccion = 0;
                            if (dr["EnConstruccion"] != DBNull.Value)
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);

                            Cliente cli = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                NombreUsuario = dr["Usuario"].ToString(),
                                IdPedidoEnConstruccion = enConstruccion,
                                NombreFantasia = dr["NombreFantasia"].ToString(),
                                Rut = dr["Rut"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                Descuento = Convert.ToDouble(dr["Descuento"]),
                                DiasDePago = dr["DiasDePago"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                NombreDeContacto = dr["NombreContacto"].ToString(),
                                TelefonoDeContacto = dr["TelefonoContacto"].ToString(),
                                EmailDeContacto = dr["EmailContacto"].ToString(),
                                Foto = dr["Imagen"].ToString()
                            };
                            clientes.Add(cli);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return clientes;
        }

        public int obtenerPedidoEnContruccion(int id)
        {
            int enContruccion = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT EnConstruccion FROM CLIENTE WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    if(cmd.ExecuteScalar() != DBNull.Value) //Ver cómo controlar esto de otra forma... No está bueno ejecutar 2 veces
                        enContruccion = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return enContruccion;
        }

        public Cliente login(string nombre, string pass)
        {
            Cliente cli = null;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente WHERE usuario = @usu AND contrasenia=@pass", con);
                    cmd.Parameters.AddWithValue("@usu", nombre);
                    cmd.Parameters.AddWithValue("@pass", Utilidades.calculateMD5Hash(pass));

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            int enConstruccion = 0;
                            if (dr["EnConstruccion"] != DBNull.Value)
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);

                            cli = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                NombreUsuario = dr["Usuario"].ToString(),
                                IdPedidoEnConstruccion = enConstruccion,
                                NombreFantasia = dr["NombreFantasia"].ToString(),
                                Rut = dr["Rut"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                Descuento = Convert.ToDouble(dr["Descuento"]),
                                DiasDePago = dr["DiasDePago"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                NombreDeContacto = dr["NombreContacto"].ToString(),
                                TelefonoDeContacto = dr["TelefonoContacto"].ToString(),
                                EmailDeContacto = dr["EmailContacto"].ToString(),
                                Foto = dr["Imagen"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return cli;
        }

        public int obtenerPrimerCliente()
        {
            int idPrimerCliente = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT MIN(Id) FROM CLIENTE", con);

                    idPrimerCliente = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return idPrimerCliente;
        }

        public Cliente obtener(int id)
        {
            Cliente cli = null;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            int enConstruccion = 0;
                            if (dr["EnConstruccion"] != DBNull.Value)
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);

                            cli = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                NombreUsuario = dr["Usuario"].ToString(),
                                IdPedidoEnConstruccion = enConstruccion,
                                NombreFantasia = dr["NombreFantasia"].ToString(),
                                Rut = dr["Rut"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                Descuento = Convert.ToDouble(dr["Descuento"]),
                                DiasDePago = dr["DiasDePago"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                NombreDeContacto = dr["NombreContacto"].ToString(),
                                TelefonoDeContacto = dr["TelefonoContacto"].ToString(),
                                EmailDeContacto = dr["EmailContacto"].ToString(),
                                Foto = dr["Imagen"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return cli;
        }

        public bool actualizar(Cliente cli)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE Cliente SET
                        Usuario = @usu, EnConstruccion = @enConstruccion, NombreFantasia = @nomFan, Rut = @rut, RazonSocial = @razSoc, Descuento = @desc,
                        DiasDePago = @diasPago, Direccion = @dir, Telefono = @tel, NombreContacto = @nomCon,
                        TelefonoContacto = @telCon, EmailContacto = @emailCon, Imagen = @foto
                        WHERE id = @id", con);

                    if(cli.IdPedidoEnConstruccion > 0)
                    {
                        cmd.Parameters.AddWithValue("@enConstruccion", cli.IdPedidoEnConstruccion);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@enConstruccion", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@usu", cli.NombreUsuario);
                    cmd.Parameters.AddWithValue("@id", cli.Id);
                    cmd.Parameters.AddWithValue("@nomFan", cli.NombreFantasia);
                    cmd.Parameters.AddWithValue("@rut", cli.Rut);
                    cmd.Parameters.AddWithValue("@razSoc", cli.RazonSocial);
                    cmd.Parameters.AddWithValue("@desc", cli.Descuento);
                    cmd.Parameters.AddWithValue("@diasPago", cli.DiasDePago);
                    cmd.Parameters.AddWithValue("@dir", cli.Direccion);
                    cmd.Parameters.AddWithValue("@tel", cli.Telefono);
                    cmd.Parameters.AddWithValue("@nomCon", cli.NombreDeContacto);
                    cmd.Parameters.AddWithValue("@telCon", cli.TelefonoDeContacto);
                    cmd.Parameters.AddWithValue("@emailCon", cli.EmailDeContacto);
                    cmd.Parameters.AddWithValue("@foto", cli.Foto);

                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public bool actualizarPassword(Cliente cli)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Cliente SET Contrasenia = @pass WHERE id = @id", con);

                    cmd.Parameters.AddWithValue("@pass", Utilidades.calculateMD5Hash(cli.Password));
                    cmd.Parameters.AddWithValue("@id", cli.Id);

                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public void registrar(Cliente cli)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Cliente VALUES(
                        @usu, @pass, null,@nomFan, @rut, @razSoc, @desc, @diasPago, @dir, @tel, @nomCon, @telCon, @emailCon, @foto)"
                        , con);

                    cmd.Parameters.AddWithValue("@usu", cli.NombreUsuario);
                    cmd.Parameters.AddWithValue("@pass", Utilidades.calculateMD5Hash(cli.Password));
                    cmd.Parameters.AddWithValue("@nomFan", cli.NombreFantasia);
                    cmd.Parameters.AddWithValue("@rut", cli.Rut);
                    cmd.Parameters.AddWithValue("@razSoc", cli.RazonSocial);
                    cmd.Parameters.AddWithValue("@desc", cli.Descuento);
                    cmd.Parameters.AddWithValue("@diasPago", cli.DiasDePago);
                    cmd.Parameters.AddWithValue("@dir", cli.Direccion);
                    cmd.Parameters.AddWithValue("@tel", cli.Telefono);
                    cmd.Parameters.AddWithValue("@nomCon", cli.NombreDeContacto);
                    cmd.Parameters.AddWithValue("@telCon", cli.TelefonoDeContacto);
                    cmd.Parameters.AddWithValue("@emailCon", cli.EmailDeContacto);
                    cmd.Parameters.AddWithValue("@foto", cli.Foto);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public bool eliminar(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Pedido WHERE IdCliente = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            throw new ProyectoException("No puede eliminar un cliente que tiene pedidos asociados.");
                        }
                    }

                    cmd.CommandText = @"DELETE FROM Cliente WHERE id = @id";
                    
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException(ex.Message);
            }
        }
    }
}
