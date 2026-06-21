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
    public partial class Recepcionistas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminNegocio adminNegocio = new AdminNegocio();
            List<Recepcionista> recepcionistas = new List<Recepcionista>();
            try
            {
                if (!IsPostBack)
                {
                    if (Session["listaRecepcionistas"] == null)
                    {
                        recepcionistas = adminNegocio.ObtenerUsuarioPorRol("sp_Traer_Recepcionistas").Cast<Recepcionista>().ToList();
                        Session.Add("listaRecepcionistas", recepcionistas);
                    }
                    else
                    {
                        recepcionistas = (List<Recepcionista>)Session["listaRecepcionistas"];
                    }
                    dgvRecepcionistas.DataSource = recepcionistas != null ? recepcionistas : new List<Recepcionista>();
                    dgvRecepcionistas.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}