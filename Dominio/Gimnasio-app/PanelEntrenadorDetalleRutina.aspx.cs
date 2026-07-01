using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Gimnasio_app
{
    public partial class PanelEntrenadorDetalleRutina : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SessionActiva(Session["usuario"]) ||
                !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ENTRENADOR))
            {
                Response.Redirect("Default", false);
                return;
            }

            try
            {
                if (!IsPostBack)
                {
                    string idRutina = Request.QueryString["id"];
                    string origen = Request.QueryString["origen"];

                    if (string.IsNullOrEmpty(idRutina))
                    {
                        Response.Redirect("PanelEntrenador", false);
                        return;
                    }

                    Session.Add("idRutinaDetalle", idRutina);
                    Session.Add("origenDetalle", origen);

                    RutinasNegocio rutinasNegocio = new RutinasNegocio();
                    Rutina rutina = rutinasNegocio.Get(idRutina);

                    if (rutina == null)
                    {
                        Response.Redirect("PanelEntrenador", false);
                        return;
                    }

                    lblNombreRutina.Text = rutina.Nombre;
                    lblDia.Text = string.IsNullOrEmpty(rutina.Dia) ? "Sin día específico" : rutina.Dia;
                    lblFechaCreacion.Text = rutina.FechaCreacion.ToString("dd/MM/yyyy");

                    List<RutinaEjercicio> ejercicios = rutinasNegocio.GetEjerciciosDeRutina(idRutina);

                    gvEjercicios.DataSource = ejercicios;
                    gvEjercicios.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cargar el detalle de la rutina (PanelEntrenadorDetalleRutina.Page_Load()):", ex);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                string origen = Session["origenDetalle"].ToString();
                if (!string.IsNullOrEmpty(origen))
                {
                    Response.Redirect(origen + ".aspx", false);
                }
                else
                {
                    Response.Redirect("PanelEntrenador", false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al redirigir (PanelEntrenadorDetalleRutina.btnVolver_Click()):", ex);
            }
        }

        protected void btnUsarPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                string idRutina = Session["idRutinaDetalle"].ToString();
                string idCliente = Request.QueryString["idCliente"];

                if (!string.IsNullOrEmpty(idCliente))
                {
                    Response.Redirect($"PanelEntrenadorAsignarRutina.aspx?idCliente={idCliente}&idRutinaBase={idRutina}", false);
                }
                else
                {
                    Response.Redirect($"PanelEntrenadorAsignarRutina.aspx?idRutinaBase={idRutina}", false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al redirigir (PanelEntrenadorDetalleRutina.btnUsarPlantilla_Click()):", ex);
            }
        }

        protected void btnAsignarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string idRutina = Session["idRutinaDetalle"].ToString();
                Response.Redirect($"PanelEntrenadorAsignarRutina.aspx?idRutinaBase={idRutina}", false);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al redirigir (PanelEntrenadorDetalleRutina.btnAsignarCliente_Click()):", ex);
            }
        }
    }
}