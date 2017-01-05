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
    public class ArticuloDAL
    {
        public List<Articulo> obtenerTodos()
        {
            List<Articulo> articulos = new List<Articulo>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    //SqlCommand cmd = new SqlCommand("SELECT * FROM Articulo", con);
                    //using (SqlDataReader dr = cmd.ExecuteReader())
                    //{
                    //    while (dr.Read())
                    //    {
                    //        Articulo articulo = new Articulo
                    //        {
                    //            Id = Convert.ToInt32(dr["Id"]),
                    //            Codigo = dr["Codigo"].ToString(),
                    //            Nombre = dr["Nombre"].ToString(),
                    //            Descripcion = dr["Descripcion"].ToString(),
                    //            Precio = Convert.ToInt32(dr["Precio"]),
                    //            Stock = Convert.ToInt32(dr["Stock"]),
                    //            Disponible = Convert.ToBoolean(Convert.ToInt32(dr["Disponible"])),
                    //            Destacado = Convert.ToBoolean(Convert.ToInt32(dr["Destacado"]))
                    //        };
                    //        articulos.Add(articulo);
                    //    }                       
                    //}   


                    //SOLO HAGO LA CARGA DE UNA IMAGEN, NO ES NECESARIO TRAER TODAS LAS IMAGENES DE TODOS LOS ARTICULOS
                    SqlCommand cmd = new SqlCommand("select a.Id as IdArticulo, i.id as IdImagen,* from ARTICULO a, IMAGEN i where i.IdArticulo=a.Id ORDER BY a.Id", con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int ultimoId = 0;
                        while (dr.Read())
                        {
                            if (ultimoId != Convert.ToInt32(dr["IdArticulo"])) {
                                Articulo articulo = new Articulo
                                {
                                    Id = Convert.ToInt32(dr["IdArticulo"]),
                                    Codigo = dr["Codigo"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Precio = Convert.ToInt32(dr["Precio"]),
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    Disponible = Convert.ToBoolean(Convert.ToInt32(dr["Disponible"])),
                                    Destacado = Convert.ToBoolean(Convert.ToInt32(dr["Destacado"]))                    
                                };
                                articulo.Imagenes.Add(new Imagen { Id = Convert.ToInt32(dr["IdImagen"]), Img = dr["Imagen"].ToString() });
                                articulos.Add(articulo);
                                ultimoId = Convert.ToInt32(dr["IdArticulo"]);
                            }//EN UN ELSE SI QUISIERA TRAIGO EL RESTO DE LAS IMAGENES                          
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return articulos;
        }

        public List<Articulo> obtenerConFiltros(List<Filtro> Filtros){
            //List<Articulo> articulos = new List<Articulo>();
            //string consulta = "";
            //if (Filtros != null && Filtros.Count > 0)
            //{
            //    int contador = 1;
            //    foreach (Filtro f in Filtros) {
            //        if (contador != Filtros.Count)
            //        {
            //            consulta += @"SELECT DISTINCT a.* FROM ARTICULO a, FILTRO_ARTICULO fa, FILTRO f WHERE a.Id=fa.IdArticulo and f.Id=fa.IdFiltro 
            //                        and f.Id = "+f.Id+" INTERSECT ";
            //        }
            //        else {
            //            consulta += @"SELECT DISTINCT a.* FROM ARTICULO a, FILTRO_ARTICULO fa, FILTRO f WHERE a.Id=fa.IdArticulo and f.Id=fa.IdFiltro 
            //                        and f.Id = " + f.Id;
            //        }
            //        contador++;
            //    }
            //}
            //else {
            //    consulta = "SELECT * FROM Articulo";
            //}
            List<Articulo> articulos = new List<Articulo>();
            string consulta = "";
            if (Filtros != null && Filtros.Count > 0)
            {
                int contador = 1;
                foreach (Filtro f in Filtros)
                {
                    if (contador != Filtros.Count)
                    {
                        consulta += @"SELECT DISTINCT a.Id as IdArticulo, i.id as IdImagen,i.Imagen, a.* FROM ARTICULO a, FILTRO_ARTICULO fa, FILTRO f, IMAGEN i WHERE i.IdArticulo=a.Id and a.Id=fa.IdArticulo and f.Id=fa.IdFiltro 
                                    and f.Id = " + f.Id + "INTERSECT ";
                    }
                    else {
                        consulta += @"SELECT DISTINCT a.Id as IdArticulo, i.id as IdImagen,i.Imagen, a.* FROM ARTICULO a, FILTRO_ARTICULO fa, FILTRO f, IMAGEN i WHERE i.IdArticulo=a.Id and a.Id=fa.IdArticulo and f.Id=fa.IdFiltro 
                                    and f.Id = " + f.Id + "Order by a.id ";
                    }
                    contador++;
                }
            }
            else {
                consulta = "select a.Id as IdArticulo, i.id as IdImagen,* from ARTICULO a, IMAGEN i where i.IdArticulo=a.Id ORDER BY a.Id";
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int ultimoId = 0;
                        while (dr.Read())
                        {
                            if (ultimoId != Convert.ToInt32(dr["IdArticulo"]))
                            {
                                Articulo articulo = new Articulo
                                {
                                    Id = Convert.ToInt32(dr["IdArticulo"]),
                                    Codigo = dr["Codigo"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Precio = Convert.ToInt32(dr["Precio"]),
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    Disponible = Convert.ToBoolean(Convert.ToInt32(dr["Disponible"])),
                                    Destacado = Convert.ToBoolean(Convert.ToInt32(dr["Destacado"]))
                                };
                                articulo.Imagenes.Add(new Imagen { Id = Convert.ToInt32(dr["IdImagen"]), Img = dr["Imagen"].ToString() });
                                articulos.Add(articulo);
                                ultimoId = Convert.ToInt32(dr["IdArticulo"]);
                            }//EN UN ELSE SI QUISIERA TRAIGO EL RESTO DE LAS IMAGENES 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return articulos;
        }

        public List<Articulo> obtenerPorCategoriaConFiltros(int idCategoria, List<Filtro> filtros)
        {
            List<Articulo> articulos = new List<Articulo>();
            string consulta = "";
            if (filtros.Count > 0)
            {
                consulta+= @"select a.Id as IdArticulo, i.id as IdImagen,i.Imagen,a.* from ARTICULO a, IMAGEN i, ARTICULO_CATEGORIA ac where i.IdArticulo=a.Id and ac.IdArticulo=a.Id and IdCategoria=" + idCategoria+" INTERSECT ";
            }
            else {
                consulta += @"select a.Id as IdArticulo, i.id as IdImagen,i.Imagen,a.* from ARTICULO a, IMAGEN i, ARTICULO_CATEGORIA ac where i.IdArticulo=a.Id and ac.IdArticulo=a.Id and IdCategoria=" + idCategoria + " ORDER BY a.Id ";
            }
            
            if (filtros != null && filtros.Count > 0)
            {
                int contador = 1;
                foreach (Filtro f in filtros)
                {
                    if (contador != filtros.Count)
                    {
                        consulta += @"SELECT DISTINCT a.Id as IdArticulo, i.id as IdImagen,i.Imagen, a.* FROM ARTICULO a, FILTRO_ARTICULO fa, FILTRO f, IMAGEN i WHERE i.IdArticulo=a.Id and a.Id=fa.IdArticulo and f.Id=fa.IdFiltro 
                                    and f.Id = " + f.Id + "INTERSECT ";
                    }
                    else {
                        consulta += @"SELECT DISTINCT a.Id as IdArticulo, i.id as IdImagen,i.Imagen, a.* FROM ARTICULO a, FILTRO_ARTICULO fa, FILTRO f, IMAGEN i WHERE i.IdArticulo=a.Id and a.Id=fa.IdArticulo and f.Id=fa.IdFiltro 
                                    and f.Id = " + f.Id + "Order by a.id ";
                    }
                    contador++;
                }
            }
            //else {
            //    consulta = "select a.Id as IdArticulo, i.id as IdImagen,* from ARTICULO a, IMAGEN i where i.IdArticulo=a.Id ORDER BY a.Id";
            //}
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int ultimoId = 0;
                        while (dr.Read())
                        {
                            if (ultimoId != Convert.ToInt32(dr["IdArticulo"]))
                            {
                                Articulo articulo = new Articulo
                                {
                                    Id = Convert.ToInt32(dr["IdArticulo"]),
                                    Codigo = dr["Codigo"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Precio = Convert.ToInt32(dr["Precio"]),
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    Disponible = Convert.ToBoolean(Convert.ToInt32(dr["Disponible"])),
                                    Destacado = Convert.ToBoolean(Convert.ToInt32(dr["Destacado"]))
                                };
                                articulo.Imagenes.Add(new Imagen { Id = Convert.ToInt32(dr["IdImagen"]), Img = dr["Imagen"].ToString() });
                                articulos.Add(articulo);
                                ultimoId = Convert.ToInt32(dr["IdArticulo"]);
                            }//EN UN ELSE SI QUISIERA TRAIGO EL RESTO DE LAS IMAGENES 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return articulos;
        }

        public List<Articulo> obtenerPorCategoria(int idCategoria)
        {
            
            List<Articulo> articulos = new List<Articulo>();
            string consulta = "select a.Id as IdArticulo, i.id as IdImagen,* from ARTICULO a, IMAGEN i, ARTICULO_CATEGORIA ac where i.IdArticulo=a.Id and ac.IdArticulo=a.Id and IdCategoria=@idCat ORDER BY a.Id";         
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@idCat", idCategoria);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int ultimoId = 0;
                        while (dr.Read())
                        {
                            if (ultimoId != Convert.ToInt32(dr["IdArticulo"]))
                            {
                                Articulo articulo = new Articulo
                                {
                                    Id = Convert.ToInt32(dr["IdArticulo"]),
                                    Codigo = dr["Codigo"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Precio = Convert.ToInt32(dr["Precio"]),
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    Disponible = Convert.ToBoolean(Convert.ToInt32(dr["Disponible"])),
                                    Destacado = Convert.ToBoolean(Convert.ToInt32(dr["Destacado"]))
                                };
                                articulo.Imagenes.Add(new Imagen { Id = Convert.ToInt32(dr["IdImagen"]), Img = dr["Imagen"].ToString() });
                                articulos.Add(articulo);
                                ultimoId = Convert.ToInt32(dr["IdArticulo"]);
                            }//EN UN ELSE SI QUISIERA TRAIGO EL RESTO DE LAS IMAGENES 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return articulos;
        }


        public List<Imagen> obtenerImagenes(int idArticulo) {
            List<Imagen> imagenes = new List<Imagen>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM Imagen WHERE IdArticulo =@id", con);
                    cmd.Parameters.AddWithValue("@id", idArticulo);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Imagen img = new Imagen
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Img = dr["Imagen"].ToString()         
                            };
                            imagenes.Add(img);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return imagenes;
        }

        public List<Filtro> obtenerFiltros(int idArticulo)
        {
            List<Filtro> filtros = new List<Filtro>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT f.* FROM Filtro_Articulo fa, FILTRO f WHERE f.Id=fa.IdFiltro AND IdArticulo =@id", con);
                    cmd.Parameters.AddWithValue("@id", idArticulo);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Filtro filtro = new Filtro
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Color = Convert.ToBoolean(Convert.ToInt32(dr["Color"]))
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

        public List<Categoria> obtenerCategorias(int idArticulo)
        {
            List<Categoria> categorias = new List<Categoria>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT c.* FROM ARTICULO_CATEGORIA ac, CATEGORIA c WHERE c.Id=ac.IdCategoria AND IdArticulo =@id", con);
                    cmd.Parameters.AddWithValue("@id", idArticulo);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Categoria categoria = new Categoria
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Img = dr["Imagen"].ToString()
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

        public Articulo obtener(int id)
        {
            Articulo articulo = null;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Articulo WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            articulo = new Articulo
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Precio = Convert.ToInt32(dr["Precio"]),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                Disponible = Convert.ToBoolean(Convert.ToInt32(dr["Disponible"])),
                                Destacado = Convert.ToBoolean(Convert.ToInt32(dr["Destacado"])),
                                Imagenes = new List<Imagen>(),
                                Categorias = new List<Categoria>(),
                                Filtros = new List<Filtro>()
                            };
                        }
                    }
                    //AGREGO IMAGENES
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT * FROM Imagen WHERE IdArticulo =@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Imagen img = new Imagen
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Img = dr["Imagen"].ToString()
                            };
                            articulo.Imagenes.Add(img);
                        }
                    }
                    //AGREGO FILTROS
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT f.* FROM Filtro_Articulo fa, FILTRO f WHERE f.Id=fa.IdFiltro AND IdArticulo =@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Filtro filtro = new Filtro
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Color = Convert.ToBoolean(Convert.ToInt32(dr["Color"]))
                            };
                            articulo.Filtros.Add(filtro);
                        }
                    }
                    //AGREGO CATEGORIAS
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT c.* FROM ARTICULO_CATEGORIA ac, CATEGORIA c WHERE c.Id=ac.IdCategoria AND IdArticulo =@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Categoria categoria = new Categoria
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Img = dr["Imagen"].ToString()
                            };
                            articulo.Categorias.Add(categoria);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }

            return articulo;
        }

        public bool actualizar(Articulo articulo)
        {
            bool res = true;
            SqlTransaction trn = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
            {
                try{
                
                        con.Open();
                        SqlCommand cmd = new SqlCommand(@"UPDATE Articulo SET Codigo = @codigo, Nombre = @nombre, 
                                                        Descripcion=@descripcion, Precio=@precio, Stock=@stock,
                                                        Disponible=@disponible, Destacado=@destacado WHERE id = @id", con);

                        cmd.Parameters.AddWithValue("@codigo", articulo.Codigo);
                        cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                        cmd.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                        cmd.Parameters.AddWithValue("@precio", articulo.Precio);
                        cmd.Parameters.AddWithValue("@stock", articulo.Stock);
                        cmd.Parameters.AddWithValue("@disponible", articulo.Disponible);
                        cmd.Parameters.AddWithValue("@destacado", articulo.Destacado);
                        cmd.Parameters.AddWithValue("@id", articulo.Id);

                        trn = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                        cmd.Transaction = trn;

                        if (cmd.ExecuteNonQuery() == 0) {
                            res = false;
                        } 
                        if (res) {
                            //ACTUALIZO IMAGENES 
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"Delete FROM Imagen WHERE IdArticulo = @id";
                            cmd.Parameters.AddWithValue("@id", articulo.Id);
                            cmd.ExecuteNonQuery();
                            foreach (Imagen i in articulo.Imagenes) {
                                cmd.Parameters.Clear();
                                cmd.CommandText = @"INSERT INTO Imagen VALUES (@idArt, @imagen)";
                                cmd.Parameters.AddWithValue("@imagen", i.Img);
                                cmd.Parameters.AddWithValue("@idArt", articulo.Id);
                                if (cmd.ExecuteNonQuery() == 0)
                                {
                                    res = false;
                                }
                            }

                            //ACTUALIZO FILTROS
                            if (res) {
                                cmd.Parameters.Clear();
                                cmd.CommandText = @"Delete FROM Filtro_Articulo WHERE IdArticulo = @id";
                                cmd.Parameters.AddWithValue("@id", articulo.Id);
                                cmd.ExecuteNonQuery();
                                foreach (Filtro f in articulo.Filtros)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"INSERT INTO Filtro_Articulo VALUES (@idFiltro, @idArt)";
                                    cmd.Parameters.AddWithValue("@idFiltro", f.Id);
                                    cmd.Parameters.AddWithValue("@idArt", articulo.Id);
                                    if (cmd.ExecuteNonQuery() == 0)
                                    {
                                        res = false;
                                    }
                                }
                            }

                            //ACTUALIZO CATEGORIAS
                            if (res)
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandText = @"Delete FROM Articulo_Categoria WHERE IdArticulo = @id";
                                cmd.Parameters.AddWithValue("@id", articulo.Id);
                                cmd.ExecuteNonQuery();
                                foreach (Categoria c in articulo.Categorias)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"INSERT INTO Articulo_Categoria VALUES (@idArt, @idCat)";
                                    cmd.Parameters.AddWithValue("@idArt", articulo.Id);
                                    cmd.Parameters.AddWithValue("@idCat", c.Id);
                                    if (cmd.ExecuteNonQuery() == 0)
                                    {
                                        res = false;
                                    }
                                }
                        

                        }
                        trn.Commit();
                        trn.Dispose();
                        trn = null;                        
                    }
                    return res;
                }
                catch (Exception ex)
                {
                    if (trn != null) {
                        trn.Rollback();
                    }
                    throw new ProyectoException("Error: " + ex.Message);
                }
            }
        }

        public bool registrar(Articulo articulo)
        {
            bool res = true;
            SqlTransaction trn = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
            {
                try
                {                
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Articulo VALUES (@codigo, @nombre, 
                                                    @descripcion, @precio, @stock,
                                                    @disponible, @destacado); SELECT CAST(Scope_Identity() AS INT);
                                                    ", con);

                    cmd.Parameters.AddWithValue("@codigo", articulo.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", articulo.Precio);
                    cmd.Parameters.AddWithValue("@stock", articulo.Stock);
                    cmd.Parameters.AddWithValue("@disponible", articulo.Disponible);
                    cmd.Parameters.AddWithValue("@destacado", articulo.Destacado);

                    trn = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = trn;

                    articulo.Id = (int)cmd.ExecuteScalar();

                    if (articulo.Id == 0) //CHEQUEAR QUE SI NO INSERTA DEVUELVE O
                    {
                        res = false;
                    }
                    if (res)
                    {
                        //INGRESO IMAGENES 
                        foreach (Imagen i in articulo.Imagenes)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"INSERT INTO Imagen VALUES (@idArt, @imagen)";
                            cmd.Parameters.AddWithValue("@imagen", i.Img);
                            cmd.Parameters.AddWithValue("@idArt", articulo.Id);
                            if (cmd.ExecuteNonQuery() == 0)
                            {
                                res = false;
                            }
                        }

                        //INGRESO FILTROS
                        if (res)
                        {
                            foreach (Filtro f in articulo.Filtros)
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandText = @"INSERT INTO Filtro_Articulo VALUES (@idFiltro, @idArt)";
                                cmd.Parameters.AddWithValue("@idFiltro", f.Id);
                                cmd.Parameters.AddWithValue("@idArt", articulo.Id);
                                if (cmd.ExecuteNonQuery() == 0)
                                {
                                    res = false;
                                }
                            }
                        }

                        //INGRESO CATEGORIAS
                        if (res)
                        {
                            foreach (Categoria c in articulo.Categorias)
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandText = @"INSERT INTO Articulo_Categoria VALUES (@idArt, @idCat)";
                                cmd.Parameters.AddWithValue("@idArt", articulo.Id);
                                cmd.Parameters.AddWithValue("@idCat", c.Id);
                                if (cmd.ExecuteNonQuery() == 0)
                                {
                                    res = false;
                                }                            
                            }

                        }
                        trn.Commit();
                        trn.Dispose();
                        trn = null;                       
                    }
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

        public bool inhabilitar(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("Update Articulo SET Disponible = 0 WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public bool habilitar(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("Update Articulo SET Disponible = 1 WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public bool destacar(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("Update Articulo SET Destacado = 1 WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

        public bool quitarDestacado(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("Update Articulo SET Destacado = 0 WHERE id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (Exception ex)
            {
                throw new ProyectoException("Error: " + ex.Message);
            }
        }

    }
}
