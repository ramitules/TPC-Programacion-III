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
            try
            {
                dgvAdmins.DataSource = adminNegocio.ObtenerUsuarioPorRol("sp_Traer_Admins");
                dgvAdmins.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
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