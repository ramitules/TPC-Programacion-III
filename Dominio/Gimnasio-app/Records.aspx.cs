using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Gimnasio_app
{
    public partial class Records : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SessionActiva(Session["usuario"]) ||
                !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.CLIENTE))
            {
                Response.Redirect("Default", false);
                return;
            }

            if (!IsPostBack)
            {
                Cliente cliente = (Cliente)Session["usuario"];

                List<Dominio.Records> records = new RecordsNegocio().GetRecordsUsuario(cliente.IdUsuario.ToString());

                rptRecords.DataSource = records;
                rptRecords.DataBind();

                phSinRecords.Visible = records.Count == 0;
            }
        }
    }
}
