using Datos;
using Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NProductos
    {
        public static void Create(Productos p)
        {
            try
            {
                DProductos.Create(p);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void Update(Productos p)
        {
            try
            {
                DProductos.Update(p);
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
                DProductos.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<Productos> Get()
        {
            try
            {
                return DProductos.GetAll();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
