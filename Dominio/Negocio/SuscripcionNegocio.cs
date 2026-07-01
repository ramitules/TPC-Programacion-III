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
                    suscripcion.IdSuscripcion = Convert.ToInt32(datos.Lector["IdSuscripcion"]);
                    suscripcion.FechaInicio = Convert.ToDateTime(datos.Lector["FechaInicio"]);
                    suscripcion.FechaFin = Convert.ToDateTime(datos.Lector["FechaVencimiento"]);
                    suscripcion.Estado = estado;
                    suscripcion.Plan = new Plan()
                    {
                        IdPlan = Convert.ToInt32(datos.Lector["IdPlan"]),
                        NombrePlan = datos.Lector["NombrePlan"].ToString(),
                        PrecioPlan = Convert.ToSingle(datos.Lector["PrecioPlan"]),
                        DuracionDiasPlan = Convert.ToInt32(datos.Lector["DuracionPlan"])
                    };
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
                throw new Exception(Excepcion, ex);
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
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool BajaSuscripcionCliente(Cliente cliente)
        {
            string Excepcion = "Ocurrio un error al dar de baja una suscripcion (SuscripcionNegocio.BajaSuscripcionCliente())\n";

            SuscripcionNegocio negocio = new SuscripcionNegocio();
            Suscripcion suscripcionActual = negocio.GetSuscripcionCliente(cliente.IdUsuario.ToString(), EstadoSuscripcion.ACTIVA);

            try
            {
                if (suscripcionActual.IdSuscripcion != 0)
                {
                    suscripcionActual.Estado = EstadoSuscripcion.CANCELADA;
                    negocio.Modificar(suscripcionActual, cliente);

                    // Si habia una suscripcion pendiente, entra en vigencia de inmediato
                    // (se recalculan sus fechas ya que se anticipa su inicio).
                    Suscripcion suscripcionPendiente = negocio.GetSuscripcionCliente(cliente.IdUsuario.ToString(), EstadoSuscripcion.VIGENTE_PENDIENTE);
                    if (suscripcionPendiente.IdSuscripcion != 0)
                    {
                        suscripcionPendiente.Estado = EstadoSuscripcion.ACTIVA;
                        suscripcionPendiente.FechaInicio = DateTime.Now;
                        suscripcionPendiente.FechaFin = suscripcionPendiente.FechaInicio.AddDays(suscripcionPendiente.Plan.DuracionDiasPlan);
                        negocio.Modificar(suscripcionPendiente, cliente);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }

            return false;
        }
    }
}
