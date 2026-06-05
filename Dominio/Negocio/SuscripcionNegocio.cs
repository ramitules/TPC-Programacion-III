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
        /// Obtiene la utlima suscripcion del cliente, vigente o no
        /// </summary>
        /// <param name="id"> Id de cliente</param>
        public Suscripcion GetSuscripcionCliente(string id)
        {
            string Excepcion = "Ocurrio un error al obtener la suscripcion (SuscripcionNegocio.GetSuscripcionCliente())\n";

            AccesoADatos datos = new AccesoADatos();
            Suscripcion suscripcion = new Suscripcion();
            try
            {
                datos.SetearConsultaSP("sp_SuscripcionCompleta");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    suscripcion.IdSuscripcion = (int)datos.Lector["IdSuscripcion"];
                    suscripcion.FechaInicio = (DateTime)datos.Lector["FechaInicio"];
                    suscripcion.FechaFin = (DateTime)datos.Lector["FechaVencimiento"];
                    switch ((int)datos.Lector["IdEstado"])
                    {
                        case 1:
                            suscripcion.Estado = EstadoSuscripcion.ACTIVA;
                            break;
                        case 2:
                            suscripcion.Estado = EstadoSuscripcion.VENCIDA;
                            break;
                        case 3:
                            suscripcion.Estado = EstadoSuscripcion.CANCELADA;
                            break;
                        default:
                            break;
                    }
                    suscripcion.Plan = new Plan()
                    {
                        IdPlan = (int)datos.Lector["IdPlan"],
                        NombrePlan = datos.Lector["NombrePlan"].ToString(),
                        PrecioPlan = (float)datos.Lector["PrecioPlan"],
                        DuracionDiasPlan = (int)datos.Lector["DuracionPlan"]
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
    }
}
