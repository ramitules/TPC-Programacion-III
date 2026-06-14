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
        /// Obtiene la sesion de entrenamiento segun su ID, o la ultima sesion agregada si ID == ""
        /// </summary>
        public SesionEntrenamiento Get(string id = "")
        {
            string Excepcion = "Ocurrio un error al obtener la sesion de entrenamiento (SesionEntrenamientoNegocio.Get())\n";

            AccesoADatos datos = new AccesoADatos();
            SesionEntrenamiento sesion = new SesionEntrenamiento();
            try
            {
                if (id != "")
                {
                    datos.SetearConsulta("SELECT * FROM SesionesEntrenamiento WHERE IdSesionesEntrenamiento = @ID");
                    datos.setearParametro("@ID", id);
                }
                else
                {
                    datos.SetearConsulta("SELECT TOP 1 * FROM SesionesEntrenamiento ORDER BY IdSesionesEntrenamiento DESC");
                }

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    sesion.IdSesion = int.Parse(datos.Lector["IdSesionesEntrenamiento"].ToString());
                    sesion.FechaHoraInicio = DateTime.Parse(datos.Lector["FechaHoraInicio"].ToString());
                    sesion.FechaHoraFin = DateTime.Parse(datos.Lector["FechaHoraFin"].ToString());
                    sesion.Cliente = new ClienteNegocio().Get(datos.Lector["IdUsuario"].ToString());

                    if (!(datos.Lector["IdRutina"] is null))
                        sesion.Rutina = new RutinasNegocio().Get(datos.Lector["IdRutina"].ToString());
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
            return sesion;
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
                throw new Exception(Excepcion + ex.ToString());
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
                throw new Exception(Excepcion + ex.ToString());
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

            Agregar(sesion);
            return Get();
        }
        /// <summary>
        /// Cierra la sesion actual estableciendo la fecha y hora de fin
        /// </summary>
        public void FinSesionEntrenamiento(SesionEntrenamiento sesion)
        {
            sesion.FechaHoraFin = DateTime.Now;
            Modificar(sesion);
        }
    }
}
