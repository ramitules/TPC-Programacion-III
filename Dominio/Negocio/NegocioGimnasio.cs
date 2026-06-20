using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AccesoDB;
using Dominio;

namespace Negocio
{
    public class NegocioGimnasio
    {
        public NegocioGimnasio() 
        { 
            

        }
        //public Usuario login(string email, string password)
        //{
        //    Usuario user = new Usuario();
        //    AccesoADatos datos = new AccesoADatos();
        //    try
        //    {
        //        datos.SetearConsulta("SELECT * FROM Usuarios WHERE Email = @Email AND Contraseña = @Password");
        //        datos.setearParametro("@Email", email);
        //        datos.setearParametro("@Password", password);
        //        datos.ejecutarLectura();
        //        if (datos.Lector.Read())
        //        {
        //            user.IdUsuario = (int)datos.Lector["IdUsuarios"];
        //            user.Nombre = (string)datos.Lector["Nombre"];
        //            user.Apellido = (string)datos.Lector["Apellido"];
        //            user.Email = (string)datos.Lector["Email"];
        //            user.IdRol = (int)datos.Lector["IdRol"];
        //            return user;
        //        }
        //        else
        //        {
        //            return null; // No se encontró un usuario con esas credenciales
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Ocurrió un error al intentar iniciar sesión: " + ex.Message);
        //    }
        //    finally
        //    {
        //        datos.cerrarConexion();
        //    }
        }
}
