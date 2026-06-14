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
                    if (Request.QueryString["id"] != null) 
                    {
                        int id = int.Parse(Request.QueryString["id"]);
                        List<Usuario> admins = (List<Usuario>)Session["listaAdmins"];
                        Usuario adminBuscado = admins.Find(a => a.IdUsuario == id);
                        if (adminBuscado != null)
                        {
                            txtNombre.Text = adminBuscado.Nombre;
                            txtApellido.Text = adminBuscado.Apellido;
                            txtEmail.Text = adminBuscado.Email;
                            txtFechaNacimiento.Text = adminBuscado.FechaNacimiento.ToString("yyyy-MM-dd");
                            btnGuardar.Text = "Modificar Usuario";
                            ddlEstadoAdmin.Enabled = true;
                        }
                    }
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
                var pass = txtPassword.Text;
                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"]);
                    List<Usuario> admins = (List<Usuario>)Session["listaAdmins"];
                    Usuario adminAModificar = admins.Find(a => a.IdUsuario == id);
                    if (adminAModificar != null)
                    {
                        adminAModificar.Nombre = txtNombre.Text;
                        adminAModificar.Apellido = txtApellido.Text;
                        adminAModificar.Email = txtEmail.Text;
                        adminAModificar.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                        adminAModificar.Activo = bool.Parse(ddlEstadoAdmin.SelectedValue);
                        pass = string.IsNullOrEmpty(txtPassword.Text) ? "" : txtPassword.Text;
                        negocio.modificarAdmin((Admin)adminAModificar, pass);
                    }
                }
                else
                {
                    admin.Nombre = txtNombre.Text;
                    admin.Apellido = txtApellido.Text;
                    admin.Email = txtEmail.Text;
                    admin.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                    negocio.crearAdmin(admin, pass);
                }
                Session.Add("listaAdmins", negocio.ObtenerUsuarioPorRol("sp_Traer_Admins")); //esta linea actualiza la lista de Admins que esta guardada en Session
                string scriptNativo = @"
                    alert('¡Administrador registrado con éxito!');
                    window.location.href = 'Admins.aspx';"; 

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNativo", scriptNativo, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}