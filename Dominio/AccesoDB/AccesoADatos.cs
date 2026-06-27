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
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public AccesoADatos()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["MiBaseDatos"].ConnectionString;
            conexion = new SqlConnection(cadenaConexion);
            comando = new SqlCommand();
        }

        public void SetearConsultaSP(string sp)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }
        public void SetearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        /// <summary>
        /// Ejecuta query y devuelve primera fila de primera columna.
        /// Util para ejecutar y devolver el ID del registro insertado, modificado o eliminado
        /// </summary>
        public int EjecutarScalar()
        {
            comando.Connection = conexion;
            conexion.Open();
            return int.Parse(comando.ExecuteScalar().ToString());
        }
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            conexion.Open();
            lector = comando.ExecuteReader();
        }
        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            conexion.Open();
            comando.ExecuteNonQuery();
        }
        public SqlDataReader Lector
        {
            get { return lector; }
        }
        public void cerrarConexion()
        {
            lector?.Close();
            conexion.Close();
        }
    }
}
