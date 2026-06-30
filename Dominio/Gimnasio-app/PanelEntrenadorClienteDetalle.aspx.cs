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
    public partial class PanelEntrenadorClienteDetalle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

                    Session.Add("idClienteDetalle", idCliente);

                    List<Cliente> clientes = (List<Cliente>)Session["listaClientesEntrenador"];
                    if (clientes == null)
                    {
                        clientes = new ClienteNegocio().ListarClientes(true);
                        Session.Add("listaClientesEntrenador", clientes);
                    }
                    Cliente cliente = clientes.Find(c => c.IdUsuario == int.Parse(idCliente));

                    lblNombreCliente.Text = cliente.Nombre + " " + cliente.Apellido;
                    lblNombre.Text = cliente.Nombre;
                    lblApellido.Text = cliente.Apellido;
                    lblEdad.Text = CalcularEdad(cliente.FechaNacimiento).ToString();
                    lblPeso.Text = cliente.PesoCorporal.ToString();
                    lblFechaIngreso.Text = cliente.FechaIngreso.ToString("dd/MM/yyyy");

                    RutinasNegocio rutinasNegocio = new RutinasNegocio();
                    List<Rutina> rutinas = rutinasNegocio.GetRutinasUsuario(cliente);

                    gvRutinas.DataSource = rutinas;
                    gvRutinas.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cargar los datos del cliente (PanelEntrenadorClienteDetalle.Page_Load()):", ex);
            }
        }

        public int CalcularEdad(DateTime fechaNac)
        {
            int edad = DateTime.Today.Year - fechaNac.Year;
            if (fechaNac.Date > DateTime.Today.AddYears(-edad)) edad--;
            return edad;
        }

        protected void btnCrearRutina_Click(object sender, EventArgs e)
        {
            try
            {
                string idCliente = Session["idClienteDetalle"].ToString();
                Response.Redirect("PanelEntrenadorAsignarRutina.aspx?idCliente=" + idCliente, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al redirigir a crear rutina (PanelEntrenadorClienteDetalle.btnCrearRutina_Click()):", ex);
            }
        }

        protected void btnVerProgreso_Click(object sender, EventArgs e)
        {
            try
            {
                string idCliente = Session["idClienteDetalle"].ToString();
                Response.Redirect("PanelEntrenadorProgresoCliente.aspx?idCliente=" + idCliente, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al redirigir a ver progreso (PanelEntrenadorClienteDetalle.btnVerProgreso_Click()):", ex);
            }
        }
    }
}