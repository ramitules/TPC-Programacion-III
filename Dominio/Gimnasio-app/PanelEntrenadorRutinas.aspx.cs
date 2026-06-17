using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Gimnasio_app
{
    public partial class PanelEntrenadorRutinas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RutinasNegocio negocio = new RutinasNegocio();
                gvRutinas.DataSource = negocio.GetRutinasGenerales();
                gvRutinas.DataBind();
            }
        }
    }
}