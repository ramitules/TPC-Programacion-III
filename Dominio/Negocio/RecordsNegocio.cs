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
                        grupo.IdGrupoMuscular = Convert.ToInt32(datos.Lector["IdGrupoMuscular"]);
                        grupo.NombreGrupoMuscular = datos.Lector["GrupoMuscularNombre"].ToString();
                    }

                    Ejercicio ejercicio = new Ejercicio();
                    ejercicio.IdEjercicio = Convert.ToInt32(datos.Lector["IdEjercicio"]);
                    ejercicio.NombreEjercicio = datos.Lector["EjercicioNombre"].ToString();
                    ejercicio.GrupoMuscular = grupo;

                    Records record = new Records();
                    record.Ejercicio = ejercicio;
                    record.PesoKG = Convert.ToSingle(datos.Lector["PesoKG"]);
                    record.Repeticiones = Convert.ToInt32(datos.Lector["Repeticiones"]);
                    record.FechaRecord = Convert.ToDateTime(datos.Lector["FechaRecord"]);

                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return records;
        }
    }
}
