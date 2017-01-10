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
    public class AdministradorDAL
    {
        public List<Administrador> obtenerTodos()
        {
            List<Administrador> administradores = new List<Administrador>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Administrador", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int enConstruccion = 0;
                            if (dr["EnConstruccion"] != DBNull.Value)
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);

                            Administrador admin = new Administrador
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                NombreUsuario = dr["Usuario"].ToString(),
                                IdPedidoEnConstruccion = enConstruccion
                            };
                            administradores.Add(admin);
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return administradores;
        }

        public int obtenerPedidoEnContruccion(int id)
        {
            int enContruccion = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT EnConstruccion FROM ADMINISTRADOR WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    if (cmd.ExecuteScalar() != DBNull.Value) //Ver cómo controlar esto de otra forma... No está bueno ejecutar 2 veces
                        enContruccion = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return enContruccion;
        }

        public Administrador login(string nombre, string pass)
        {
            Administrador admin = null;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM administrador WHERE usuario = @usu AND contrasenia=@pass", con);
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

                            admin = new Administrador
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                NombreUsuario = dr["Usuario"].ToString(),
                                IdPedidoEnConstruccion = enConstruccion
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return admin;
        }
        public Administrador obtener(int id)
        {
            Administrador admin = null;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM administrador WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            int enConstruccion = 0;
                            if (dr["EnConstruccion"] != DBNull.Value)
                                enConstruccion = Convert.ToInt32(dr["EnConstruccion"]);

                            admin = new Administrador
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                NombreUsuario = dr["Usuario"].ToString(),
                                IdPedidoEnConstruccion = enConstruccion
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return admin;
        }

        public bool actualizar(Administrador admin)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Administrador SET Usuario = @usu, EnConstruccion = @enConstruccion WHERE id = @id", con);

                    if (admin.IdPedidoEnConstruccion > 0)
                    {
                        cmd.Parameters.AddWithValue("@enConstruccion", admin.IdPedidoEnConstruccion);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@enConstruccion", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@usu", admin.NombreUsuario);
                    cmd.Parameters.AddWithValue("@id", admin.Id);

                    return cmd.ExecuteNonQuery()==1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public bool actualizarPassword(Administrador admin)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Administrador SET Contrasenia = @pass WHERE id = @id", con);

                    cmd.Parameters.AddWithValue("@pass", Utilidades.calculateMD5Hash(admin.Password));
                    cmd.Parameters.AddWithValue("@id", admin.Id);

                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }


        public void registrar(Administrador admin)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO Administrador(Usuario, Contrasenia) VALUES (@usu, @pass, null)", con);

                    cmd.Parameters.AddWithValue("@usu", admin.NombreUsuario);
                    cmd.Parameters.AddWithValue("@pass", Utilidades.calculateMD5Hash(admin.Password));

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

                    SqlCommand cmd = new SqlCommand("DELETE FROM Administrador WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery()==1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }
    }

}
