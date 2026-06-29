using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Gimnasio_app
{
    public partial class PanelEntrenador : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClienteNegocio clienteNegocio = new ClienteNegocio();
                List<Cliente> clientes = clienteNegocio.ListarClientes(true);
                Session["listaClientesEntrenador"] = clientes; //Guardo todo en Session 

                dgvClientes.DataSource = clientes;
                dgvClientes.DataBind();
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
            List<Cliente> clientes = (List<Cliente>)Session["listaClientesEntrenador"];
            List<Cliente> filtrado = clientes;

            if (txtBuscar.Text != "")
            {
                filtrado = filtrado.FindAll(a =>
                    (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower())); //emparejo el texto para la busqueda
            }

            dgvClientes.DataSource = filtrado;
            dgvClientes.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            aplicarFiltros(); 
        }
    }
}