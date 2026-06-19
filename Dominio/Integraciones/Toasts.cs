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
        public static void MostrarToast(Page page, string mensaje, string tipo = "success", string titulo = "")
        {
            if (page == null) return;

            string mensajeLimpio = mensaje.Replace("'", "\\'");
            string tituloLimpio = titulo.Replace("'", "\\'");
            string script = $"toastr.{tipo}('{mensajeLimpio}', '{tituloLimpio}');";

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
    }
}