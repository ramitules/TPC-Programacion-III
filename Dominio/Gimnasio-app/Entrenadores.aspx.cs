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
    public partial class Profesores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            AdminNegocio adminNegocio = new AdminNegocio();
            List<Entrenador> entrenadores = new List<Entrenador>();
            try
            {
                if (!IsPostBack)
                {
                    if (Session["listaEntrenadores"] == null)
                    {
                        entrenadores = adminNegocio.ObtenerUsuarioPorRol("sp_Traer_Entrenadores").Cast<Entrenador>().ToList();
                        Session.Add("listaEntrenadores", entrenadores);
                    }
                    else
                    {
                        entrenadores = (List<Entrenador>)Session["listaEntrenadores"];
                    }
                    dgvEntrenadores.DataSource = entrenadores != null ? entrenadores : new List<Entrenador>();
                    dgvEntrenadores.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}