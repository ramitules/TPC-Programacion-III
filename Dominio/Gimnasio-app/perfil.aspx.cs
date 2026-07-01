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
        public bool Editando
        {
            get { return ViewState["Editando"] != null && (bool)ViewState["Editando"]; }
            set { ViewState["Editando"] = value; }
        }
        public bool CambiandoPlan
        {
            get { return ViewState["CambiandoPlan"] != null && (bool)ViewState["CambiandoPlan"]; }
            set { ViewState["CambiandoPlan"] = value; }
        }
        public bool TienePlanProximo { get; set; }
        public bool TienePlanVigente { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SessionActiva(Session["usuario"]) ||
                !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.CLIENTE))
            {
                Response.Redirect("Default", false);
                return;
            }

            if (!IsPostBack)
            {
                Cliente cliente = (Cliente)Session["usuario"];
                SuscripcionNegocio suscripciones = new SuscripcionNegocio();
                Suscripcion suscripcionVPendiente = suscripciones.GetSuscripcionCliente(cliente.IdUsuario.ToString(), EstadoSuscripcion.VIGENTE_PENDIENTE);

                TienePlanProximo = suscripcionVPendiente.IdSuscripcion != 0;
                TienePlanVigente = cliente.SuscripcionCliente != null && cliente.SuscripcionCliente.IdSuscripcion != 0 && cliente.SuscripcionCliente.Plan != null;

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
                if (TienePlanVigente)
                {
                    ddlPlan.SelectedValue = cliente.SuscripcionCliente.Plan.IdPlan.ToString();
                    txtVencimiento.Text = cliente.SuscripcionCliente.FechaFin.ToString("yyyy-MM-dd");
                    int vencimiento = (cliente.SuscripcionCliente.FechaFin - DateTime.Now).Days;
                    lblVencimiento.Text = "Vence en " + vencimiento.ToString() + " dias";

                    // Habilitar boton para renovar suscripcion si vence en los proximos 5 dias
                    if (vencimiento < 5)
                        btnRenovarPlan.Enabled = true;
                }
                else
                {
                    // Sin suscripcion vigente: se permite contratar la primera pero no renovar
                    lblVencimiento.Text = "No tenes una suscripción activa.";
                    btnCambiarPlan.Enabled = true;
                    btnRenovarPlan.Enabled = false;
                }


                if (TienePlanProximo)
                {
                    btnRenovarPlan.Enabled = false;
                    btnCambiarPlan.Enabled = false;
                    txtProximoPlan.Text = suscripcionVPendiente.Plan.NombrePlan;
                    txtVencimientoProximo.Text = suscripcionVPendiente.FechaFin.ToString("yyyy-MM-dd");
                }
            }
        }
        // ---------- DATOS ----------
        protected void btnEditarDatos_click(object sender, EventArgs e)
        {
            if (Session["usuario"] == null) return;
            Cliente cliente = (Cliente)Session["usuario"];

            try
            {
                if (!Editando)
                {
                    Editando = true;
                    SetReadOnly(false);
                }
                else
                {
                    string nombre = txtNombre.Text.Trim();
                    string apellido = txtApellido.Text.Trim();
                    string email = txtEmail.Text.Trim().ToLower();
                    string fechaNacimiento = txtNacimiento.Text;

                    // Validacion de campos obligatorios
                    if (!(Validaciones.validarNombre(nombre) && Validaciones.validarNombre(apellido)))
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

                    if (email != cliente.Email && new ClienteNegocio().ExisteEmail(email, cliente.IdUsuario))
                    {
                        Toasts.ToastAdvertencia(this, "Ese correo electronico ya esta en uso.");
                        return;
                    }

                    // Validacion fecha de nacimiento (minimo 12 años)
                    if (!(Validaciones.validarFechaNacimiento(fechaNacimiento, 12)))
                    {
                        Toasts.ToastAdvertencia(this, "Fecha de nacimiento inválida. Debes tener mínimo 12 años para entrenar en el gimnasio.");
                        return;
                    }

                    // Validacion peso KG
                    float peso = Validaciones.validarPeso(txtPeso.Text);

                    // Se arma un objeto aparte (no se muta el "cliente" de Session) para no
                    // reflejar cambios en la sesion hasta confirmar que se guardaron en la BD.
                    Cliente actualizado = new Cliente
                    {
                        IdUsuario = cliente.IdUsuario,
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = email,
                        FechaNacimiento = DateTime.Parse(fechaNacimiento),
                        PesoCorporal = peso,
                        Activo = cliente.Activo,
                        Rol = cliente.Rol,
                        FechaIngreso = cliente.FechaIngreso
                    };

                    new ClienteNegocio().Modificar(actualizado);
                    // Refrescar
                    Session["usuario"] = new ClienteNegocio().Get(cliente.IdUsuario.ToString(), true);
                    txtPeso.Text = peso.ToString();  // Por si esta fuera de rango

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
            if (Session["usuario"] == null) return;
            Cliente cliente = (Cliente)Session["usuario"];

            // Descartar cambios no guardados y restablecer los valores originales
            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtEmail.Text = cliente.Email;
            txtNacimiento.Text = cliente.FechaNacimiento.ToString("yyyy-MM-dd");
            txtPeso.Text = cliente.PesoCorporal.ToString();

            Editando = false;
            SetReadOnly();
        }

        protected void btnDarDeBaja_click(object sender, EventArgs e)
        {
            if (Session["usuario"] == null) return;
            Cliente cliente = (Cliente)Session["usuario"];
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
            if (!CambiandoPlan)
            {
                CambiandoPlan = true;
                ddlPlan.Enabled = true;
                btnCambiarPlan.Text = "Guardar";
            }
            else
            {
                // No se confirma el cambio hasta que se apruebe el pago en el modal
                AbrirModalPago("cambio");
            }
        }
        protected void btnRenovarPlan_click(object sender, EventArgs e)
        {   // Asume que ya se puede renovar el plan (ver Page_Load())
            // No se confirma la renovacion hasta que se apruebe el pago en el modal
            AbrirModalPago("renovar");
        }
        /// <summary>
        /// Prepara el resumen del pago y abre el modal de tarjeta. El alta real de la
        /// suscripcion ocurre recien al confirmar el pago (ver btnConfirmarPago_click).
        /// </summary>
        protected void AbrirModalPago(string accion)
        {
            Plan plan = new PlanNegocio().GetPlan(ddlPlan.SelectedValue);
            lblPlanPago.Text = plan.NombrePlan;
            lblMontoPago.Text = plan.PrecioPlan.ToString("C");
            hfAccionPago.Value = accion;

            ScriptManager.RegisterStartupScript(this, GetType(), "abrirPago",
                "new bootstrap.Modal(document.getElementById('modalPago')).show();", true);
        }
        protected void btnConfirmarPago_click(object sender, EventArgs e)
        {
            // Validacion de formato en el servidor. Los datos de tarjeta no se persisten.
            if (!ValidarTarjeta())
            {
                Toasts.ToastAdvertencia(this, "Revise los datos de la tarjeta. Hay campos invalidos.");
                ScriptManager.RegisterStartupScript(this, GetType(), "abrirPago",
                    "new bootstrap.Modal(document.getElementById('modalPago')).show();", true);
                return;
            }

            // Pago aprobado automaticamente al ser validos los datos.
            if (CrearPlan())
            {
                Toasts.ToastExito(this, "Pago aprobado. Su suscripcion entrara en vigencia al momento de vencerse la suscripcion actual.");

                if (hfAccionPago.Value == "cambio")
                {
                    CambiandoPlan = false;
                    ddlPlan.Enabled = false;
                    btnCambiarPlan.Text = "Cambiar plan";
                }
                else if (hfAccionPago.Value == "renovar")
                {
                    btnRenovarPlan.Enabled = false;
                }

                LimpiarCamposTarjeta();

                ScriptManager.RegisterStartupScript(this, GetType(), "cerrarPago",
                    "var m = bootstrap.Modal.getInstance(document.getElementById('modalPago')); if (m) m.hide();", true);
            }
        }
        /// <summary>
        /// Valida el formato de los campos de la tarjeta y marca en rojo los invalidos.
        /// </summary>
        protected bool ValidarTarjeta()
        {
            // Resetear estado visual
            txtNombreTarjeta.CssClass = "form-control";
            txtNumeroTarjeta.CssClass = "form-control";
            txtVencimientoTarjeta.CssClass = "form-control";
            txtCvvTarjeta.CssClass = "form-control";

            bool valido = true;

            if (!Validaciones.validarNombre(txtNombreTarjeta.Text))
            {
                txtNombreTarjeta.CssClass += " is-invalid";
                valido = false;
            }
            if (!Validaciones.validarNumeroTarjeta(txtNumeroTarjeta.Text))
            {
                txtNumeroTarjeta.CssClass += " is-invalid";
                valido = false;
            }
            if (!Validaciones.validarVencimientoTarjeta(txtVencimientoTarjeta.Text))
            {
                txtVencimientoTarjeta.CssClass += " is-invalid";
                valido = false;
            }
            if (!Validaciones.validarCvv(txtCvvTarjeta.Text))
            {
                txtCvvTarjeta.CssClass += " is-invalid";
                valido = false;
            }

            return valido;
        }
        protected void LimpiarCamposTarjeta()
        {
            txtNombreTarjeta.Text = "";
            txtNumeroTarjeta.Text = "";
            txtVencimientoTarjeta.Text = "";
            txtCvvTarjeta.Text = "";
            txtNombreTarjeta.CssClass = "form-control";
            txtNumeroTarjeta.CssClass = "form-control";
            txtVencimientoTarjeta.CssClass = "form-control";
            txtCvvTarjeta.CssClass = "form-control";
        }
        protected bool CrearPlan()
        {
            if (Session["usuario"] == null) 
                return false;

            Cliente cliente = (Cliente)Session["usuario"];
            SuscripcionNegocio negocio = new SuscripcionNegocio();

            // No se puede crear una suscripcion mas si ya existe una pendiente de activacion
            if (negocio.GetSuscripcionCliente(cliente.IdUsuario.ToString(), EstadoSuscripcion.VIGENTE_PENDIENTE).IdSuscripcion != 0)
            {
                Toasts.ToastError(this, "Ya tiene otro plan pendiente de activacion. Espere a que se venza o cancele su plan actual para suscribirse a uno nuevo");
                return false;
            }

            // Al contratar/cambiar de plan, se crea una nueva suscripcion
            Suscripcion suscripcionNueva = new Suscripcion();
            suscripcionNueva.Plan = new PlanNegocio().GetPlan(ddlPlan.SelectedValue);

            if (suscripcionNueva.Plan.DuracionDiasPlan <= 0)
            {
                Toasts.ToastError(this, "Este plan no esta disponible actualmente. Por favor contacte al gimnasio.");
                return false;
            }

            // Hay una suscripcion vigente (todavia no vencida) si existe y su vencimiento es futuro.
            bool tieneVigente = cliente.SuscripcionCliente != null && cliente.SuscripcionCliente.FechaFin > DateTime.Now;

            if (tieneVigente)
            {
                // Queda pendiente: entra en vigencia recien al vencer la actual.
                suscripcionNueva.FechaInicio = cliente.SuscripcionCliente.FechaFin;
                suscripcionNueva.Estado = EstadoSuscripcion.VIGENTE_PENDIENTE;
            }
            else
            {
                // Primera suscripcion (o la anterior ya vencio): entra en vigencia inmediata.
                suscripcionNueva.FechaInicio = DateTime.Now;
                suscripcionNueva.Estado = EstadoSuscripcion.ACTIVA;
            }

            suscripcionNueva.FechaFin = suscripcionNueva.FechaInicio.AddDays(suscripcionNueva.Plan.DuracionDiasPlan);

            negocio.Agregar(suscripcionNueva, cliente);

            if (tieneVigente)
            {
                TienePlanProximo = true;
                txtProximoPlan.Text = suscripcionNueva.Plan.NombrePlan;
                txtVencimientoProximo.Text = suscripcionNueva.FechaFin.ToString("yyyy-MM-dd");
            }
            else
            {
                // Reflejar la nueva suscripcion activa en el panel, recien tras confirmar
                // que la persistencia fue exitosa (se recarga el cliente desde la BD).
                Session["usuario"] = new ClienteNegocio().Get(cliente.IdUsuario.ToString(), true);
                txtVencimiento.Text = suscripcionNueva.FechaFin.ToString("yyyy-MM-dd");
                int vencimiento = (suscripcionNueva.FechaFin - DateTime.Now).Days;
                lblVencimiento.Text = "Vence en " + vencimiento.ToString() + " dias";
            }

            return true;
        }
        protected void btnCancelarSuscripcion_click(object sender, EventArgs e)
        {
            if (Session["usuario"] == null) return;
            Cliente cliente = (Cliente)Session["usuario"];

            if(new SuscripcionNegocio().BajaSuscripcionCliente(cliente))
            {
                // Refrescar (si habia una suscripcion pendiente, ya quedo activa en la BD)
                cliente = new ClienteNegocio().Get(cliente.IdUsuario.ToString(), true);
                Session["usuario"] = cliente;

                bool tieneNuevaVigente = cliente.SuscripcionCliente != null && cliente.SuscripcionCliente.IdSuscripcion != 0 && cliente.SuscripcionCliente.Plan != null;

                if (tieneNuevaVigente)
                {
                    // La suscripcion que estaba pendiente entro en vigencia
                    ddlPlan.SelectedValue = cliente.SuscripcionCliente.Plan.IdPlan.ToString();
                    txtVencimiento.Text = cliente.SuscripcionCliente.FechaFin.ToString("yyyy-MM-dd");
                    int vencimiento = (cliente.SuscripcionCliente.FechaFin - DateTime.Now).Days;
                    lblVencimiento.Text = "Vence en " + vencimiento.ToString() + " dias";
                    btnRenovarPlan.Enabled = vencimiento < 5;
                    btnCambiarPlan.Enabled = false;

                    Toasts.ToastInformacion(this, "Su suscripcion ha sido cancelada. La suscripcion pendiente ha entrado en vigencia.", "Suscripcion actualizada");
                }
                else
                {
                    txtVencimiento.Text = "";
                    lblVencimiento.Text = "No tenes una suscripcion activa.";
                    btnCambiarPlan.Enabled = true;
                    btnRenovarPlan.Enabled = false;

                    Toasts.ToastInformacion(this, "Su suscripcion ha sido dada de baja. Esperamos volver a verlo pronto.", "Dado de baja");
                }
            }
            else
            {
                Toasts.ToastError(this, "Ha ocurrido un error. Por favor intente nuevamente.");
            }
        }
    }
}