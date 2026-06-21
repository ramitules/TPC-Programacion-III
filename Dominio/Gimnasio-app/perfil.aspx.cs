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
                    string nombre = txtNombre.Text.Trim();
                    string apellido = txtApellido.Text.Trim().ToLower();
                    string email = txtEmail.Text.Trim().ToLower();
                    string fechaNacimiento = txtNacimiento.Text;

                    // Validacion de campos obligatorios
                    if (!(Validaciones.validarNombre(nombre) || Validaciones.validarNombre(apellido)))
                    {
                        Toasts.ToastAdvertencia(this, "Por favor complete todos los campos.", "Atencion");
                        return;
                    }

                    // Validación de Email
                    if (!Validaciones.validarEmail(email))
                    {
                        Toasts.ToastAdvertencia(this, "Por favor, ingresa un correo electronico valido.");
                        return;
                    }

                    // Validacion fecha de nacimiento (minimo 12 años)
                    if (!(Validaciones.validarFechaNacimiento(fechaNacimiento, 12)))
                    {
                        Toasts.ToastAdvertencia(this, "Fecha de nacimiento inválida. Debes tener mínimo 12 años para entrenar en el gimnasio.");
                        return;
                    }

                    cliente.Nombre = nombre;
                    cliente.Apellido = apellido;
                    cliente.Email = email;
                    cliente.FechaNacimiento = DateTime.Parse(fechaNacimiento);
                    cliente.PesoCorporal = float.Parse(txtPeso.Text);

                    new ClienteNegocio().Modificar(cliente);

                    Toasts.ToastExito(this, "Se han modificado sus datos personales con exito");

                    Editando = false;
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
                Toasts.ToastInformacion(this, "Tu cuenta ha sido dada de baja.", "Hasta pronto", "Default");
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

                Toasts.ToastExito(this, "Se ha agregado una suscripcion con exito. Entrara en vigencia al momento de vencerse la suscripcion actual.");

                ddlPlan.Enabled = false;
                btnCambiarPlan.Text = "Cambiar plan";
            }
        }
        protected void btnRenovarPlan_click(object sender, EventArgs e)
        {   // Asume que ya se puede renovar el plan (ver Page_Load())
            if (CrearPlan())
                Toasts.ToastExito(this, "Se ha renovado su suscripcion con exito. Entrara en vigencia al momento de vencerse la suscripcion actual.");

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
                Toasts.ToastError(this, "Ya tiene otro plan pendiente de activacion. Espere a que se venza o cancele su plan actual para suscribirse a uno nuevo");
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
                Toasts.ToastInformacion(this, "Su suscripcion ha sido dada de baja. Esperamos volver a verlo pronto.", "Dado de baja");
            }
        }
    }
}