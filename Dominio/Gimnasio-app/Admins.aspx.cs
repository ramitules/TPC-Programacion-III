using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class Admins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminNegocio adminNegocio = new AdminNegocio();
            List<Usuario> admins = new List<Usuario>();
            try
            {
                if (!IsPostBack)
                {
                    if (Session["litaAdmins"] == null)
                    {
                        admins = adminNegocio.ObtenerUsuarioPorRol("sp_Traer_Admins");
                        Session.Add("listaAdmins", admins);
                    }
                    else
                    {
                        admins = (List<Usuario>)Session["listaAdmins"];
                    }
                    dgvAdmins.DataSource = admins != null ? admins : new List<Usuario>();
                    dgvAdmins.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("FormularioAdmins", false);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}