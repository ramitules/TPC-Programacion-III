using Integraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Gimnasio_app
{
    public partial class AdminControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ADMIN)) 
            {
                Response.Redirect("~/Login.aspx", false);
            }
            if (!IsPostBack)
            {
                List<Ejercicio> listaEjercicios;
                List<GrupoMuscular> listaGruposMusculares;
                List<Plan> listaPlanes;
                if (Session["listaEjercicios"] != null && Session["listaGruposMusculares"] != null && Session["listaPlanes"] != null)
                {
                    listaEjercicios = (List<Ejercicio>)Session["listaEjercicios"];
                    listaGruposMusculares = (List<GrupoMuscular>)Session["listaGruposMusculares"];
                    listaPlanes = (List<Plan>)Session["listaPlanes"];                    
                }
                else
                {
                    AdminNegocio negocio = new AdminNegocio();
                    listaEjercicios = negocio.listarEjercicios();
                    listaGruposMusculares = negocio.listarGruposMusculares();
                    listaPlanes = negocio.listarPlanes();                    
                    Session.Add("listaEjercicios", listaEjercicios);
                    Session.Add("listaGruposMusculares", listaGruposMusculares);
                    Session.Add("listaPlanes", listaPlanes);
                }
                dgvEjercicios.DataSource = listaEjercicios;
                dgvEjercicios.DataBind();
                dgvGruposMusculares.DataSource = listaGruposMusculares;
                dgvGruposMusculares.DataBind();
                dgvPlanes.DataSource = listaPlanes;
                dgvPlanes.DataBind();
            }
        }

        protected void btnNuevoEjercicio_Click(object sender, EventArgs e)
        {
            pnlFormularioABM.Visible = true;
            lblTituloForm.Text = "Nuevo Ejercicio";
            divCamposEjercicio.Visible = true;
            divCamposPlan.Visible = false;
        }

        protected void dgvEjercicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string accion = e.CommandName.ToString();
            int idEjercicio = Convert.ToInt32(e.CommandArgument);
            Ejercicio ejercicioSeleccionado = ((List<Ejercicio>)Session["listaEjercicios"]).Find(x => x.IdEjercicio == idEjercicio);
            if (accion == "Editar")
            {
                pnlFormularioABM.Visible = true;
                lblTituloForm.Text = "Editar Ejercicio";
                divCamposEjercicio.Visible = true;
                divCamposPlan.Visible = false;
                txtNombre.Text = ejercicioSeleccionado.NombreEjercicio;
                txtLink.Text = ejercicioSeleccionado.LinkExplicacion;
                ddlGrupoMuscular.DataSource = ((List<GrupoMuscular>)Session["listaGruposMusculares"]);
                ddlGrupoMuscular.DataBind();
                ddlGrupoMuscular.SelectedValue = ejercicioSeleccionado.GrupoMuscular.IdGrupoMuscular.ToString();
            }
            else if (accion == "Eliminar")
            {
                //AdminNegocio negocio = new AdminNegocio();
                //negocio.eliminarEjercicio(idEjercicio);
                //List<Ejercicio> listaEjercicios = negocio.listarEjercicios();
                //Session["listaEjercicios"] = listaEjercicios;
                //dgvEjercicios.DataSource = listaEjercicios;
                //dgvEjercicios.DataBind();
            }
        }

        protected void dgvGruposMusculares_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnNuevoMusculo_Click(object sender, EventArgs e)
        {
            pnlFormularioABM.Visible = true;
            lblTituloForm.Text = "Nuevo Grupo Muscular";
            divCamposEjercicio.Visible = false;
            divCamposPlan.Visible = false;
        }

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {
            pnlFormularioABM.Visible = true;
            lblTituloForm.Text = "Nuevo Plan";
            divCamposEjercicio.Visible = false;
            divCamposPlan.Visible = true;
        }

        protected void dgvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}