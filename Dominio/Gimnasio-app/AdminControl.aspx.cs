using Integraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;

namespace Gimnasio_app
{
    public partial class AdminControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ADMIN)) 
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
    }
}