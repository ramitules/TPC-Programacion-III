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
                    //listaEjercicios = negocio.listarEjercicios();
                    listaGruposMusculares = negocio.listarGruposMusculares();
                    listaPlanes = negocio.listarPlanes();                    
                    //Session.Add("listaEjercicios", listaEjercicios);
                    Session.Add("listaGruposMusculares", listaGruposMusculares);
                    Session.Add("listaPlanes", listaPlanes);
                }
                //dgvEjercicios.DataSource = listaEjercicios;
                //dgvEjercicios.DataBind();
                dgvGruposMusculares.DataSource = listaGruposMusculares;
                dgvGruposMusculares.DataBind();
                dgvPlanes.DataSource = listaPlanes;
                dgvPlanes.DataBind();
            }
        }

        protected void btnNuevoEjercicio_Click(object sender, EventArgs e)
        {

        }

        protected void dgvEjercicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void dgvGruposMusculares_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnNuevoMusculo_Click(object sender, EventArgs e)
        {

        }

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {

        }

        protected void dgvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}