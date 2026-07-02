using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        // Para recuperacion de contrasenia
        private const int CodigoResetVigenciaMinutos = 15;
        private const int CodigoResetMaxIntentos = 5;
        private const int CodigoResetDigitos = 6;
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

        // Genera y envia por mail un codigo de recuperacion de contrasenia.
        // Si el email no corresponde a un usuario activo, no hace nada.
        public static void SolicitarRecuperacion(string email)
        {
            AccesoADatos datos = new AccesoADatos();
            bool existeUsuario;
            try
            {
                datos.SetearConsultaSP("sp_ExisteEmail");
                datos.setearParametro("@Email", email);
                datos.ejecutarLectura();
                existeUsuario = datos.Lector.Read();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al solicitar recuperación (Seguridad.SolicitarRecuperacion()):", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            if (!existeUsuario)
                return;

            string codigo = GenerarCodigoNumerico(CodigoResetDigitos);
            string hashCodigo = HashContrasenia.Hashear(codigo);

            datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_GuardarCodigoReset");
                datos.setearParametro("@Email", email);
                datos.setearParametro("@CodigoHash", hashCodigo);
                datos.setearParametro("@Expira", DateTime.Now.AddMinutes(CodigoResetVigenciaMinutos));
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al guardar el codigo de recuperacion (Seguridad.SolicitarRecuperacion()):", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            try
            {
                var mail = new EmailService();
                mail.armarCorreo(email, "Recuperacion de contraseña",
                    $"Tu codigo de recuperacion es <b>{codigo}</b>. Vence en {CodigoResetVigenciaMinutos} minutos.");
                mail.enviarCorreo();
            }
            catch (Exception)
            {
                // No se propaga: una falla de SMTP no debe filtrar si el email existe.
            }
        }

        // Valida el codigo de recuperacion y, si es correcto, aplica la nueva contrasenia.
        // Devuelve false ante codigo incorrecto, vencido, agotado (por intentos) o email inexistente,
        // sin distinguir el motivo para no dar pistas a quien este probando codigos.
        public static bool RestablecerContrasenia(string email, string codigo, string passNueva)
        {
            int idUsuario;
            string hashGuardado;
            DateTime? expira;
            int intentos;

            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_ObtenerCodigoReset");
                datos.setearParametro("@Email", email);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                    return false;

                idUsuario = Convert.ToInt32(datos.Lector["IdUsuarios"]);
                hashGuardado = datos.Lector["CodigoReset"].ToString();
                expira = datos.Lector["CodigoResetExpira"] as DateTime?;
                intentos = Convert.ToInt32(datos.Lector["CodigoResetIntentos"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al restablecer la contrasenia (Seguridad.RestablecerContrasenia()):", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            if (hashGuardado == null || expira == null || DateTime.Now > expira.Value || intentos >= CodigoResetMaxIntentos)
                return false;

            if (!HashContrasenia.Verificar(codigo, hashGuardado))
            {
                datos = new AccesoADatos();
                try
                {
                    datos.SetearConsultaSP("sp_IncrementarIntentosCodigoReset");
                    datos.setearParametro("@IdUsuarios", idUsuario);
                    datos.ejecutarAccion();
                }
                finally
                {
                    datos.cerrarConexion();
                }
                return false;
            }

            datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_ActualizarPasswordConCodigo");
                datos.setearParametro("@IdUsuarios", idUsuario);
                datos.setearParametro("@PassHash", HashContrasenia.Hashear(passNueva));
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }

            return true;
        }

        private static string GenerarCodigoNumerico(int digitos)
        {
            byte[] bytes = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            uint valor = BitConverter.ToUInt32(bytes, 0);
            int max = (int)Math.Pow(10, digitos);
            return (valor % max).ToString(new string('0', digitos));
        }
    }
}
