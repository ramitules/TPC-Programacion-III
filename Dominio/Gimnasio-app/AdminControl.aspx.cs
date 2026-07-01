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


        protected void dgvEjercicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string accion = e.CommandName.ToString();
            int idEjercicio = Convert.ToInt32(e.CommandArgument);
            AdminNegocio negocio = new AdminNegocio();
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

                hfTipoEntidad.Value = "Ejercicio";
                hfIdEntidad.Value = idEjercicio.ToString();
            }
            else if (accion == "Eliminar")
            {
                negocio.eliminarEjercicio(ejercicioSeleccionado);
            }
            List<Ejercicio> listaEjercicios = negocio.listarEjercicios();
            Session["listaEjercicios"] = listaEjercicios;   
            dgvEjercicios.DataSource = listaEjercicios;
            dgvEjercicios.DataBind();
            pnlFormularioABM.Visible = false;

        }

        protected void dgvGruposMusculares_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string accion = e.CommandName.ToString();
            int idGrupoMuscular = Convert.ToInt32(e.CommandArgument);
            AdminNegocio negocio = new AdminNegocio();
            GrupoMuscular grupoMuscularSeleccionado = ((List<GrupoMuscular>)Session["listaGruposMusculares"]).Find(x => x.IdGrupoMuscular == idGrupoMuscular);
            if (accion == "Editar")
            {
                pnlFormularioABM.Visible = true;
                lblTituloForm.Text = "Editar Grupo Muscular";
                divCamposEjercicio.Visible = false;
                divCamposPlan.Visible = false;
                txtNombre.Text = grupoMuscularSeleccionado.NombreGrupoMuscular;

                hfTipoEntidad.Value = "Grupos Musculares";
                hfIdEntidad.Value = idGrupoMuscular.ToString();
            }
            else if (accion == "Eliminar")
            {
                negocio.eliminarGrupoMuscular(grupoMuscularSeleccionado);
            }
            List<GrupoMuscular> listaGruposMusculares = negocio.listarGruposMusculares();
            Session["listaEjercicios"] = listaGruposMusculares;
            dgvGruposMusculares.DataSource = listaGruposMusculares;
            dgvGruposMusculares.DataBind();
            pnlFormularioABM.Visible = false;
        }

        protected void dgvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string accion = e.CommandName.ToString();
            int idPlan = Convert.ToInt32(e.CommandArgument);
            AdminNegocio negocio = new AdminNegocio();
            Plan planSeleccionado = ((List<Plan>)Session["listaPlanes"]).Find(x => x.IdPlan == idPlan);
            if (accion == "Editar")
            {
                pnlFormularioABM.Visible = true;
                lblTituloForm.Text = "Editar Plan";
                divCamposPlan.Visible = true;
                divCamposEjercicio.Visible = false;
                txtNombre.Text = planSeleccionado.NombrePlan;

                hfTipoEntidad.Value = "Planes ";
                hfIdEntidad.Value = idPlan.ToString();
            }
            else if (accion == "Eliminar")
            {
                negocio.eliminarPlan(planSeleccionado);
            }
            List<Plan> listaPlan = negocio.listarPlanes();
            Session["listaPlanes"] = listaPlan;
            dgvPlanes.DataSource = listaPlan;
            dgvPlanes.DataBind();
            pnlFormularioABM.Visible = false;
        }
        protected void btnNuevoEjercicio_Click(object sender, EventArgs e)
        {
            //pnlFormularioABM.Visible = true;
            //lblTituloForm.Text = "Nuevo Ejercicio";
            //divCamposEjercicio.Visible = true;
            //divCamposPlan.Visible = false;

            lblTituloForm.Text = "Nuevo Ejercicio";
            txtNombre.Text = ""; 
            txtLink.Text = "";

            ddlGrupoMuscular.DataSource = (List<GrupoMuscular>)Session["listaGruposMusculares"];
            ddlGrupoMuscular.DataBind();

            hfTipoEntidad.Value = "Ejercicio";
            hfIdEntidad.Value = "";

            divCamposEjercicio.Visible = true; 
            divCamposPlan.Visible = false;
            pnlFormularioABM.Visible = true;
        }
        
        protected void btnNuevoMusculo_Click(object sender, EventArgs e)
        {
            //pnlFormularioABM.Visible = true;
            //lblTituloForm.Text = "Nuevo Grupo Muscular";
            //divCamposEjercicio.Visible = false;
            //divCamposPlan.Visible = false;

            lblTituloForm.Text = "Nuevo Grupo Muscular";
            txtNombre.Text = "";
            hfTipoEntidad.Value = "Musculo";
            hfIdEntidad.Value = "";
            divCamposEjercicio.Visible = false; 
            divCamposPlan.Visible = false;
            pnlFormularioABM.Visible = true;
        }

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {
            //pnlFormularioABM.Visible = true;
            //lblTituloForm.Text = "Nuevo Plan";
            //divCamposEjercicio.Visible = false;
            //divCamposPlan.Visible = true;

            lblTituloForm.Text = "Nuevo Plan";
            txtNombre.Text = ""; 
            txtPrecio.Text = ""; 
            txtDias.Text = "";
            hfTipoEntidad.Value = "Plan";
            hfIdEntidad.Value = "";
            divCamposEjercicio.Visible = false; divCamposPlan.Visible = true;
            pnlFormularioABM.Visible = true;
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string pestania = hfTipoEntidad.Value; // "Ejercicio", "Musculo" o "Plan"
            string id = hfIdEntidad.Value;
            bool editar = !string.IsNullOrEmpty(id);
            int idEntidad = editar ? Convert.ToInt32(id) : 0;

            AdminNegocio negocio = new AdminNegocio();

            try
            {
                // 2. Evaluamos qué objeto estamos guardando
                switch (pestania)
                {
                    case "Ejercicio":
                        Ejercicio ejer = new Ejercicio();
                        if (editar) ejer.IdEjercicio = idEntidad;
                        ejer.NombreEjercicio = txtNombre.Text.Trim();
                        ejer.LinkExplicacion = txtLink.Text.Trim();

                        // Armamos el objeto relacionado con el valor seleccionado en el combo
                        ejer.GrupoMuscular = new GrupoMuscular { IdGrupoMuscular = Convert.ToInt32(ddlGrupoMuscular.SelectedValue) };

                        if (editar)
                            negocio.modificarEjercicio(ejer); // O el nombre de tu método de UPDATE
                        else
                            negocio.crearEjercicio(ejer);     // O el nombre de tu método de INSERT

                        // Recargamos la grilla y la sesión de Ejercicios
                        Session["listaEjercicios"] = negocio.listarEjercicios();
                        dgvEjercicios.DataSource = Session["listaEjercicios"];
                        dgvEjercicios.DataBind();
                        break;

                    case "Musculo":
                        GrupoMuscular grupo = new GrupoMuscular();
                        if (editar) grupo.IdGrupoMuscular = idEntidad;
                        grupo.NombreGrupoMuscular = txtNombre.Text.Trim();

                        if (editar)
                            negocio.modificarGrupoMuscular(grupo);
                        else
                            negocio.crearGrupoMuscular(grupo);

                        // Recargamos la grilla y la sesión de Músculos
                        Session["listaGruposMusculares"] = negocio.listarGruposMusculares();
                        dgvGruposMusculares.DataSource = Session["listaGruposMusculares"];
                        dgvGruposMusculares.DataBind();
                        break;

                    case "Plan":
                        Plan plan = new Plan();
                        if (editar) plan.IdPlan = idEntidad;
                        plan.NombrePlan = txtNombre.Text.Trim();
                        plan.PrecioPlan = Convert.ToSingle(txtPrecio.Text.Trim());
                        plan.DuracionDiasPlan = Convert.ToInt32(txtDias.Text.Trim());

                        if (editar)
                            negocio.modificarPlan(plan);
                        else
                            negocio.crearPlan(plan);

                        // Recargamos la grilla y la sesión de Planes (corregido el nombre de la sesión)
                        Session["listaPlanes"] = negocio.listarPlanes();
                        dgvPlanes.DataSource = Session["listaPlanes"];
                        dgvPlanes.DataBind();
                        break;
                }

                // 3. Todo salió bien: Ocultamos el panel ABM y limpiamos los inputs
                restaurarFormulario();

                // 4. Mantenemos fija la pestaña actual de Bootstrap usando JavaScript
                dejarPestañaActiva(pestania);
            }
            catch (Exception ex)
            {
                // En caso de error (ej. error de conversión de datos o BD), mostramos un alert sin romper el diseño
                string script = $"alert('Error al guardar los cambios: {ex.Message.Replace("'", "\\'")}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorABM", script, true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            string tipoEntidadActual = hfTipoEntidad.Value;

            // Ocultamos el panel y limpiamos los controles
            restaurarFormulario();

            // Dejamos al usuario parado en la misma pestaña que estaba mirando
            dejarPestañaActiva(tipoEntidadActual);
        }
   

        private void restaurarFormulario()
        {
            // Ocultamos el contenedor principal de edición
            pnlFormularioABM.Visible = false;

            // Limpiamos los cuadros de texto comunes y específicos
            txtNombre.Text = "";
            txtLink.Text = "";
            txtPrecio.Text = "";
            txtDias.Text = "";

            // Vaciamos las variables de estado de los HiddenFields
            hfIdEntidad.Value = "";
            hfTipoEntidad.Value = "";
        }

        private void dejarPestañaActiva(string tipoEntidad)
        {
            string targetTabId = "";

            // Mapeamos el tipo de entidad con el ID del <div> de tu HTML
            switch (tipoEntidad)
            {
                case "Ejercicio": targetTabId = "ejercicios"; break;
                case "Musculo": targetTabId = "musculos"; break;
                case "Plan": targetTabId = "planes"; break;
            }

            if (!string.IsNullOrEmpty(targetTabId))
            {
                // Script JS para decirle a Bootstrap 5 que fuerce la visualización de la pestaña seleccionada
                string script = $"var triggerEl = document.querySelector('a[href=\"#{targetTabId}\"]'); if(triggerEl) {{ var tab = new bootstrap.Tab(triggerEl); tab.show(); }}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activeTabScript", script, true);
            }
        }

    }
}