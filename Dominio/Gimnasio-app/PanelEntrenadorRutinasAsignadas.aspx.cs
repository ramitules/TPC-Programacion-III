using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using Integraciones;

namespace Gimnasio_app
{
    public partial class PanelEntrenadorRutinasAsignadas : Page
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
                    RutinasNegocio rutinasNegocio = new RutinasNegocio();
                    List<Rutina> rutinas = rutinasNegocio.GetRutinasAsignadas();

                    gvRutinasAsignadas.DataSource = rutinas;
                    gvRutinasAsignadas.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cargar las rutinas asignadas (RutinasAsignadas.Page_Load()):", ex);
            }
        }
    }
}