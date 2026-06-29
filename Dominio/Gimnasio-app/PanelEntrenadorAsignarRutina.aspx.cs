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
    public partial class PanelEntrenadorAsignarRutina : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idCliente = Request.QueryString["idCliente"];
                if (string.IsNullOrEmpty(idCliente))
                {
                    Response.Redirect("PanelEntrenador", false); // Vuelvo sin no tengo ID.
                    return;
                }

                ClienteNegocio clienteNegocio = new ClienteNegocio();
                Cliente cliente = clienteNegocio.Get(idCliente);
                Session.Add("listaClientesEntrenador", clientes);
                lblCliente.Text = cliente.Nombre + " " + cliente.Apellido;

                EjerciciosNegocio ejerciciosNegocio = new EjerciciosNegocio();
                List<Ejercicio> ejercicios = ejerciciosNegocio.ListarEjercicios();

                ddlEjercicios.DataSource = ejercicios;
                ddlEjercicios.DataTextField = "NombreEjercicio";
                ddlEjercicios.DataValueField = "IdEjercicio";
                ddlEjercicios.DataBind();
                ddlEjercicios.Items.Insert(0, new ListItem("--Seleccionar --", "0"));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            int idEjercicio = int.Parse(ddlEjercicios.SelectedValue);
            if (idEjercicio == 0) return;

            if (Session["EjerciciosRutina"] == null)
            {
                Session["EjerciciosRutina"] = new List<RutinaEjercicio>();
            }

            List<RutinaEjercicio> listaEjercicios = (List<RutinaEjercicio>)Session["EjerciciosRutina"];

            RutinaEjercicio rutinaEjercicio = new RutinaEjercicio();
            Ejercicio ejercicio = new Ejercicio();

            ejercicio.IdEjercicio = idEjercicio;
            ejercicio.NombreEjercicio = ddlEjercicios.SelectedItem.Text;
            rutinaEjercicio.Ejercicio = ejercicio;
            rutinaEjercicio.ObjetivoKG = int.Parse(txtPeso.Text);
            rutinaEjercicio.ObjetivoSeries = int.Parse(txtSeries.Text);
            rutinaEjercicio.ObjetivoRepeticiones = int.Parse(txtRepeticiones.Text);
            rutinaEjercicio.OrdenEjercicio = listaEjercicios.Count + 1;

            listaEjercicios.Add(rutinaEjercicio);
            Session["EjerciciosRutina"] = listaEjercicios;

            gvEjercicios.DataSource = listaEjercicios;
            gvEjercicios.DataBind();
        }

        protected void btnQuitar_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrEmpty(nombre)) return;
            if (Session["EjerciciosRutina"] == null) return;

            List<RutinaEjercicio> ejercicios = (List<RutinaEjercicio>)Session["EjerciciosRutina"];
            if (ejercicios.Count == 0) return;

            int idCliente = int.Parse(Session["idClienteAsignar"].ToString());

            RutinasNegocio rutinasNegocio = new RutinasNegocio();
            rutinasNegocio.CrearRutinaParaCliente(idCliente, nombre, ejercicios);

            lblAgregar.Text = "Rutina asignada OK";
            lblAgregar.Visible = true;

            Session["EjerciciosRutina"] = null;
            txtNombre.Text = "";
            gvEjercicios.DataSource = null;
            gvEjercicios.DataBind();
        }
    }
}