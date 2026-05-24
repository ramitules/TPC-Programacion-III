using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDB
{
    public class AccesoADatos
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;

        public AccesoADatos()
        {
            try
            {
                string cadenaConexion = ConfigurationManager.ConnectionStrings["MiBaseDatos"].ConnectionString;
                conexion = new SqlConnection(cadenaConexion);
                comando = new SqlCommand();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error al conectar a la base de datos: (Intancia de acceso a datos) " + e.Message);
            }
        }

        public void SetearConsultaSP(string sp)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + e.Message);
            }
        }
        public SqlDataReader Lector
        {
            get { return lector; }
        }
    }
}
