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
            //  TESTING
            Session.Add("cliente", new ClienteNegocio().Get("9", true));
            //  TESTING

            if (!(Seguridad.SessionActiva(Session["cliente"])))
            {
                Response.Redirect("Default", false);
                return;
            }
            if (!IsPostBack)
            {
                Cliente cliente = (Cliente)Session["cliente"];
                RutinasNegocio negocio = new RutinasNegocio();
                rptPropias.DataSource = negocio.AgruparPorDia(negocio.GetRutinasUsuario(cliente));
                rptPropias.DataBind();

                rptGenerales.DataSource = negocio.AgruparPorDia(negocio.GetRutinasGenerales());
                rptGenerales.DataBind();
            }
        }
    }
}