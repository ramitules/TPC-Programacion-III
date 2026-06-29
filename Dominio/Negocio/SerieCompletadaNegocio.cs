using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class SerieCompletadaNegocio
    {
        /// <summary>
        /// Registra una serie completada por el cliente dentro de una sesion de entrenamiento.
        /// </summary>
        public void Agregar(SerieCompletada serie)
        {
            string Excepcion = "Ocurrio un error al registrar una serie completada (SerieCompletadaNegocio.Agregar())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_CrearSerieCompletada");
                datos.setearParametro("@IdSesion", serie.Sesion.IdSesion);
                datos.setearParametro("@IdEjercicio", serie.Ejercicio.IdEjercicio);
                datos.setearParametro("@PesoLevantadoKG", serie.PesoLevantadoKG);
                datos.setearParametro("@RepeticionesLogradas", serie.RepeticionesLogradas);
                datos.setearParametro("@RIR", serie.RIR);
                datos.setearParametro("@EsRecordPersonal", serie.EsRecordPersonal);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        /// <summary>
        /// Obtiene las series completadas de una sesion de entrenamiento.
        /// Util para mostrar el progreso (series ya registradas por ejercicio).
        /// </summary>
        public List<SerieCompletada> GetSeriesDeSesion(int idSesion)
        {
            string Excepcion = "Ocurrio un error al obtener las series de la sesion (SerieCompletadaNegocio.GetSeriesDeSesion())\n";

            AccesoADatos datos = new AccesoADatos();
            List<SerieCompletada> series = new List<SerieCompletada>();
            try
            {
                datos.SetearConsulta("SELECT IdSeriesCompletadas, IdEjercicio, PesoLevantadoKG, RepeticionesLogradas, RIR, EsRecordPersonal FROM SeriesCompletadas WHERE IdSesion = @IdSesion");
                datos.setearParametro("@IdSesion", idSesion);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    SerieCompletada serie = new SerieCompletada();
                    serie.IdSerieCompletada = int.Parse(datos.Lector["IdSeriesCompletadas"].ToString());
                    serie.Ejercicio = new Ejercicio();
                    serie.Ejercicio.IdEjercicio = int.Parse(datos.Lector["IdEjercicio"].ToString());
                    serie.PesoLevantadoKG = float.Parse(datos.Lector["PesoLevantadoKG"].ToString());
                    serie.RepeticionesLogradas = int.Parse(datos.Lector["RepeticionesLogradas"].ToString());
                    serie.RIR = datos.Lector["RIR"] is DBNull ? 0 : int.Parse(datos.Lector["RIR"].ToString());
                    serie.EsRecordPersonal = bool.Parse(datos.Lector["EsRecordPersonal"].ToString());

                    series.Add(serie);
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

            return series;
        }

        /// <summary>
        /// Obtiene las series completadas de una sesion agrupadas por ejercicio (con su grupo muscular).
        /// Se invoca bajo demanda al expandir una sesion, para no cargar miles de series de una sola vez.
        /// </summary>
        public List<SeriesPorEjercicio> GetSeriesAgrupadasDeSesion(int idSesion)
        {
            string Excepcion = "Ocurrio un error al obtener las series agrupadas de la sesion (SerieCompletadaNegocio.GetSeriesAgrupadasDeSesion())\n";

            AccesoADatos datos = new AccesoADatos();
            List<SeriesPorEjercicio> grupos = new List<SeriesPorEjercicio>();
            try
            {
                datos.SetearConsultaSP("sp_SeriesDeSesionAgrupadas");
                datos.setearParametro("@IdSesion", idSesion);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    string nombreEjercicio = datos.Lector["NombreEjercicio"].ToString();
                    string nombreGrupoMuscular = datos.Lector["NombreGrupoMuscular"] is DBNull ? "" : datos.Lector["NombreGrupoMuscular"].ToString();

                    SerieCompletada serie = new SerieCompletada();
                    serie.Ejercicio = new Ejercicio();
                    serie.Ejercicio.IdEjercicio = int.Parse(datos.Lector["IdEjercicio"].ToString());
                    serie.Ejercicio.NombreEjercicio = nombreEjercicio;
                    serie.Ejercicio.GrupoMuscular = new GrupoMuscular { NombreGrupoMuscular = nombreGrupoMuscular };
                    serie.PesoLevantadoKG = float.Parse(datos.Lector["PesoLevantadoKG"].ToString());
                    serie.RepeticionesLogradas = int.Parse(datos.Lector["RepeticionesLogradas"].ToString());
                    serie.RIR = datos.Lector["RIR"] is DBNull ? 0 : int.Parse(datos.Lector["RIR"].ToString());
                    serie.EsRecordPersonal = bool.Parse(datos.Lector["EsRecordPersonal"].ToString());

                    // Las filas vienen ordenadas por nombre de ejercicio: agrupamos por el ultimo grupo abierto.
                    SeriesPorEjercicio grupo = grupos.Count > 0 && grupos[grupos.Count - 1].NombreEjercicio == nombreEjercicio
                        ? grupos[grupos.Count - 1]
                        : null;

                    if (grupo == null)
                    {
                        grupo = new SeriesPorEjercicio
                        {
                            NombreEjercicio = nombreEjercicio,
                            NombreGrupoMuscular = nombreGrupoMuscular,
                            Series = new List<SerieCompletada>()
                        };
                        grupos.Add(grupo);
                    }

                    grupo.Series.Add(serie);
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

            return grupos;
        }
    }
}
