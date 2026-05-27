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
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rol"] == null)
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else 
            {
                if (Session["rol"].ToString() == "1")
                {
                    NegocioGimnasio negocio = new NegocioGimnasio();
                    List<Usuario> lista = negocio.listarTodosLosUsuarios("sp_ObtenerUsuarios"); ;
                    List<Cliente> listaClientes = lista.Where(x => x.IdRol == 3).Cast<Cliente>().ToList();
                    List<Profesor> listaProfes = lista.Where(x => x.IdRol == 2).Cast<Profesor>().ToList();
                    List<Administrativo> listaAdministrativos = lista.Where(x => x.IdRol == 4).Cast<Administrativo>().ToList();
                    List<Admin> listaAdmin = lista.Where(x => x.IdRol == 1).Cast<Admin>().ToList();

                    dgvCliente.DataSource = listaClientes;
                    dgvCliente.DataBind();

                    dgvProfesor.DataSource = listaProfes;
                    dgvProfesor.DataBind();

                    dgvAdministrador.DataSource = listaAdministrativos;
                    dgvAdministrador.DataBind();

                    dgvAdmin.DataSource = listaAdmin;
                    dgvAdmin.DataBind();
                } 
            }
        }
    }
}