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

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlEstado_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            aplicarFiltros();
            //List<Entrenador> filtrado;
            //try
            //{
            //    if (Session["listaEntrenadores"] != null)
            //    {
            //        List<Entrenador> entrenadores = (List<Entrenador>)Session["listaEntrenadores"];
            //        filtrado = entrenadores.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
            //    }
            //    else
            //    {
            //        filtrado = new List<Entrenador>();
            //    }
            //    dgvEntrenadores.DataSource = filtrado != null ? filtrado : new List<Entrenador>();
            //    dgvEntrenadores.DataBind();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            aplicarFiltros();
            //List<Entrenador> filtrado;
            //bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            //if (Session["listaEntrenadores"] != null)
            //{
            //    List<Entrenador> entrenadores = (List<Entrenador>)Session["listaEntrenadores"];
            //    if (ddlEstado.SelectedValue == "todos")
            //    {
            //        filtrado = entrenadores;
            //    }
            //    else
            //    {
            //        filtrado = entrenadores.FindAll(a => a.Activo == estadoSeleccionado);
            //    }
            //    dgvEntrenadores.DataSource = filtrado != null ? filtrado : new List<Entrenador>();
            //    dgvEntrenadores.DataBind();
            //}
        }

        public void aplicarFiltros()
        {
            List<Entrenador> filtrado;
            bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            try
            {
                if (Session["listaEntrenadores"] != null)
                {
                    List<Entrenador> entrenadores = (List<Entrenador>)Session["listaEntrenadores"];
                    filtrado = entrenadores;
                    if (ddlEstado.SelectedValue != "todos")
                    {
                        filtrado = filtrado.FindAll(a => a.Activo == estadoSeleccionado);
                    }
                    if (txtBuscar.Text != "")
                    {
                        filtrado = filtrado.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
                    }
                    dgvEntrenadores.DataSource = filtrado != null ? filtrado : new List<Entrenador>();
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