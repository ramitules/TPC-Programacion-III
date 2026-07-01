using Dominio;
using Negocio;
using System.Web.SessionState;

namespace Gimnasio_app
{
    /// <summary>
    /// Resuelve la sesion de entrenamiento en curso de un cliente, usada por Rutinas.aspx y
    /// DetalleRutina.aspx para detectar sesiones activas y evitar que se inicie una segunda
    /// sesion en paralelo.
    /// </summary>
    public static class SesionActivaHelper
    {
        public static SesionEntrenamiento Obtener(HttpSessionState session, Cliente cliente)
        {
            SesionEntrenamiento activa = (SesionEntrenamiento)session["sesionActiva"];
            if (activa != null)
                return activa;

            activa = new SesionEntrenamientoNegocio().GetSesionActiva(cliente.IdUsuario);
            if (activa != null)
                session["sesionActiva"] = activa;

            return activa;
        }
    }
}
