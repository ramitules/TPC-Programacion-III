using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PlanNegocio
    {
        public List<Plan> GetPlanes()
        {
            string Excepcion = "Ocurrio un error al obtener los planes (PlanNegocio.GetPlanes())\n";

            AccesoADatos datos = new AccesoADatos();
            List<Plan> planes = new List<Plan>();

            try
            {
                datos.SetearConsulta("SELECT * FROM Planes");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Plan plan = new Plan()
                    {
                        IdPlan = int.Parse(datos.Lector["IdPlanes"].ToString()),
                        NombrePlan = datos.Lector["Nombre"].ToString(),
                        PrecioPlan = float.Parse(datos.Lector["PrecioMensual"].ToString()),
                        DuracionDiasPlan = int.Parse(datos.Lector["DuracionDias"].ToString())
                    };
                    planes.Add(plan);
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

            return planes;
        }
        public Plan GetPlan(string ID)
        {
            string Excepcion = "Ocurrio un error al obtener el plan (PlanNegocio.GetPlan())\n";

            AccesoADatos datos = new AccesoADatos();
            Plan plan = new Plan();

            try
            {
                datos.SetearConsulta("SELECT * FROM Planes WHERE IdPlanes = @ID");
                datos.setearParametro("@ID", ID);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    plan.IdPlan = int.Parse(datos.Lector["IdPlanes"].ToString());
                    plan.NombrePlan = datos.Lector["Nombre"].ToString();
                    plan.PrecioPlan = float.Parse(datos.Lector["PrecioMensual"].ToString());
                    plan.DuracionDiasPlan = int.Parse(datos.Lector["DuracionDias"].ToString());
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

            return plan;
        }
    }
}
