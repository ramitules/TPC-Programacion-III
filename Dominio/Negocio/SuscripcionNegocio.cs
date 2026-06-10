using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class SuscripcionNegocio
    {
        /// <summary>
        /// Obtiene la utlima suscripcion del cliente filtrando por el estado de la suscripcion
        /// </summary>
        /// <param name="id"> Id de cliente</param>
        public Suscripcion GetSuscripcionCliente(string id, EstadoSuscripcion estado)
        {
            string Excepcion = "Ocurrio un error al obtener la suscripcion (SuscripcionNegocio.GetSuscripcionCliente())\n";

            AccesoADatos datos = new AccesoADatos();
            Suscripcion suscripcion = new Suscripcion();

            try
            {
                datos.SetearConsultaSP("sp_SuscripcionCompleta");
                datos.setearParametro("@Id", id);
                datos.setearParametro("@IdEstado", estado);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    suscripcion.IdSuscripcion = int.Parse(datos.Lector["IdSuscripcion"].ToString());
                    suscripcion.FechaInicio = DateTime.Parse(datos.Lector["FechaInicio"].ToString());
                    suscripcion.FechaFin = DateTime.Parse(datos.Lector["FechaVencimiento"].ToString());
                    suscripcion.Estado = estado;
                    suscripcion.Plan = new Plan()
                    {
                        IdPlan = int.Parse(datos.Lector["IdPlan"].ToString()),
                        NombrePlan = datos.Lector["NombrePlan"].ToString(),
                        PrecioPlan = float.Parse(datos.Lector["PrecioPlan"].ToString()),
                        DuracionDiasPlan = int.Parse(datos.Lector["DuracionPlan"].ToString())
                    };
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

            return suscripcion;
        }
        private void AltaOModificacion(Suscripcion suscripcion, AccesoADatos datos, Cliente cliente)
        {
            // Creacion
            if (suscripcion.IdSuscripcion == 0)
                datos.SetearConsultaSP("sp_CrearSuscripcion");
            // Modificacion
            else
            {
                datos.SetearConsultaSP("sp_ModificarSuscripcion");
                datos.setearParametro("@IdSuscripcion", suscripcion.IdSuscripcion);
            }

            datos.setearParametro("@IdUsuario", cliente.IdUsuario);
            datos.setearParametro("@IdPlan", suscripcion.Plan.IdPlan);
            datos.setearParametro("@IdEstado", suscripcion.Estado);
            datos.setearParametro("@FechaInicio", suscripcion.FechaInicio);
            datos.setearParametro("@FechaVencimiento", suscripcion.FechaFin);

            datos.ejecutarAccion();
        }
        public void Agregar(Suscripcion suscripcion, Cliente cliente)
        {
            string Excepcion = "Ocurrio un error al agregar una suscripcion (SuscripcionNegocio.Agregar())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                AltaOModificacion(suscripcion, datos, cliente);
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
        public void Modificar(Suscripcion suscripcion, Cliente cliente)
        {
            string Excepcion = "Ocurrio un error al modificar una suscripcion (SuscripcionNegocio.Modificar())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                AltaOModificacion(suscripcion, datos, cliente);
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
