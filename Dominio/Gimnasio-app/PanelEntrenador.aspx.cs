using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Integraciones;

namespace Gimnasio_app
{
    public partial class PanelEntrenador : Page
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
                    List<Cliente> clientes;
                    if (Session["listaClientesEntrenador"] == null)
                    {
                        clientes = new ClienteNegocio().ListarClientes(true);
                        Session.Add("listaClientesEntrenador", clientes);
                    }
                    else
                    {
                        clientes = (List<Cliente>)Session["listaClientesEntrenador"];
                    }

                    dgvClientes.DataSource = clientes;
                    dgvClientes.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cargar la lista de clientes (PanelEntrenador.Page_Load()): ", ex);
            }
        }

        public int CalcularEdad(object fechaNac)
        {
            DateTime fecha = Convert.ToDateTime(fechaNac);
            int edad = DateTime.Today.Year - fecha.Year;
            if (fecha.Date > DateTime.Today.AddYears(-edad)) edad--;
            return edad;
        }

        private void aplicarFiltros()
        {
            try
            {
                List<Cliente> clientes = (List<Cliente>)Session["listaClientesEntrenador"];
                List<Cliente> filtrado = clientes;

                if (txtBuscar.Text != "")
                {
                    filtrado = filtrado.FindAll(a =>
                        (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
                }

                dgvClientes.DataSource = filtrado;
                dgvClientes.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al aplicar filtros de clientes (PanelEntrenador.aplicarFiltros()): ", ex);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            aplicarFiltros(); 
        }
    }
}