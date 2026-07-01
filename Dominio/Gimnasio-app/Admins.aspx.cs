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
    public partial class Admins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ADMIN)))
            {
                Response.Redirect("Login.aspx", false);
            }
            AdminNegocio adminNegocio = new AdminNegocio();
            List<Admin> admins = new List<Admin>();
            try
            {
                if (!IsPostBack)
                {
                    if (Session["listaAdmins"] == null)
                    {
                        admins = adminNegocio.ObtenerUsuarioPorRol("sp_Traer_Admins").Cast<Admin>().ToList();
                        Session.Add("listaAdmins", admins);
                    }
                    else
                    {
                        admins = (List<Admin>)Session["listaAdmins"];
                    }
                    dgvAdmins.DataSource = admins != null ? admins : new List<Admin>();
                    dgvAdmins.DataBind();
                }
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
                Response.Redirect("FormularioAdmins.aspx?IdRol=" + (int)Roles.ADMIN, false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Admin> filtrado;
            try
            {
                if (Session["listaAdmins"] != null)
                {
                    List<Admin> admins = (List<Admin>)Session["listaAdmins"];
                    filtrado = admins.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
                }
                else
                {
                    filtrado = new List<Admin>();
                }
                dgvAdmins.DataSource = filtrado != null ? filtrado : new List<Admin>();
                dgvAdmins.DataBind();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Admin> filtrado;
            bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            if (Session["listaAdmins"] != null)
            {
                List<Admin> admins = (List<Admin>)Session["listaAdmins"];
                if (ddlEstado.SelectedValue == "todos")
                {
                    filtrado = admins;
                }
                else
                {
                    filtrado = admins.FindAll(a => a.Activo == estadoSeleccionado);
                }
                dgvAdmins.DataSource = filtrado != null ? filtrado : new List<Admin>();
                dgvAdmins.DataBind();
            }
        }
    }
}