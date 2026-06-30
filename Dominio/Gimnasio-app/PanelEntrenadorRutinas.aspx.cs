using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Integraciones;

namespace Gimnasio_app
{
    public partial class PanelEntrenadorRutinas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SessionActiva(Session["usuario"]) || !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ENTRENADOR))
            {
                Response.Redirect("Default", false);
                return;
            }

            if (!IsPostBack)
            {
                RutinasNegocio negocio = new RutinasNegocio();
                gvRutinas.DataSource = negocio.GetRutinasGenerales();
                gvRutinas.DataBind();
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer; //capturo fila
            int idRutina = int.Parse(gvRutinas.DataKeys[row.RowIndex].Value.ToString());

            RutinasNegocio negocio = new RutinasNegocio();
            negocio.EliminarRutina(idRutina);

            gvRutinas.DataSource = negocio.GetRutinasGenerales();
            gvRutinas.DataBind();
        }
    }
}