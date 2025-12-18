using Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DPrecios
    {
        public static void Create(Precios p)
        {
            string sql = @"
                INSERT INTO precios (fecha, monto, idProducto)
                VALUES (@Fecha, @Monto, @IdProducto)";

            using (SqlConnection cn = Db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@Fecha", System.Data.SqlDbType.Date).Value = p.Fecha;
                    cmd.Parameters.Add("@Monto", System.Data.SqlDbType.Int).Value = p.Monto;
                    cmd.Parameters.Add("@IdProducto", System.Data.SqlDbType.Int).Value = p.idProducto;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public static List<Precios> GetAll()
        {
            List<Precios> lista = new List<Precios>();
            string sql = @"
                SELECT p.id, p.fecha, p.monto, p.idProducto, pr.descripcion
                FROM precios p
                INNER JOIN productos pr ON p.idProducto = pr.id
                ORDER BY p.id";

            using (SqlConnection cn = Db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Precios p = new Precios()
                            {
                                Id = reader.GetInt32(0),
                                Fecha = reader.GetDateTime(1),
                                Monto = reader.GetInt32(2),
                                idProducto = reader.GetInt32(3),
                                
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
            string sql = @"DELETE FROM precios WHERE id = @Id";

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

        public static void Update(Precios p)
        {
            string sql = @"
                UPDATE precios SET
                    fecha = @Fecha,
                    monto = @Monto,
                    idProducto = @IdProducto
                WHERE id = @Id";

            using (SqlConnection cn = Db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = p.Id;
                    cmd.Parameters.Add("@Fecha", System.Data.SqlDbType.Date).Value = p.Fecha;
                    cmd.Parameters.Add("@Monto", System.Data.SqlDbType.Int).Value = p.Monto;
                    cmd.Parameters.Add("@IdProducto", System.Data.SqlDbType.Int).Value = p.idProducto;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<Precios> GetByMonto(int montoMin, int montoMax)
        {
            List<Precios> lista = new List<Precios>();
            string sql = @"
        SELECT p.id, p.fecha, p.monto, p.idProducto, pr.descripcion
        FROM precios p
        INNER JOIN productos pr ON p.idProducto = pr.id
        WHERE p.monto BETWEEN @MontoMin AND @MontoMax
        ORDER BY p.monto";

            using (SqlConnection cn = Db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@MontoMin", System.Data.SqlDbType.Int).Value = montoMin;
                    cmd.Parameters.Add("@MontoMax", System.Data.SqlDbType.Int).Value = montoMax;
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Precios p = new Precios()
                            {
                                Id = reader.GetInt32(0),
                                Fecha = reader.GetDateTime(1),
                                Monto = reader.GetInt32(2),
                                idProducto = reader.GetInt32(3),

                            };
                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }
    }
}