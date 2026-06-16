using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class RutinasNegocio
    {
        /// <summary>
        /// Obtiene las rutinas asignadas a un solo usuario especifico.
        /// Puede devolver lista vacia.
        /// </summary>
        public List<Rutina> GetRutinasUsuario(Cliente cliente)
        {
            string Excepcion = "Ocurrio un error al obtener rutinas (RutinasNegocio.GetRutinasUsuario())\n";

            AccesoADatos datos = new AccesoADatos();
            List<Rutina> rutinas = new List<Rutina>();

            try
            {
                datos.SetearConsulta("SELECT * FROM Rutinas WHERE IdUsuario = @Id");
                datos.setearParametro("@Id", cliente.IdUsuario);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    if (!bool.Parse(datos.Lector["Activo"].ToString())) continue;

                    rutinas.Add(new Rutina() {
                        IdRutina = int.Parse(datos.Lector["IdRutinas"].ToString()),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Cliente = cliente,
                        FechaCreacion = DateTime.Parse(datos.Lector["FechaCreacion"].ToString()),
                        Dia = datos.Lector["Dia"].ToString(),
                        Activo = true
                    });
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

            return rutinas;
        }
        /// <summary>
        /// Obtiene todas las rutinas sin asignar a un usuario en especifico.
        /// Puede devolver lista vacia.
        /// </summary>
        public List<Rutina> GetRutinasGenerales()
        {
            string Excepcion = "Ocurrio un error al obtener rutinas (RutinasNegocio.GetRutinasGenerales())\n";

            AccesoADatos datos = new AccesoADatos();
            List<Rutina> rutinas = new List<Rutina>();

            try
            {
                datos.SetearConsulta("SELECT IdRutinas, Nombre, FechaCreacion, Dia, Activo FROM Rutinas WHERE IdUsuario IS NULL");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    if (!bool.Parse(datos.Lector["Activo"].ToString())) continue;

                    rutinas.Add(new Rutina()
                    {
                        IdRutina = int.Parse(datos.Lector["IdRutinas"].ToString()),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Cliente = null,
                        FechaCreacion = DateTime.Parse(datos.Lector["FechaCreacion"].ToString()),
                        Dia = datos.Lector["Dia"].ToString(),
                        Activo = true
                    });
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
            
            return rutinas;
        }

        /// <summary>
        /// Obtiene una rutina segun su ID. Puede devolver NULL si no la encuentra.
        /// Si ClienteCompleto es verdadero, obtiene todos los datos del cliente siempre y cuando este activo.
        /// De lo contrario lo deja nulo.
        /// ADVERTENCIA: se ejecuta una consulta a la base de datos para obtener el cliente.
        /// </summary>
        public Rutina Get(string id, bool ClienteCompleto = false)
        {
            string Excepcion = "Ocurrio un error al obtener rutinas (RutinasNegocio.Get())\n";

            AccesoADatos datos = new AccesoADatos();
            Rutina rutina = null;

            try
            {
                datos.SetearConsulta("SELECT * FROM Rutinas WHERE IdRutinas = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    rutina = new Rutina() {
                        IdRutina = int.Parse(id),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Cliente = datos.Lector["IdUsuario"] is DBNull ? null : new ClienteNegocio().Get(datos.Lector["IdUsuario"].ToString()),
                        FechaCreacion = DateTime.Parse(datos.Lector["FechaCreacion"].ToString()),
                        Dia = datos.Lector["Dia"].ToString(),
                        Activo = true
                    };
                    break;
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
            return rutina;
        }

        /// <summary>
        /// Obtiene los ejercicios (con sus objetivos de series, repeticiones, peso y orden)
        /// que componen una rutina. Puede devolver lista vacia.
        /// </summary>
        public List<RutinaEjercicio> GetEjerciciosDeRutina(string idRutina)
        {
            string Excepcion = "Ocurrio un error al obtener los ejercicios de la rutina (RutinasNegocio.GetEjerciciosDeRutina())\n";

            AccesoADatos datos = new AccesoADatos();
            List<RutinaEjercicio> ejercicios = new List<RutinaEjercicio>();

            try
            {
                datos.SetearConsultaSP("sp_EjerciciosDeRutina");
                datos.setearParametro("@IdRutina", idRutina);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    RutinaEjercicio re = new RutinaEjercicio();

                    re.IdRutinaEjercicio = int.Parse(datos.Lector["IdRutinaEjercicio"].ToString());
                    re.ObjetivoKG = datos.Lector["ObjetivoKG"] is DBNull ? 0 : int.Parse(datos.Lector["ObjetivoKG"].ToString());
                    re.ObjetivoSeries = int.Parse(datos.Lector["ObjetivoSeries"].ToString());
                    re.ObjetivoRepeticiones = int.Parse(datos.Lector["ObjetivoRepeticiones"].ToString());
                    re.OrdenEjercicio = int.Parse(datos.Lector["OrdenEjercicio"].ToString());

                    re.Ejercicio = new Ejercicio();
                    re.Ejercicio.IdEjercicio = int.Parse(datos.Lector["IdEjercicio"].ToString());
                    re.Ejercicio.NombreEjercicio = datos.Lector["NombreEjercicio"].ToString();

                    if (!(datos.Lector["IdGrupoMuscular"] is DBNull))
                    {
                        re.Ejercicio.GrupoMuscular.IdGrupoMuscular = int.Parse(datos.Lector["IdGrupoMuscular"].ToString());
                        re.Ejercicio.GrupoMuscular.NombreGrupoMuscular = datos.Lector["NombreGrupoMuscular"].ToString();
                    }

                    ejercicios.Add(re);
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

            return ejercicios;
        }

        public List<DiaRutina> AgruparPorDia(List<Rutina> rutinas)
        {
            string[] dias = new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            List<DiaRutina> resultado = new List<DiaRutina>();

            foreach (string dia in dias)
                resultado.Add(new DiaRutina
                {
                    DiaDeRutina = dia,
                    Rutinas = rutinas.Where(r => string.Equals(r.Dia, dia,
                                  StringComparison.OrdinalIgnoreCase)).ToList()
                });

            resultado.Add(new DiaRutina
            {
                DiaDeRutina = "Sin día",
                Rutinas = rutinas.Where(r => string.IsNullOrEmpty(r.Dia)).ToList()
            });

            return resultado;
        }

    }
}
