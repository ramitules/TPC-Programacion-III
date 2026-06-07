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
    public partial class FormularioAdmins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["id"] == null) { }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            AdminNegocio negocio = new AdminNegocio();
            Admin admin = new Admin();
            try
            {
                admin.Nombre = txtNombre.Text;
                admin.Apellido = txtApellido.Text;
                admin.Email = txtEmail.Text;
                admin.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                var pass = txtPassword.Text;
                negocio.crearAdmin(admin, pass);
                Session.Add("listaAdmins", negocio.ObtenerUsuarioPorRol("sp_Traer_Admins")); //esta linea actualiza la lista de Admins que esta guardada en Session
                string scriptNativo = @"
                    alert('¡Administrador registrado con éxito!');
                    window.location.href = 'Admins.aspx';"; /* Tu página del listado */

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNativo", scriptNativo, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}