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
                    if (Session["listaAdmins"] == null)
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

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Usuario> filtrado;
            try
            {
                if (Session["listaAdmins"] != null)
                {
                    List<Usuario> admins = (List<Usuario>)Session["listaAdmins"];
                    filtrado = admins.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
                }
                else
                {
                    filtrado = new List<Usuario>();
                }
                dgvAdmins.DataSource = filtrado != null ? filtrado : new List<Usuario>();
                dgvAdmins.DataBind();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Usuario> filtrado;
            bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            if (Session["listaAdmins"] != null)
            {
                List<Usuario> admins = (List<Usuario>)Session["listaAdmins"];
                if (ddlEstado.SelectedValue == "todos")
                {
                    filtrado = admins;
                }
                else
                {
                    filtrado = admins.FindAll(a => a.Activo == estadoSeleccionado);
                }
                dgvAdmins.DataSource = filtrado != null ? filtrado : new List<Usuario>();
                dgvAdmins.DataBind();
            }
        }
    }
}