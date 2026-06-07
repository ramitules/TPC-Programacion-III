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
        protected void Page_Load(object sender, EventArgs e)
        {
            //  TESTING
            Session.Add("cliente", new ClienteNegocio().Get("9"));
            //  TESTING

            if (!(Seguridad.SessionActiva(Session["cliente"]))) return;

            if (!IsPostBack)
            {
                Cliente cliente = (Cliente)Session["cliente"];
                if (cliente.SuscripcionCliente is null)
                    cliente.SuscripcionCliente = new SuscripcionNegocio().GetSuscripcionCliente(cliente.IdUsuario.ToString());

                txtNombre.Text = cliente.Nombre;
                txtApellido.Text = cliente.Apellido;
                txtEmail.Text = cliente.Email;
                txtNacimiento.Text = cliente.FechaNacimiento.ToString("yyyy-MM-dd");
                txtPeso.Text = cliente.PesoCorporal.ToString();
                txtIngreso.Text = cliente.FechaIngreso.ToString("yyyy-MM-dd");
                txtPlan.Text = cliente.SuscripcionCliente.Plan.NombrePlan;
                txtVencimiento.Text = cliente.SuscripcionCliente.FechaFin.ToString("yyyy-MM-dd");

                int vencimiento = (cliente.SuscripcionCliente.FechaFin - DateTime.Now).Days;
                lblVencimiento.Text = "Vence en " + vencimiento.ToString() + " dias";
                // Habilitar boton para renovar suscripcion si vence en los proximos 5 dias
                if (vencimiento < 5)
                {
                    btnRenovarPlan.Enabled = true;
                }
            }
        }
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

                    SetReadOnly();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error", false);
            }
        }
        protected void btnCambiarPlan_click(object sender, EventArgs e)
        {
            if (btnCambiarPlan.Text.ToLower() == "cambiar plan")
            {
                // PENDIENTE
                btnCambiarPlan.Text = "Guardar";
            }
            else if (btnCambiarPlan.Text.ToLower() == "guardar")
            {
                // PENDIENTE
                btnCambiarPlan.Text = "Cambiar plan";
            }
        }
        protected void btnCancelarDatos_click(object sender, EventArgs e)
        {
            SetReadOnly();
        }
        protected void btnRenovarPlan_click(object sender, EventArgs e)
        {
            
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
    }
}