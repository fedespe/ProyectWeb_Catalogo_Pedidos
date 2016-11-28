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
                            Administrador admin = new Administrador
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                NombreUsuario = dr["Usuario"].ToString()
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
                    cmd.Parameters.AddWithValue("@pass", pass);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            admin = new Administrador();
                            admin.Id = Convert.ToInt32(dr["id"]);
                            admin.NombreUsuario = dr["Usuario"].ToString();
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
                            admin = new Administrador();
                            admin.Id = Convert.ToInt32(dr["id"]);
                            admin.NombreUsuario = dr["Usuario"].ToString();
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

                    SqlCommand cmd = new SqlCommand("UPDATE Administrador SET Usuario = @usu WHERE id = @id", con);

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

                    cmd.Parameters.AddWithValue("@pass", admin.Password);
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

                    SqlCommand cmd = new SqlCommand("INSERT INTO Administrador(Usuario, Contrasenia) VALUES (@usu, @pass)", con);

                    cmd.Parameters.AddWithValue("@usu", admin.NombreUsuario);
                    cmd.Parameters.AddWithValue("@pass", admin.Password);

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
