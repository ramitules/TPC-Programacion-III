using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class PanelEntrenadorProgresoCliente : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SessionActiva(Session["usuario"]) || !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ENTRENADOR))
            {
                Response.Redirect("Default", false);
                return;
            }

            try
            {
                if (!IsPostBack)
                {
                    string idCliente = Request.QueryString["idCliente"];
                    if (string.IsNullOrEmpty(idCliente))
                    {
                        Response.Redirect("PanelEntrenador", false);
                        return;
                    }

                    Session.Add("idClienteProgreso", idCliente);

                    List<Cliente> clientes = (List<Cliente>)Session["listaClientesEntrenador"];
                    if (clientes == null)
                    {
                        clientes = new ClienteNegocio().ListarClientes(true);
                        Session.Add("listaClientesEntrenador", clientes);
                    }
                    Cliente cliente = clientes.Find(c => c.IdUsuario == int.Parse(idCliente));

                    lblNombreCliente.Text = cliente.Nombre + " " + cliente.Apellido;

                    SesionEntrenamientoNegocio sesionNegocio = new SesionEntrenamientoNegocio();
                    List<SesionEntrenamiento> sesiones = sesionNegocio.GetSesionesDeCliente(int.Parse(idCliente));

                    gvSesiones.DataSource = sesiones;
                    gvSesiones.DataBind();

                    lblSinSesiones.Visible = sesiones.Count == 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cargar el progreso del cliente (PanelEntrenadorProgresoCliente.Page_Load()):", ex);
            }
        }

        protected void gvSesiones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            SesionEntrenamiento sesion = (SesionEntrenamiento)e.Row.DataItem;

            Label lblDuracion = (Label)e.Row.FindControl("lblDuracion");
            lblDuracion.Text = CalcularDuracion(sesion.FechaHoraInicio, sesion.FechaHoraFin);

            HyperLink lnkVerDetalle = (HyperLink)e.Row.FindControl("lnkVerDetalle");
            string idClienteActual = Session["idClienteProgreso"].ToString();
            lnkVerDetalle.NavigateUrl = $"PanelEntrenadorProgresoSesionDetalle.aspx?idSesion={sesion.IdSesion}&idCliente={idClienteActual}";
        }

        private string CalcularDuracion(DateTime inicio, DateTime fin)
        {
            TimeSpan duracion = fin - inicio;
            if (duracion.TotalMinutes < 1)
                return "-";

            int horas = (int)duracion.TotalHours;
            int minutos = duracion.Minutes;

            return horas > 0 ? $"{horas}h {minutos}m" : $"{minutos}m";
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                string idCliente = Session["idClienteProgreso"].ToString();
                Response.Redirect("PanelEntrenadorClienteDetalle.aspx?idCliente=" + idCliente, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al redirigir al detalle del cliente (PanelEntrenadorProgresoCliente.btnVolver_Click()):", ex);
            }
        }
    }
}