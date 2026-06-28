using System;
using System.Web.UI;

namespace Gimnasio_app
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cierra la sesion del usuario y vuelve al login.
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx", false);
        }
    }
}
