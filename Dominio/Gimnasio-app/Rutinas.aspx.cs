using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class Rutinas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SessionActiva(Session["usuario"]) ||
                !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.CLIENTE))
            {
                Response.Redirect("Default", false);
                return;
            }
            if (!IsPostBack)
            {
                Cliente cliente = (Cliente)Session["usuario"];
                RutinasNegocio negocio = new RutinasNegocio();
                rptPropias.DataSource = negocio.AgruparPorDia(negocio.GetRutinasUsuario(cliente));
                rptPropias.DataBind();

                rptGenerales.DataSource = negocio.AgruparPorDia(negocio.GetRutinasGenerales());
                rptGenerales.DataBind();
            }
        }
    }
}