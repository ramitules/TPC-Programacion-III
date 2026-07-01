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
    public partial class Recepcionistas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ADMIN)))
            {
                Response.Redirect("Login.aspx", false);
            }
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
                        recepcionistas = ((List<Recepcionista>)Session["listaRecepcionistas"]);
                    }
                    dgvRecepcionistas.DataSource = recepcionistas != null ? recepcionistas : new List<Recepcionista>();
                    dgvRecepcionistas.DataBind();
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
                Response.Redirect("FormularioAdmins.aspx?IdRol=" + (int)Roles.RECEPCIONISTA, false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            aplicarFiltros();
            //List<Recepcionista> filtrado;
            //try
            //{
            //    if (Session["listaRecepcionistas"] != null)
            //    {
            //        List<Recepcionista> recepcionistas = (List<Recepcionista>)Session["listaRecepcionistas"];
            //        filtrado = recepcionistas.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
            //    }
            //    else
            //    {
            //        filtrado = new List<Recepcionista>();
            //    }
            //    dgvRecepcionistas.DataSource = filtrado != null ? filtrado : new List<Recepcionista>();
            //    dgvRecepcionistas.DataBind();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            aplicarFiltros();
            //List<Recepcionista> filtrado;
            //bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            //if (Session["listaRecepcionistas"] != null)
            //{
            //    List<Recepcionista> recepcionistas = (List<Recepcionista>)Session["listaRecepcionistas"];
            //    if (ddlEstado.SelectedValue == "todos")
            //    {
            //        filtrado = recepcionistas;
            //    }
            //    else
            //    {
            //        filtrado = recepcionistas.FindAll(a => a.Activo == estadoSeleccionado);
            //    }
            //    dgvRecepcionistas.DataSource = filtrado != null ? filtrado : new List<Recepcionista>();
            //    dgvRecepcionistas.DataBind();
            //}
        }

        public void aplicarFiltros()
        {
            List<Recepcionista> filtrado;
            bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            try
            {
                if (Session["listaRecepcionistas"] != null)
                {
                    List<Recepcionista> recepcionistas = (List<Recepcionista>)Session["listaRecepcionistas"];
                    filtrado = recepcionistas;
                    if (ddlEstado.SelectedValue != "todos")
                    {
                        filtrado = filtrado.FindAll(a => a.Activo == estadoSeleccionado);
                    }
                    if (txtBuscar.Text != "")
                    {
                        filtrado = filtrado.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
                    }
                    dgvRecepcionistas.DataSource = filtrado != null ? filtrado : new List<Recepcionista>();
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