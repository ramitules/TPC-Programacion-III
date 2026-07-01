using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Gimnasio_app
{
    public partial class PanelEntrenadorProgresoSesionDetalle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Seguridad.SessionActiva(Session["usuario"]) || !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ENTRENADOR))
            {
                Response.Redirect("Default", false);
                return;
            }

            try
            {
                if (!IsPostBack)
                {
                    string idSesion = Request.QueryString["idSesion"];
                    string idCliente = Request.QueryString["idCliente"];

                    if (string.IsNullOrEmpty(idSesion) || string.IsNullOrEmpty(idCliente))
                    {
                        Response.Redirect("PanelEntrenador", false);
                        return;
                    }

                    Session.Add("idClienteProgresoDetalle", idCliente);
                    Session.Add("idSesionDetalle", idSesion);

                    SesionEntrenamientoNegocio sesionNegocio = new SesionEntrenamientoNegocio();
                    List<SesionEntrenamiento> sesiones = sesionNegocio.GetSesionesDeCliente(int.Parse(idCliente));
                    SesionEntrenamiento sesion = sesiones.Find(s => s.IdSesion == int.Parse(idSesion));

                    if (sesion == null)
                    {
                        Response.Redirect("PanelEntrenador", false);
                        return;
                    }

                    lblFechaSesion.Text = sesion.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm");

                    SerieCompletadaNegocio serieNegocio = new SerieCompletadaNegocio();
                    List<SeriesPorEjercicio> grupos = serieNegocio.GetSeriesAgrupadasDeSesion(int.Parse(idSesion));

                    List<SerieCompletada> todasLasSeries = new List<SerieCompletada>();
                    foreach (SeriesPorEjercicio grupo in grupos)
                    {
                        foreach (SerieCompletada serie in grupo.Series)
                        {
                            serie.Ejercicio.NombreEjercicio = grupo.NombreEjercicio;
                            serie.Ejercicio.GrupoMuscular = new GrupoMuscular { NombreGrupoMuscular = grupo.NombreGrupoMuscular };
                            todasLasSeries.Add(serie);
                        }
                    }

                    gvSeries.DataSource = todasLasSeries;
                    gvSeries.DataBind();

                    lblSinSeries.Visible = todasLasSeries.Count == 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cargar el detalle de la sesión (PanelEntrenadorProgresoSesionDetalle.Page_Load()):", ex);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                string idCliente = Session["idClienteProgresoDetalle"].ToString();
                string idSesion = Session["idSesionDetalle"].ToString();
                Response.Redirect($"PanelEntrenadorProgresoCliente.aspx?idCliente={idCliente}", false);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al redirigir al progreso del cliente (PanelEntrenadorProgresoSesionDetalle.btnVolver_Click()):", ex);
            }
        }
    }
}