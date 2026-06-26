using Integraciones;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class SiteMaster : MasterPage
    {
        public string NombreUsuario { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SessionActiva(Session["usuario"]))
                NombreUsuario = "Sin sesion";
            else
                NombreUsuario = ((Usuario)Session["usuario"]).Nombre;
        }
    }
}