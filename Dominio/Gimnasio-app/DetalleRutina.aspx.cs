using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Web.UI;

namespace Gimnasio_app
{
    public partial class DetalleRutina : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Seguridad.SessionActiva(Session["cliente"])))
            {
                Response.Redirect("Default", false);
                return;
            }

            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];
                if (string.IsNullOrEmpty(id))
                {
                    Response.Redirect("Rutinas", false);
                    return;
                }

                RutinasNegocio negocio = new RutinasNegocio();
                Rutina rutina = negocio.Get(id);

                if (rutina == null)
                {
                    Response.Redirect("Rutinas", false);
                    return;
                }

                rutina.Ejercicios = negocio.GetEjerciciosDeRutina(id);

                litNombre.Text = rutina.Nombre;
                litDia.Text = string.IsNullOrEmpty(rutina.Dia) ? "Día libre" : rutina.Dia;
                litFecha.Text = rutina.FechaCreacion.ToString("dd/MM/yyyy");

                rptEjercicios.DataSource = rutina.Ejercicios;
                rptEjercicios.DataBind();

                phSinEjercicios.Visible = rutina.Ejercicios.Count == 0;
            }
        }
    }
}
