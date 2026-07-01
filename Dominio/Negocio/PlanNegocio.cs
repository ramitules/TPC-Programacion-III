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
                        IdPlan = Convert.ToInt32(datos.Lector["IdPlanes"]),
                        NombrePlan = datos.Lector["Nombre"].ToString(),
                        PrecioPlan = Convert.ToSingle(datos.Lector["PrecioMensual"]),
                        DuracionDiasPlan = Convert.ToInt32(datos.Lector["DuracionDias"])
                    };
                    planes.Add(plan);
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
                    plan.IdPlan = Convert.ToInt32(datos.Lector["IdPlanes"]);
                    plan.NombrePlan = datos.Lector["Nombre"].ToString();
                    plan.PrecioPlan = Convert.ToSingle(datos.Lector["PrecioMensual"]);
                    plan.DuracionDiasPlan = Convert.ToInt32(datos.Lector["DuracionDias"]);
                    break;
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

            return plan;
        }
    }
}
