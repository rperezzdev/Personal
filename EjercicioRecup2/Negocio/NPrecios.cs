using Datos;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NPrecios
    {
        public static void Create(Precios p)
        {
            try
            {
                DPrecios.Create(p);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void Update(Precios p)
        {
            try
            {
                DPrecios.Update(p);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<Precios> Get()
        {
            try
            {
                return DPrecios.GetAll();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void Delete(int id)
        {
            try
            {
                DPrecios.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Precios> GetByMonto(int montoMin, int montoMax)
        {
            try
            {
                return DPrecios.GetByMonto(montoMin, montoMax);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
