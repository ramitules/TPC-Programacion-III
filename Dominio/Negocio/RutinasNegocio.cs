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
        public List<Rutina> GetRutinasUsuario(string id = "")
        {
            string Excepcion = "Ocurrio un error al obtener rutinas (RutinasNegocio.GetRutinasCliente())\n";

            AccesoADatos datos = new AccesoADatos();
            List<Rutina> rutinas = new List<Rutina>();

            try
            {
                datos.SetearConsulta("SELECT * FROM Rutinas WHERE IdUsuario = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    if (!bool.Parse(datos.Lector["Activo"].ToString())) continue;

                    rutinas.Add(new Rutina(
                        idRutina: int.Parse(datos.Lector["IdRutinas"].ToString()),
                        nombre: datos.Lector["Nombre"].ToString(),
                        idUsuario: int.Parse(id),
                        fechaCreacion: DateTime.Parse(datos.Lector["FechaCreacion"].ToString()),
                        dia: datos.Lector["Dia"].ToString(),
                        activo: true
                    ));
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

                    rutinas.Add(new Rutina(
                        idRutina: int.Parse(datos.Lector["IdRutinas"].ToString()),
                        nombre: datos.Lector["Nombre"].ToString(),
                        idUsuario: 0,
                        fechaCreacion: DateTime.Parse(datos.Lector["FechaCreacion"].ToString()),
                        dia: datos.Lector["Dia"].ToString(),
                        activo: true
                    ));
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
        /// Obtiene una rutina segun su ID. Puede devolver NULL si no la encuentra
        /// </summary>
        public Rutina Get(string id)
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
                    rutina = new Rutina(
                        idRutina: int.Parse(datos.Lector["IdRutinas"].ToString()),
                        nombre: datos.Lector["Nombre"].ToString(),
                        idUsuario: int.Parse(id),
                        fechaCreacion: DateTime.Parse(datos.Lector["FechaCreacion"].ToString()),
                        dia: datos.Lector["Dia"].ToString(),
                        activo: true
                    );
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
