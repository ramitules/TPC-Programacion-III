using Integraciones;
using System;

namespace Gimnasio_app
{
    public partial class RecuperarContrasenia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Seguridad.SessionActiva(Session["usuario"]))
            {
                Response.Redirect("Login", false);
            }
        }

        protected void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            string email = tbxEmail.Text.Trim();

            try
            {
                Seguridad.SolicitarRecuperacion(email);
            }
            catch (Exception)
            {
                // No se expone el detalle: no debe filtrar si el email existe o si fallo el envio.
            }

            tbxEmail.Enabled = false;
            pnlRestablecer.Visible = true;
            lblError.Visible = false;
            lblInfo.Text = "Si el email está registrado, te enviamos un código de verificación.";
            lblInfo.Visible = true;
        }

        protected void btnRestablecer_Click(object sender, EventArgs e)
        {
            lblInfo.Visible = false;

            if (tbxPassNueva.Text != tbxPassNuevaConfirmar.Text)
            {
                lblError.Text = "Las contraseñas no coinciden.";
                lblError.Visible = true;
                return;
            }

            bool ok = Seguridad.RestablecerContrasenia(tbxEmail.Text.Trim(), tbxCodigo.Text.Trim(), tbxPassNueva.Text);
            if (ok)
            {
                pnlRestablecer.Visible = false;
                lblError.Visible = false;
                lblInfo.Text = "Contraseña actualizada. Ya podés iniciar sesión.";
                lblInfo.Visible = true;
            }
            else
            {
                lblError.Text = "Codigo inválido o vencido.";
                lblError.Visible = true;
            }
        }
    }
}
