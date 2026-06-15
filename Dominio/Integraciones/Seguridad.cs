using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    }
}
