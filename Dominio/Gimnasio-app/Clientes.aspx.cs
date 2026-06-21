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
    }
}