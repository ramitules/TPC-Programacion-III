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
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim().ToLower();
                string email = txtEmail.Text.Trim().ToLower();
                string fechaNacimiento = txtFechaNacimiento.Text;
                string contrasenia = txtPassword.Text;

                ClienteNegocio negocio = new ClienteNegocio();

                // Validacion de campos obligatorios
                if (!(Validaciones.validarNombre(nombre) && Validaciones.validarNombre(apellido)))
                {
                    Toasts.ToastAdvertencia(this, "Por favor complete todos los campos.", "Atencion");
                    return;
                }

                // Validacion de coincidencia de contraseñas
                if (!(Validaciones.validarContrasenias(contrasenia, txtConfirmarPassword.Text)))
                {
                    Toasts.ToastError(this, "Las contraseñas no coinciden.");
                    return;
                }

                // Validación de Email
                if (!Validaciones.validarEmail(email))
                {
                    Toasts.ToastAdvertencia(this, "Por favor, ingresa un correo electronico valido.");
                    return;
                }

                if (negocio.ExisteEmail(email))  // Duplicados
                {
                    Toasts.ToastError(this, "Ya existe una cuenta con esta direccion de correo electronico.");
                    return;
                }
                

                // Validacion fecha de nacimiento (minimo 12 años)
                if (!(Validaciones.validarFechaNacimiento(fechaNacimiento, 12)))
                {
                    Toasts.ToastAdvertencia(this, "Fecha de nacimiento inválida. Debes tener mínimo 12 años para entrenar en el gimnasio.");
                    return;
                }

                Cliente cliente = new Cliente
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Email = txtEmail.Text,
                    FechaNacimiento = DateTime.Parse(fechaNacimiento),
                    FechaIngreso = DateTime.Now
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
