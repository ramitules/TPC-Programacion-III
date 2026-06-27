using Integraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;

namespace Gimnasio_app
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //if (tbxEmail.Text == "user" && tbxPass.Text == "1234")
                //{
                //    Session["rol"] = 1;
                //    Response.Redirect("~/Default.aspx", false);
                //}
                var email = tbxEmail.Text;
                var pass = tbxPass.Text;
                Usuario usuario = Seguridad.logueo(email, pass);
                if (usuario != null)
                {
                    Session.Add("usuario", usuario);
                    switch (usuario.Rol.IdRol)
                    {
                        case (int)Roles.ADMIN:
                            Response.Redirect("~/AdminControl.aspx", false);
                            break;
                        case (int)Roles.CLIENTE:
                            Response.Redirect("~/perfil.aspx", false);
                            break;
                        default:
                            break;
                    }
                    
                    //Response.Redirect("~/Default.aspx", false);
                }
                else
                {
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}