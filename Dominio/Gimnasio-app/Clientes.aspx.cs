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
    public partial class Clientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminNegocio adminNegocio = new AdminNegocio();
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                if (!IsPostBack)
                {
                    if (Session["listaClientes"] == null)
                    {
                        clientes = adminNegocio.ObtenerUsuarioPorRol("sp_Traer_Clientes").Cast<Cliente>().ToList();
                        Session.Add("listaClientes", clientes);
                    }
                    else
                    {
                        clientes = (List<Cliente>)Session["listaClientes"];
                    }
                    dgvClientes.DataSource = clientes != null ? clientes : new List<Cliente>();
                    dgvClientes.DataBind();
                    ddlEstadoSuscripcion.DataSource = adminNegocio.listaSuscripciones();
                    ddlEstadoSuscripcion.DataTextField = "Estado";
                    ddlEstadoSuscripcion.DataValueField = "IdSuscripcion";
                    ddlEstadoSuscripcion.DataBind();
                    ddlEstadoSuscripcion.Items.Insert(0, new ListItem("Todos", "0"));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Funcion solo de estetica
        public string ObtenerBadgeEstado(object estadoObj)
        {
            if (estadoObj == null)
                return "<span class='badge bg-secondary'>Sin Info</span>";

            // Convertimos el objeto que viene del Eval al tipo de tu Enum
            EstadoSuscripcion estado = (EstadoSuscripcion)estadoObj;

            string claseCss = "";
            string texto = "";

            switch (estado)
            {
                case EstadoSuscripcion.ACTIVA: // Valor 1
                    claseCss = "bg-success";   // Verde
                    texto = "Activa";
                    break;

                case EstadoSuscripcion.VENCIDA: // Valor 2
                    claseCss = "bg-danger";    // Rojo
                    texto = "Vencida";
                    break;

                case EstadoSuscripcion.CANCELADA: // Valor 3
                    claseCss = "bg-secondary";  // Gris
                    texto = "Cancelada";
                    break;

                case EstadoSuscripcion.VIGENTE_PENDIENTE: // Valor 4 (O como figure exacto en tu enum)
                    claseCss = "bg-warning text-dark"; // Amarillo con letras oscuras
                    texto = "Pendiente";
                    break;

                default:
                    claseCss = "bg-dark";
                    texto = "Desconocido";
                    break;
            }

            // Retornamos la etiqueta HTML armada dinámicamente
            return $"<span class='badge {claseCss}'>{texto}</span>";
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            aplicarFiltros();
            //List<Cliente> filtrado;
            //try
            //{
            //    if (Session["listaClientes"] != null)
            //    {
            //        List<Cliente> clientes = (List<Cliente>)Session["listaClientes"];
            //        filtrado = clientes.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
            //    }
            //    else
            //    {
            //        filtrado = new List<Cliente>();
            //    }
            //    dgvClientes.DataSource = filtrado != null ? filtrado : new List<Cliente>();
            //    dgvClientes.DataBind();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            aplicarFiltros();
            //List<Cliente> filtrado;
            //bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            //try
            //{
            //    if (Session["listaClientes"] != null)
            //    {
            //        List<Cliente> clientes = (List<Cliente>)Session["listaClientes"];
            //        if (ddlEstado.SelectedValue == "todos")
            //        {
            //            filtrado = clientes;
            //        }
            //        else
            //        {
            //            filtrado = clientes.FindAll(a => a.Activo == estadoSeleccionado);
            //        }
            //        dgvClientes.DataSource = filtrado != null ? filtrado : new List<Cliente>();
            //        dgvClientes.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        protected void ddlEstadoSuscripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            aplicarFiltros();
            //List<Cliente> filtrado;
            //int estadoSeleccionado = int.Parse(ddlEstadoSuscripcion.SelectedValue);
            //try
            //{
            //    if (Session["listaClientes"] != null)
            //    {
            //        List<Cliente> clientes = (List<Cliente>)Session["listaClientes"];
            //        if (ddlEstadoSuscripcion.SelectedValue == "0")
            //        {
            //            filtrado = clientes;
            //        }
            //        else
            //        {
            //            filtrado = clientes.FindAll(a => a.SuscripcionCliente.IdSuscripcion == estadoSeleccionado);
            //        }
            //        dgvClientes.DataSource = filtrado != null ? filtrado : new List<Cliente>();
            //        dgvClientes.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        //Metodo para unificar filtros
        public void aplicarFiltros()
        {
            List<Cliente> filtrado;
            bool estadoSeleccionado = ddlEstado.SelectedValue == "activos" ? true : false;
            int estadoSuscripcionSeleccionado = int.Parse(ddlEstadoSuscripcion.SelectedValue);
            try
            {
                if (Session["listaClientes"] != null)
                {
                    List<Cliente> clientes = (List<Cliente>)Session["listaClientes"];
                    filtrado = clientes;
                    if (ddlEstado.SelectedValue != "todos")
                    {
                        filtrado = filtrado.FindAll(a => a.Activo == estadoSeleccionado);
                    }
                    if (ddlEstadoSuscripcion.SelectedValue != "0")
                    {
                        filtrado = filtrado.FindAll(a => a.SuscripcionCliente.IdSuscripcion == estadoSuscripcionSeleccionado);
                    }
                    if (txtBuscar.Text != "")
                    {
                        filtrado = filtrado.FindAll(a => (a.Nombre + " " + a.Apellido).ToLower().Contains(txtBuscar.Text.ToLower()));
                    }
                    dgvClientes.DataSource = filtrado != null ? filtrado : new List<Cliente>();
                    dgvClientes.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}