using System;
using System.Web.UI;


namespace Integraciones
{
    /// <summary>
    /// Clase para mostrar toasts al momento de generarse un problema, modificar, o guardar algo de forma exitosa.
    /// Sirve como reemplazo a Console.WriteLine() y alert
    /// </summary>
    public static class Toasts
    {
        private static void MostrarToast(Page page, string mensaje, string tipo = "success", string titulo = "", string urlRedireccion = "")
        {
            if (page == null) return;

            string mensajeLimpio = mensaje.Replace("'", "\\'");
            string tituloLimpio = titulo.Replace("'", "\\'");

            string script = $"toastr.{tipo}('{mensajeLimpio}', '{tituloLimpio}');";

            if (!string.IsNullOrEmpty(urlRedireccion))
            {
                script = $@"toastr.options.onHidden = function() {{ window.location.href = '{urlRedireccion}'; }};" + script;
            }

            ScriptManager sm = ScriptManager.GetCurrent(page);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                // Dentro de un UpdatePanel
                ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
            else
            {
                // Postback normal
                page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        public static void ToastAdvertencia(Page page, string mensaje, string titulo = "Advertencia", string urlRedireccion = "")
        {
            MostrarToast(page, mensaje, "warning", titulo, urlRedireccion);
        }
        public static void ToastInformacion(Page page, string mensaje, string titulo = "Informacion", string urlRedireccion = "")
        {
            MostrarToast(page, mensaje, "info", titulo, urlRedireccion);
        }
        public static void ToastError(Page page, string mensaje, string titulo = "Error", string urlRedireccion = "")
        {
            MostrarToast(page, mensaje, "error", titulo, urlRedireccion);
        }
        public static void ToastExito(Page page, string mensaje, string titulo = "Exito", string urlRedireccion = "")
        {
            MostrarToast(page, mensaje, "success", titulo, urlRedireccion);
        }
    }
}