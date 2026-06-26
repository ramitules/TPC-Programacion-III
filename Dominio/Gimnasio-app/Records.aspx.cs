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
            // TESTING
            Session.Add("cliente", new ClienteNegocio().Get("9", true));

            if (!(Seguridad.SessionActiva(Session["cliente"])))
            {
                Response.Redirect("Default", false);
                return;
            }

            if (!IsPostBack)
            {
                Cliente cliente = (Cliente)Session["cliente"];

                List<Dominio.Records> records = new RecordsNegocio().GetRecordsUsuario(cliente.IdUsuario.ToString());

                rptRecords.DataSource = records;
                rptRecords.DataBind();

                phSinRecords.Visible = records.Count == 0;
            }
        }
    }
}
