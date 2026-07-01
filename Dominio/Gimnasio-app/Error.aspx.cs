using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["error"] != null)
                Session.Remove("error");

            lblMensaje.Text = "Ocurrió un error inesperado. Por favor, intentá nuevamente más tarde.";
        }
    }
}