using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class RecordsNegocio
    {
        public List<Records> GetRecordsUsuario(string id)
        {
            string Excepcion = "Ocurrio un error al obtener records personales (RecordsNegocio.GetRecordsUsuario())\n";

            List<Records> records = new List<Records>();
            AccesoADatos datos = new AccesoADatos();

            try
            {
                datos.SetearConsultaSP("sp_RecordsPersonales");
                datos.setearParametro("@ID", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    // Grupo muscular puede ser nulo si el ejercicio es demasiado abarcativo
                    GrupoMuscular grupo = new GrupoMuscular();
                    if (!(datos.Lector["IdGrupoMuscular"] is DBNull))
                    {
                        grupo.IdGrupoMuscular = int.Parse(datos.Lector["IdGrupoMuscular"].ToString());
                        grupo.NombreGrupoMuscular = datos.Lector["GrupoMuscularNombre"].ToString();
                    }

                    Ejercicio ejercicio = new Ejercicio();
                    ejercicio.IdEjercicio = int.Parse(datos.Lector["IdEjercicio"].ToString());
                    ejercicio.NombreEjercicio = datos.Lector["EjercicioNombre"].ToString();
                    ejercicio.GrupoMuscular = grupo;

                    Records record = new Records();
                    record.Ejercicio = ejercicio;
                    record.PesoKG = float.Parse(datos.Lector["PesoKG"].ToString());
                    record.Repeticiones = int.Parse(datos.Lector["Repeticiones"].ToString());
                    record.FechaRecord = DateTime.Parse(datos.Lector["FechaRecord"].ToString());

                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion + ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }

            return records;
        }

        /// <summary>
        /// Devuelve el peso maximo (en KG) que el cliente levanto historicamente en un ejercicio.
        /// Devuelve 0 si todavia no registro ninguna serie de ese ejercicio.
        /// Se usa al registrar una serie para determinar si constituye un nuevo record personal.
        /// </summary>
        public float GetMaxPesoEjercicio(int idUsuario, int idEjercicio)
        {
            string Excepcion = "Ocurrio un error al obtener el peso maximo del ejercicio (RecordsNegocio.GetMaxPesoEjercicio())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsulta(@"SELECT MAX(SC.PesoLevantadoKG)
                    FROM SeriesCompletadas SC
                    INNER JOIN SesionesEntrenamiento SE ON SC.IdSesion = SE.IdSesionesEntrenamiento
                    WHERE SE.IdUsuario = @IdUsuario AND SC.IdEjercicio = @IdEjercicio");
                datos.setearParametro("@IdUsuario", idUsuario);
                datos.setearParametro("@IdEjercicio", idEjercicio);
                datos.ejecutarLectura();

                if (datos.Lector.Read() && !(datos.Lector[0] is DBNull))
                    return float.Parse(datos.Lector[0].ToString());

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion + ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
