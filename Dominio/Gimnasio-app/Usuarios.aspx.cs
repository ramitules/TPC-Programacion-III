using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

namespace Gimnasio_app
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    NegocioGimnasio negocio = new NegocioGimnasio();
                    List<Cliente> lista = negocio.listarTodosLosUsuarios("SP_ListarTodosLosUsuarios");
                    gdvUsuario.DataSource = lista;
                    gdvUsuario.DataBind();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}