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
    public class FiltroDAL
    {
        public List<Filtro> obtenerTodos()
        {
            List<Filtro> filtros = new List<Filtro>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Filtro", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Filtro filtro = new Filtro
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                //Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                //Img = dr["Imagen"].ToString(),
                                //Precio = Convert.ToInt32(dr["Precio"]),
                                //Stock = Convert.ToInt32(dr["Stock"]),
                                //Disponible = Convert.ToBoolean(Convert.ToInt32(dr["Disponible"])),
                                //Destacado = Convert.ToBoolean(Convert.ToInt32(dr["Destacado"]))
                            };
                            filtros.Add(filtro);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return filtros;
        }
    }
}
