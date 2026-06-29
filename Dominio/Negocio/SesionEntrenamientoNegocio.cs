using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class SesionEntrenamientoNegocio
    {
        /// <summary>
        /// Obtiene todas las sesiones de entrenamiento de un cliente (solo metadata: rutina y cantidad de
        /// series). No trae las series completadas; estas se cargan aparte y bajo demanda al expandir cada
        /// sesion. Evita el N+1 de Get() (que resuelve Cliente y Rutina por separado en cada fila).
        /// </summary>
        public List<SesionEntrenamiento> GetSesionesDeCliente(int idUsuario)
        {
            string Excepcion = "Ocurrio un error al obtener las sesiones del cliente (SesionEntrenamientoNegocio.GetSesionesDeCliente())\n";

            AccesoADatos datos = new AccesoADatos();
            List<SesionEntrenamiento> sesiones = new List<SesionEntrenamiento>();
            try
            {
                datos.SetearConsultaSP("sp_SesionesDeCliente");
                datos.setearParametro("@IdUsuario", idUsuario);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    SesionEntrenamiento sesion = new SesionEntrenamiento();
                    sesion.IdSesion = int.Parse(datos.Lector["IdSesion"].ToString());
                    sesion.FechaHoraInicio = DateTime.Parse(datos.Lector["FechaHoraInicio"].ToString());
                    sesion.FechaHoraFin = DateTime.Parse(datos.Lector["FechaHoraFin"].ToString());
                    sesion.CantidadSeries = int.Parse(datos.Lector["CantidadSeries"].ToString());

                    if (!(datos.Lector["IdRutina"] is DBNull))
                    {
                        sesion.Rutina = new Rutina();
                        sesion.Rutina.IdRutina = int.Parse(datos.Lector["IdRutina"].ToString());
                        sesion.Rutina.Nombre = datos.Lector["NombreRutina"].ToString();
                    }

                    sesiones.Add(sesion);
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
            return sesiones;
        }

        private void AltaOModificacion(SesionEntrenamiento sesion, AccesoADatos datos)
        {
            // Creacion
            if (sesion.IdSesion == 0)
                datos.SetearConsultaSP("sp_CrearSesionEntrenamiento");
            // Modificacion
            else
            {
                datos.SetearConsultaSP("sp_ModificarSesionEntrenamiento");
                datos.setearParametro("@IdSesion", sesion.IdSesion);
            }

            datos.setearParametro("@IdUsuario", sesion.Cliente.IdUsuario);
            if (sesion.Rutina is null)
                datos.setearParametro("@IdRutina", DBNull.Value);
            else
                datos.setearParametro("@IdRutina", sesion.Rutina.IdRutina);
            datos.setearParametro("@FechaHoraInicio", sesion.FechaHoraInicio);
            datos.setearParametro("@FechaHoraFin", sesion.FechaHoraFin);

            datos.ejecutarAccion();
        }
        public void Agregar(SesionEntrenamiento nuevo)
        {
            string Excepcion = "Ocurrio un error al agregar una sesion de entrenamiento (SesionEntrenamientoNegocio.Agregar())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                AltaOModificacion(nuevo, datos);
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

        public void Modificar(SesionEntrenamiento existente)
        {
            string Excepcion = "Ocurrio un error al modificar una sesion de entrenamiento (SesionEntrenamientoNegocio.Modificar())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                AltaOModificacion(existente, datos);
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
        /// Inicializa una nueva sesion de entrenamiento anclada a un cliente,
        /// estableciendo la fecha y hora de inicio actual.
        /// </summary>
        public SesionEntrenamiento IniciarSesionEntrenamiento(Cliente cliente, Rutina rutina = null)
        {
            SesionEntrenamiento sesion = new SesionEntrenamiento();
            sesion.Cliente = cliente;
            sesion.Rutina = rutina;
            sesion.FechaHoraInicio = DateTime.Now;
            sesion.FechaHoraFin = DateTime.Now;

            sesion.IdSesion = AgregarYDevolverId(sesion);
            return sesion;
        }
        /// <summary>
        /// Cierra la sesion actual estableciendo la fecha y hora de fin
        /// </summary>
        public void FinSesionEntrenamiento(SesionEntrenamiento sesion)
        {
            sesion.FechaHoraFin = DateTime.Now;
            Modificar(sesion);
        }
        public int AgregarYDevolverId(SesionEntrenamiento nuevo)
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_CrearSesionEntrenamiento");
                datos.setearParametro("@IdUsuario", nuevo.Cliente.IdUsuario);
                datos.setearParametro("@IdRutina", (object)nuevo.Rutina?.IdRutina ?? DBNull.Value);
                datos.setearParametro("@FechaHoraInicio", nuevo.FechaHoraInicio);
                datos.setearParametro("@FechaHoraFin", nuevo.FechaHoraFin);
                return datos.EjecutarScalar();
            }
            finally { datos.cerrarConexion(); }
        }
    }
}
