using Dominio;
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
    public partial class FormularioAdmins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ADMIN)))
            {
                Response.Redirect("Login.aspx", false);
            }
            try
            {
                if (!IsPostBack)
                {
                    string paginaRetorno = "Admins.aspx";
                    if (Request.QueryString["idRol"] == null) return; 
                    int idRol = int.Parse(Request.QueryString["idRol"]);
                    if (Request.QueryString["id"] != null) 
                    {
                        int id = int.Parse(Request.QueryString["id"]);
                        List<Usuario> users;
                        if (idRol == (int)Roles.ADMIN)
                        {
                            List<Admin> lista = (List<Admin>)Session["listaAdmins"];
                            users = lista != null ? lista.Cast<Usuario>().ToList() : new List<Usuario>();
                            Usuario userBuscado = users.Find(a => a.IdUsuario == id);
                            if (userBuscado != null)
                            {
                                litTituloFormulario.Text = "Registrar nuevo Admin";
                                txtNombre.Text = userBuscado.Nombre;
                                txtApellido.Text = userBuscado.Apellido;
                                txtEmail.Text = userBuscado.Email;
                                txtFechaNacimiento.Text = userBuscado.FechaNacimiento.ToString("yyyy-MM-dd");
                                btnGuardar.Text = "Modificar Admin";
                                ddlEstadoAdmin.Enabled = true;

                                paginaRetorno = "Admins.aspx";
                            }
                        }
                        else if (idRol == (int)Roles.RECEPCIONISTA)
                        {
                            List<Recepcionista> lista = ((List<Recepcionista>)Session["listaRecepcionistas"]);
                            Recepcionista recepBuscado = lista.Find(a => a.IdUsuario == id);
                            //users = lista != null ? lista.Cast<Usuario>().ToList() : new List<Usuario>();                           
                            Usuario userBuscado = recepBuscado;
                            if (userBuscado != null)
                            {
                                litTituloFormulario.Text = "Registrar nuevo Recepcionista";
                                txtNombre.Text = userBuscado.Nombre;
                                txtApellido.Text = userBuscado.Apellido;
                                txtEmail.Text = userBuscado.Email;
                                txtFechaNacimiento.Text = userBuscado.FechaNacimiento.ToString("yyyy-MM-dd");
                                btnGuardar.Text = "Modificar Recepcionista";
                                ddlEstadoAdmin.Enabled = true;

                                paginaRetorno = "Recepcionistas.aspx";
                            }
                        }
                        else if (idRol == (int)Roles.ENTRENADOR)
                        {
                            List<Entrenador> lista = (List<Entrenador>)Session["listaEntrenadores"];
                            users = lista != null ? lista.Cast<Usuario>().ToList() : new List<Usuario>();
                            Usuario userBuscado = users.Find(a => a.IdUsuario == id);
                            if (userBuscado != null)
                            {
                                litTituloFormulario.Text = "Registrar nuevo Entrenador";
                                txtNombre.Text = userBuscado.Nombre;
                                txtApellido.Text = userBuscado.Apellido;
                                txtEmail.Text = userBuscado.Email;
                                txtFechaNacimiento.Text = userBuscado.FechaNacimiento.ToString("yyyy-MM-dd");
                                btnGuardar.Text = "Modificar Entrenador";
                                ddlEstadoAdmin.Enabled = true;

                                paginaRetorno = "Entrenadores.aspx";
                            }
                        }

                    }
                    else
                    {
                        if (idRol == (int)Roles.ADMIN)
                        {
                            litTituloFormulario.Text = "Registrar nuevo Admin";
                            btnGuardar.Text = "Crear Admin";
                            paginaRetorno = "Admins.aspx";
                        }
                        else if (idRol == (int)Roles.RECEPCIONISTA)
                        {
                            litTituloFormulario.Text = "Registrar nuevo Recepcionista";
                            btnGuardar.Text = "Crear Recepcionista";
                            paginaRetorno = "Recepcionistas.aspx";
                        }
                        else if (idRol == (int)Roles.ENTRENADOR)
                        {
                            litTituloFormulario.Text = "Registrar nuevo Entrenador";
                            btnGuardar.Text = "Crear Entrenador";
                            paginaRetorno = "Entrenadores.aspx";
                        }
                    }
                    btnVolver.HRef = paginaRetorno;
                    btnCancelar.HRef = paginaRetorno;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //AdminNegocio negocio = new AdminNegocio();
            //Usuario user;
            //EmailService emailService = new EmailService();
            //try
            //{
            //    var pass = string.IsNullOrEmpty(txtPassword.Text) ? "" : HashContrasenia.Hashear(txtPassword.Text);
            //    if (Request.QueryString["id"] != null)
            //    {
            //        int id = int.Parse(Request.QueryString["id"]);
            //        if (id == (int)Roles.ADMIN)
            //        {                        
            //            List<Usuario> admins = (List<Usuario>)Session["listaAdmins"];
            //            Usuario adminAModificar = admins.Find(a => a.IdUsuario == id);
            //            if (adminAModificar != null)
            //            {
            //                adminAModificar.Nombre = txtNombre.Text;
            //                adminAModificar.Apellido = txtApellido.Text;
            //                adminAModificar.Email = txtEmail.Text;
            //                adminAModificar.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
            //                adminAModificar.Activo = bool.Parse(ddlEstadoAdmin.SelectedValue);
            //                //string pass = string.IsNullOrEmpty(txtPassword.Text) ? "" : HashContrasenia.Hashear(txtPassword.Text);
            //                user = adminAModificar;
            //            }

            //        }
            //        negocio.modificarUsuario(user, pass);
            //    }
            //    else
            //    {
            //        if (!(string.IsNullOrEmpty(pass)))
            //        {
            //            admin.Nombre = txtNombre.Text;
            //            admin.Apellido = txtApellido.Text;
            //            admin.Email = txtEmail.Text;
            //            admin.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
            //            negocio.crearAdmin(admin, pass);
            //            emailService.armarCorreo(admin.Email, "Bienvenido al Gimnasio", $"Hola {admin.Nombre}, tu cuenta de administrador ha sido creada exitosamente. Tu contraseña es: {pass}");
            //            emailService.enviarCorreo();
            //        }
            //    }
            //    Session.Add("listaAdmins", negocio.ObtenerUsuarioPorRol("sp_Traer_Admins")); //esta linea actualiza la lista de Admins que esta guardada en Session
            //    string scriptNativo = @"
            //        alert('¡Administrador registrado con éxito!');
            //        window.location.href = 'Admins.aspx';"; 

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNativo", scriptNativo, true);

            AdminNegocio negocio = new AdminNegocio();
            Usuario user = null; // Inicializamos en null para evitar errores de compilación
            EmailService emailService = new EmailService();

            try
            {
                // 1. Recuperamos parámetros de la URL de forma segura
                string paginaRetorno = "Admins.aspx";
                int id = Request.QueryString["id"] != null ? int.Parse(Request.QueryString["id"]) : 0;
                int idRol = Request.QueryString["idRol"] != null ? int.Parse(Request.QueryString["idRol"]) : 0;

                // Hasheamos solo si el campo no está vacío (para modificaciones puede quedar vacío)
                string pass = string.IsNullOrEmpty(txtPassword.Text) ? "" : txtPassword.Text;

                if (id != 0) // ==================== MODO MODIFICAR ====================
                {
                    // Filtramos la lista de sesión correcta según el idRol que vino por URL
                    if (idRol == (int)Roles.ADMIN)
                    {
                        List<Admin> admins = (List<Admin>)Session["listaAdmins"];
                        if (admins != null)
                        {
                            user = admins.Find(x => x.IdUsuario == id);
                        }
                        paginaRetorno = "Admins.aspx";
                    }
                    else if (idRol == (int)Roles.RECEPCIONISTA)
                    {
                        List<Recepcionista> recepcionistas = (List<Recepcionista>)Session["listaRecepcionistas"];
                        if (recepcionistas != null)
                        {
                            user = recepcionistas.Find(x => x.IdUsuario == id);
                        }
                        paginaRetorno = "Recepcionistas.aspx";
                    }
                    else if (idRol == (int)Roles.ENTRENADOR)
                    {
                        List<Entrenador> entrenadores = (List<Entrenador>)Session["listaEntrenadores"];
                        if (entrenadores != null)
                        {
                            user = entrenadores.Find(x => x.IdUsuario == id);
                        }
                        paginaRetorno = "Entrenadores.aspx";
                    }

                    // Si encontramos al usuario, mapeamos los cambios de las cajas de texto
                    if (user != null)
                    {
                        user.Nombre = txtNombre.Text.Trim();
                        user.Apellido = txtApellido.Text.Trim();
                        user.Email = txtEmail.Text.Trim();
                        user.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                        user.Activo = bool.Parse(ddlEstadoAdmin.SelectedValue);

                        // Mandamos a actualizar (tu método se encarga de discriminar internamente o usar el SP correspondiente)
                        negocio.modificarUsuario(user, pass);
                    }
                }
                else // ==================== MODO CREAR (NUEVO) ====================
                {
                    // Para el alta, la contraseña SI O SI es obligatoria
                    if (!string.IsNullOrEmpty(txtPassword.Text))
                    {
                        Usuario nuevoUsuario = null;
                        if (idRol == (int)Roles.ADMIN)
                        {
                            nuevoUsuario = new Admin();
                            paginaRetorno = "Admins.aspx";
                        }
                        else if (idRol == (int)Roles.RECEPCIONISTA)
                        {
                            nuevoUsuario = new Recepcionista(); // o el nombre exacto de tu clase derivada
                            paginaRetorno = "Recepcionistas.aspx";
                        }
                        else if (idRol == (int)Roles.ENTRENADOR)
                        {
                            nuevoUsuario = new Entrenador();
                            paginaRetorno = "Entrenadores.aspx";
                        }
                        // Instanciamos un usuario nuevo genérico

                        nuevoUsuario.Nombre = txtNombre.Text.Trim();
                        nuevoUsuario.Apellido = txtApellido.Text.Trim();
                        nuevoUsuario.Email = txtEmail.Text.Trim();
                        nuevoUsuario.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                        nuevoUsuario.Activo = true;
                        nuevoUsuario.FechaIngreso = DateTime.Now;
                        nuevoUsuario.Rol = new RolUsuario { IdRol = idRol };

                        negocio.crearUsuario(nuevoUsuario, pass); // Este método debería manejar la creación según el rol


                        // 📧 Envío de mail de bienvenida (Le mandamos el texto PLANO de la contraseña, no el hash)
                        emailService.armarCorreo(nuevoUsuario.Email, "Bienvenido al Gimnasio", $"Hola {nuevoUsuario.Nombre}, tu cuenta ha sido creada exitosamente. Tu contraseña de acceso es: {txtPassword.Text}");
                        emailService.enviarCorreo();
                    }
                    else
                    {
                        // Validación rápida por si intentan guardar un alta sin contraseña
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorPass", "alert('La contraseña es obligatoria para nuevos registros.');", true);
                        return;
                    }
                }

                // 3. Actualizamos TODAS las sesiones de staff para que queden sincronizadas
                Session["listaAdmins"] = negocio.ObtenerUsuarioPorRol("sp_Traer_Admins").Cast<Admin>().ToList();
                Session["listaRecepcionistas"] = negocio.ObtenerUsuarioPorRol("sp_Traer_Recepcionistas").Cast<Recepcionista>().ToList();
                Session["listaEntrenadores"] = negocio.ObtenerUsuarioPorRol("sp_Traer_Entrenadores").Cast<Entrenador>().ToList();

                // 4. Alerta de éxito y redirección a la grilla principal del Personal
                string mensajeExito = id > 0 ? "¡Datos actualizados con éxito!" : "¡Personal registrado con éxito!";
                string scriptNativo = $@"
            alert('{mensajeExito}');
            window.location.href = '{paginaRetorno}';";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertNativo", scriptNativo, true);
            }
            catch (Exception ex)
            {
                string scriptError = $"alert('Ocurrió un error: {ex.Message.Replace("'", "\\'")}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertError", scriptError, true);
            }
        }
    }
}