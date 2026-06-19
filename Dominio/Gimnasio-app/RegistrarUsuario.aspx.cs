using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class RegistrarUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validacion de campos obligatorios
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtFechaNacimiento.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    Toasts.MostrarToast(this, "Por favor complete todos los campos.", "warning", "Atencion");
                    return;
                }

                // Validacion de coincidencia de contraseñas
                if (txtPassword.Text != txtConfirmarPassword.Text)
                {
                    Toasts.MostrarToast(this, "Las contraseñas no coinciden.", "error", "Error");
                    return;
                }

                ClienteNegocio negocio = new ClienteNegocio();
                Cliente cliente = new Cliente();

                cliente.Nombre = txtNombre.Text;
                cliente.Apellido = txtApellido.Text;
                cliente.Email = txtEmail.Text;
                cliente.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                cliente.FechaIngreso = DateTime.Now;

                negocio.Agregar(cliente, txtPassword.Text);

                // Oculta el formulario y muestra la leyenda de exito con el boton hacia Login.
                // Al desaparecer el boton, ademas, se evita un segundo alta accidental.
                pnlFormulario.Visible = false;
                pnlExito.Visible = true;
            }
            catch (Exception)
            {
                Toasts.MostrarToast(this, "Ocurrio un error al crear la cuenta. Intente nuevamente.", "error", "Error");
            }
        }
    }
}
