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
    public class CategoriaDAL
    {
        public List<Categoria> obtenerTodos()
        {
            List<Categoria> categorias = new List<Categoria>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Categoria", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Categoria categoria = new Categoria
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Img = dr["Imagen"].ToString(),                               
                            };
                            categorias.Add(categoria);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return categorias;
        }

        public Categoria obtener(int id)
        {
            Categoria cat = null;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Categoria WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            cat = new Categoria
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Img = dr["Imagen"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return cat;
        }

        public bool actualizar(Categoria cat)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE Categoria SET
                        Nombre = @nom, Imagen = @img
                        WHERE id = @id", con);

                    cmd.Parameters.AddWithValue("@id", cat.Id);
                    cmd.Parameters.AddWithValue("@nom", cat.Nombre);
                    cmd.Parameters.AddWithValue("@img", cat.Img);

                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }     


        public void registrar(Categoria cat)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Categoria VALUES(
                        @nom,@img)"
                        , con);

                    cmd.Parameters.AddWithValue("@nom", cat.Nombre);
                    cmd.Parameters.AddWithValue("@img", cat.Img);

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
            bool res = true;
            SqlTransaction trn = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
            {
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"delete from ARTICULO_CATEGORIA where IdCategoria = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    trn = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = trn;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"delete from FILTRO_CATEGORIA where IdCategoria = @id";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"delete from CATEGORIA where id = @id";
                    res= cmd.ExecuteNonQuery()!=0;
                    trn.Commit();
                    trn.Dispose();
                    trn = null;
                    return res;
                }
                catch (Exception ex)
                {
                    if (trn != null)
                    {
                        trn.Rollback();
                    }
                    throw new ProyectoException("Error: " + ex.Message);
                }
            }
        }
    }

}
