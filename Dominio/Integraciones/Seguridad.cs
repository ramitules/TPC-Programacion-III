using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccesoDB;

namespace Integraciones
{
    public abstract class Seguridad
    {
        public static bool SessionActiva(object user)
        {
            if (user is null) return false;

            try
            {
                if (((Usuario)user).IdUsuario != 0) 
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //Este se encarga no solo de verificar si el usuario esta logueado o no, sino que tambien, implementado en el Page_load de cada pagina, permite evaluar si el usuario logueado tiene acceso o no. 
        //recibe como argumento el usuario de Session, y un ENUM de tipo Roles, que se corresponde con el rol que se requiere para acceder a la pagina.
        public static bool accesoYPermisos(Usuario user, Roles nombreRolPermiso)
        {
            if (user == null)
            {
                return false;
            }

            if (user.Rol.IdRol != (int)nombreRolPermiso)
            {
                return false;
            }
            return true;
        }
        public static Usuario logueo(string email, string pass) 
        {
            Usuario usuario;
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_logueo");
                datos.setearParametro("@Email", email);
                //datos.setearParametro("@Pass", pass);
                datos.ejecutarLectura();
                //string contraseia = HashContrasenia.Hashear(pass);
                //Object algo = datos.Lector.Read();
                if (datos.Lector.Read())
                {
                    // La validacion de la contrasenia se hace aca (PBKDF2), no en SQL.
                    if (!HashContrasenia.Verificar(pass, datos.Lector["Pass"] as string))
                        return null; // Contrasenia incorrecta

                    int rol = Convert.ToInt32(datos.Lector["IdRol"]);
                    if (rol == (int)Roles.CLIENTE)
                    {
                        usuario = new Cliente()
                        {
                            PesoCorporal = Convert.ToSingle(datos.Lector["PesoCorporalKG"])
                        };
                    }
                    else if (rol == (int)Roles.ADMIN)
                    {
                        usuario = new Admin(); //Elegi Admin, pero para el casteo podria ser cualquier tipo de rol distinto a cliente, es solo para poder almacenar los datos en session
                    }
                    else
                    {
                        usuario = new Entrenador();
                    }
                    usuario.IdUsuario = (int)datos.Lector["IdUsuarios"];
                    usuario.Nombre = (string)datos.Lector["Nombre"];
                    usuario.Apellido = (string)datos.Lector["Apellido"];
                    usuario.Email = (string)datos.Lector["Email"];
                    usuario.Rol.IdRol = rol;
                    usuario.Rol.RolDescripcion = (string)datos.Lector["Rol Nombre"];
                    usuario.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    usuario.FechaIngreso = (DateTime)datos.Lector["FechaIngreso"];
                    usuario.Activo = (bool)datos.Lector["Activo"];

                    return usuario;
                }
                else
                {
                    return null; // No se encontró un usuario con esas credenciales
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al loguear usuario (Seguridad.logueo()):", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
