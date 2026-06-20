using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;

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
                    Toasts.ToastAdvertencia(this, "Por favor complete todos los campos.", "Atencion");
                    return;
                }

                // Validacion de coincidencia de contraseñas
                if (txtPassword.Text != txtConfirmarPassword.Text)
                {
                    Toasts.ToastError(this, "Las contraseñas no coinciden.");
                    return;
                }

                // Validación de Email
                string email = txtEmail.Text.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                {
                    Toasts.ToastAdvertencia(this, "Por favor, ingresa un correo electronico valido.");
                    return;
                }

                // Validacion fecha de nacimiento (minimo 12 años)
                DateTime ahora = DateTime.Today;

                if (!DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fechaNacimiento))
                {
                    Toasts.ToastAdvertencia(this, "Por favor, ingresa una fecha de nacimiento válida.");
                    return;
                }

                DateTime FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                int edad = ahora.Year - fechaNacimiento.Year;

                if (fechaNacimiento.Date > ahora.AddYears(-edad))
                    edad--;

                if (edad < 12)
                {
                    Toasts.ToastAdvertencia(this, "Fecha de nacimiento inválida. Debes tener mínimo 12 años para entrenar en el gimnasio.");
                    return;
                }

                ClienteNegocio negocio = new ClienteNegocio();
                Cliente cliente = new Cliente
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Email = txtEmail.Text,
                    FechaNacimiento = FechaNacimiento,
                    FechaIngreso = ahora
                };

                negocio.Agregar(cliente, txtPassword.Text);

                // Oculta el formulario y muestra la leyenda de exito con el boton hacia Login.
                // Al desaparecer el boton, ademas, se evita un segundo alta accidental.
                pnlFormulario.Visible = false;
                pnlExito.Visible = true;
            }
            catch (Exception)
            {
                Toasts.ToastError(this, "Ocurrio un error al crear la cuenta. Intente nuevamente.");
            }
        }
    }
}
