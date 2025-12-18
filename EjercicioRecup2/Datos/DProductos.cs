using Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DProductos
    {
        public static void Create (Productos p)
        {
            string sql = @"
                    INSERT INTO productos (Descripcion)
                    VALUES (@Descripcion)";
            using (SqlConnection cn = Db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@Descripcion", System.Data.SqlDbType.NVarChar).Value = p.Descripcion;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public static List<Productos> GetAll()
        {
            List<Productos> lista = new List<Productos>();
            string sql = @"SELECT id, descripcion FROM productos ORDER BY id";
            using (SqlConnection cn = Db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Productos p = new Productos()
                            {
                                Id = reader.GetInt32(0),  // Lee directamente como int
                                Descripcion = reader.GetString(1)  // Lee directamente como string
                            };
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }
        public static void Delete(int id)
        {
            string sql = @"Delete from productos Where Id = @Id";
            using (SqlConnection cn = Db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void Update(Productos p)
        {
            //definimo instruccion SQL
            string sql = @"
                    UPDATE productos SET
                    Descripcion = @Descripcion
                    WHERE Id = @Id
                ";
            //creamos SqlConnection
            using (SqlConnection cn = Db.GetConnection())
            {
                //Creamos SqlCommand
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {

                    // Definimos los parametros que esta en el SQL
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = p.Id;
                    cmd.Parameters.Add("@Descripcion", System.Data.SqlDbType.NVarChar).Value = p.Descripcion;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
