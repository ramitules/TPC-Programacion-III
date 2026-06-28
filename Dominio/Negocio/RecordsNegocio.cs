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
    }
}
