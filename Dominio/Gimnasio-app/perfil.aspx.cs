using Dominio;
using Negocio;
using Integraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class Perfil : System.Web.UI.Page
    {
        public bool Editando { get; set; }
        public bool TienePlanProximo { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            //  TESTING
            Cliente cliente = new ClienteNegocio().Get("9", true);
            Session.Add("cliente", cliente);
            //  TESTING

            if (!(Seguridad.SessionActiva(Session["cliente"])))
            {
                Response.Redirect("Default", false);
                return;
            }

            if (!IsPostBack)
            {
                SuscripcionNegocio suscripciones = new SuscripcionNegocio();
                Suscripcion suscripcionVPendiente = suscripciones.GetSuscripcionCliente(cliente.IdUsuario.ToString(), EstadoSuscripcion.VIGENTE_PENDIENTE);
                TienePlanProximo = suscripcionVPendiente.IdSuscripcion != 0;
                ddlPlan.DataSource = new PlanNegocio().GetPlanes();
                ddlPlan.DataValueField = "IdPlan";
                ddlPlan.DataTextField = "NombrePlan";
                ddlPlan.DataBind();

                // Cliente cliente = (Cliente)Session["cliente"];
                txtNombre.Text = cliente.Nombre;
                txtApellido.Text = cliente.Apellido;
                txtEmail.Text = cliente.Email;
                txtNacimiento.Text = cliente.FechaNacimiento.ToString("yyyy-MM-dd");
                txtPeso.Text = cliente.PesoCorporal.ToString();
                txtIngreso.Text = cliente.FechaIngreso.ToString("yyyy-MM-dd");
                ddlPlan.SelectedValue = cliente.SuscripcionCliente.Plan.IdPlan.ToString();
                txtVencimiento.Text = cliente.SuscripcionCliente.FechaFin.ToString("yyyy-MM-dd");

                int vencimiento = (cliente.SuscripcionCliente.FechaFin - DateTime.Now).Days;
                lblVencimiento.Text = "Vence en " + vencimiento.ToString() + " dias";
                // Habilitar boton para renovar suscripcion si vence en los proximos 5 dias
                if (vencimiento < 5)
                    btnRenovarPlan.Enabled = true;

                if (TienePlanProximo)
                {
                    btnRenovarPlan.Enabled = false;
                    btnCambiarPlan.Enabled = false;
                    txtProximoPlan.Text = suscripcionVPendiente.Plan.NombrePlan;
                }
            }
        }
        // ---------- DATOS ----------
        protected void btnEditarDatos_click(object sender, EventArgs e)
        {
            if (Session["cliente"] == null) return;
            Cliente cliente = (Cliente)Session["cliente"];

            try
            {
                if (btnEditarDatos.Text.ToLower() == "editar")
                {
                    Editando = true;
                    SetReadOnly(false);
                }
                else if (btnEditarDatos.Text.ToLower() == "guardar")
                {
                    Editando = false;
                    ClienteNegocio negocio = new ClienteNegocio();

                    cliente.Nombre = txtNombre.Text;
                    cliente.Apellido = txtApellido.Text;
                    cliente.Email = txtEmail.Text;
                    cliente.FechaNacimiento = DateTime.Parse(txtNacimiento.Text);
                    cliente.PesoCorporal = float.Parse(txtPeso.Text);

                    negocio.Modificar(cliente);

                    Toasts.MostrarToast(this, "Se han modificado sus datos personales con exito", "success", "Exito");

                    SetReadOnly();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error", false);
            }
        }
        
        protected void btnCancelarDatos_click(object sender, EventArgs e)
        {
            SetReadOnly();
        }

        protected void btnDarDeBaja_click(object sender, EventArgs e)
        {
            if (Session["cliente"] == null) return;
            Cliente cliente = (Cliente)Session["cliente"];
            try
            {
                new ClienteNegocio().DarBaja(cliente);
                Session.Clear();
                Toasts.MostrarToast(this, "Tu cuenta ha sido dada de baja.", "success", "Hasta pronto");
                Response.Redirect("Default", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error", false);
            }
        }
        
        protected void CambioTab(object sender, EventArgs e)
        {
            SetReadOnly();
        }
        protected void SetReadOnly(bool ReadOnly = true)
        {
            txtNombre.ReadOnly = ReadOnly;
            txtApellido.ReadOnly = ReadOnly;
            txtEmail.ReadOnly = ReadOnly;
            txtNacimiento.ReadOnly = ReadOnly;
            txtPeso.ReadOnly = ReadOnly;

            btnEditarDatos.Text = ReadOnly ? "Editar" : "Guardar";
        }
        // ---------- SUSCRIPCION ----------
        protected void btnCambiarPlan_click(object sender, EventArgs e)
        {
            if (btnCambiarPlan.Text.ToLower() == "cambiar plan")
            {
                ddlPlan.Enabled = true;
                btnCambiarPlan.Text = "Guardar";
            }
            else if (btnCambiarPlan.Text.ToLower() == "guardar")
            {
                CrearPlan();

                Toasts.MostrarToast(this, "Se ha agregado una suscripcion con exito. Entrara en vigencia al momento de vencerse la suscripcion actual.", "success", "Exito");

                ddlPlan.Enabled = false;
                btnCambiarPlan.Text = "Cambiar plan";
            }
        }
        protected void btnRenovarPlan_click(object sender, EventArgs e)
        {   // Asume que ya se puede renovar el plan (ver Page_Load())
            if (CrearPlan())
                Toasts.MostrarToast(this, "Se ha renovado su suscripcion con exito. Entrara en vigencia al momento de vencerse la suscripcion actual.", "success", "Exito");

            btnRenovarPlan.Enabled = false;
        }
        protected bool CrearPlan()
        {
            if (Session["cliente"] == null) 
                return false;

            Cliente cliente = (Cliente)Session["cliente"];
            SuscripcionNegocio negocio = new SuscripcionNegocio();

            // No se puede crear una suscripcion mas si ya existe una pendiente de activacion
            if (negocio.GetSuscripcionCliente(cliente.IdUsuario.ToString(), EstadoSuscripcion.VIGENTE_PENDIENTE).IdSuscripcion != 0)
            {
                Toasts.MostrarToast(this, "Ya tiene otro plan pendiente de activacion. Espere a que se venza o cancele su plan actual para suscribirse a uno nuevo", "error", "Exito");
                return false;
            }

            // Al cambiar de plan, se crea una nueva suscripcion
            Suscripcion suscripcionNueva = new Suscripcion();
            suscripcionNueva.Plan = new PlanNegocio().GetPlan(ddlPlan.SelectedValue);

            // Fecha de inicio igual al vencimiento de la anterior si aun no esta vencida
            if (cliente.SuscripcionCliente.FechaFin > DateTime.Now)
                suscripcionNueva.FechaInicio = cliente.SuscripcionCliente.FechaFin;
            // Fecha de hoy si no hay suscripcion vigente
            else
                suscripcionNueva.FechaInicio = DateTime.Now;

            suscripcionNueva.FechaFin = suscripcionNueva.FechaInicio.AddDays(suscripcionNueva.Plan.DuracionDiasPlan);
            suscripcionNueva.Estado = EstadoSuscripcion.VIGENTE_PENDIENTE;

            negocio.Agregar(suscripcionNueva, cliente);
            return true;
        }
        protected void btnCancelarSuscripcion_click(object sender, EventArgs e)
        {
            if (Session["cliente"] == null) return;
            Cliente cliente = (Cliente)Session["cliente"];
            SuscripcionNegocio negocio = new SuscripcionNegocio();
            Suscripcion suscripcionActual = negocio.GetSuscripcionCliente(cliente.IdUsuario.ToString(), EstadoSuscripcion.ACTIVA);
            
            if (suscripcionActual.IdSuscripcion != 0)
            {
                suscripcionActual.Estado = EstadoSuscripcion.CANCELADA;
                negocio.Modificar(suscripcionActual, cliente);
                Toasts.MostrarToast(this, "Su suscripcion ha sido dada de baja. Esperamos volver a verlo pronto.", "success", "Exito");
            }
        }
    }
}