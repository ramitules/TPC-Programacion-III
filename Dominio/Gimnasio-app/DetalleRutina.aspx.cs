using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class DetalleRutina : Page
    {
        // Conteo de series ya registradas por ejercicio (IdEjercicio -> cantidad). Usado por el markup en modo activo.
        private Dictionary<int, int> _seriesPorEjercicio = new Dictionary<int, int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Seguridad.SessionActiva(Session["cliente"])))
            {
                Response.Redirect("Default", false);
                return;
            }

            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect("Rutinas", false);
                return;
            }

            if (!IsPostBack)
            {
                if (HaySesionActiva(id))
                    CargarModoActivo(id);
                else
                    CargarResumen(id);
            }
        }

        /// <summary>
        /// Indica si hay una sesion de entrenamiento en curso para la rutina indicada.
        /// </summary>
        private bool HaySesionActiva(string idRutina)
        {
            SesionEntrenamiento activa = (SesionEntrenamiento)Session["sesionActiva"];
            return activa != null && activa.Rutina != null && activa.Rutina.IdRutina.ToString() == idRutina;
        }

        /// <summary>
        /// Modo resumen: muestra la rutina como tabla de solo lectura (aun no iniciada).
        /// </summary>
        private void CargarResumen(string id)
        {
            RutinasNegocio negocio = new RutinasNegocio();
            Rutina rutina = negocio.Get(id);

            if (rutina is null)
            {
                Response.Redirect("Rutinas", false);
                return;
            }

            rutina.Ejercicios = negocio.GetEjerciciosDeRutina(id);

            litNombre.Text = rutina.Nombre;
            litDia.Text = string.IsNullOrEmpty(rutina.Dia) ? "Dia libre" : rutina.Dia;
            litFecha.Text = rutina.FechaCreacion.ToString("dd/MM/yyyy");

            phResumen.Visible = true;
            phSesionActiva.Visible = false;

            rptEjercicios.DataSource = rutina.Ejercicios;
            rptEjercicios.DataBind();

            phSinEjercicios.Visible = rutina.Ejercicios.Count == 0;

            btnIniciar.Enabled = true;
        }

        /// <summary>
        /// Modo activo: sesion en curso. Muestra los ejercicios con inputs para registrar cada serie.
        /// </summary>
        private void CargarModoActivo(string id)
        {
            SesionEntrenamiento sesion = (SesionEntrenamiento)Session["sesionActiva"];

            RutinasNegocio negocio = new RutinasNegocio();
            Rutina rutina = negocio.Get(id);
            rutina.Ejercicios = negocio.GetEjerciciosDeRutina(id);

            litNombre.Text = rutina.Nombre;
            litDia.Text = string.IsNullOrEmpty(rutina.Dia) ? "Dia libre" : rutina.Dia;
            litFecha.Text = rutina.FechaCreacion.ToString("dd/MM/yyyy");

            // Precalcular cuantas series se registraron por ejercicio en esta sesion
            _seriesPorEjercicio.Clear();
            foreach (SerieCompletada serie in new SerieCompletadaNegocio().GetSeriesDeSesion(sesion.IdSesion))
            {
                int idEj = serie.Ejercicio.IdEjercicio;
                _seriesPorEjercicio[idEj] = _seriesPorEjercicio.ContainsKey(idEj) ? _seriesPorEjercicio[idEj] + 1 : 1;
            }

            phResumen.Visible = false;
            phSesionActiva.Visible = true;

            rptEjecucion.DataSource = rutina.Ejercicios;
            rptEjecucion.DataBind();

            btnIniciar.Enabled = false;
        }

        /// <summary>
        /// Devuelve cuantas series ya se registraron para un ejercicio en la sesion actual. Usado desde el markup.
        /// </summary>
        protected int ContarSeries(int idEjercicio)
        {
            return _seriesPorEjercicio.ContainsKey(idEjercicio) ? _seriesPorEjercicio[idEjercicio] : 0;
        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect("Rutinas", false);
                return;
            }

            Cliente cliente = (Cliente)Session["cliente"];
            Rutina rutina = new RutinasNegocio().Get(id);
            if (rutina is null)
            {
                Response.Redirect("Rutinas", false);
                return;
            }

            SesionEntrenamiento sesion = new SesionEntrenamientoNegocio().IniciarSesionEntrenamiento(cliente, rutina);
            Session["sesionActiva"] = sesion;

            CargarModoActivo(id);
        }

        protected void rptEjecucion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "RegistrarSerie")
                return;

            string id = Request.QueryString["id"];
            SesionEntrenamiento sesion = (SesionEntrenamiento)Session["sesionActiva"];
            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect("Rutinas", false);
                return;
            }

            HiddenField hfObjetivoSeries = (HiddenField)e.Item.FindControl("hfObjetivoSeries");
            int objetivoSeries = int.Parse(hfObjetivoSeries.Value);
            int idEjercicio = int.Parse(e.CommandArgument.ToString());
            int seriesActuales = new SerieCompletadaNegocio()
                .GetSeriesDeSesion(sesion.IdSesion)
                .FindAll(s => s.Ejercicio.IdEjercicio == idEjercicio).Count;

            if (seriesActuales >= objetivoSeries + 1)
            {
                Toasts.MostrarToast(this, $"Ya alcanzaste el límite de {objetivoSeries + 1} series para este ejercicio.", "warning", "Límite alcanzado");
                CargarModoActivo(id);
                return;
            }

            TextBox txtPeso = (TextBox)e.Item.FindControl("txtPeso");
            TextBox txtReps = (TextBox)e.Item.FindControl("txtReps");
            TextBox txtRir = (TextBox)e.Item.FindControl("txtRir");

            SerieCompletada serie = new SerieCompletada();
            serie.Sesion = sesion;
            serie.Ejercicio = new Ejercicio { IdEjercicio = idEjercicio };
            serie.PesoLevantadoKG = ParsearEntero(txtPeso.Text);
            serie.RepeticionesLogradas = (int)ParsearEntero(txtReps.Text);
            serie.RIR = (int)ParsearEntero(txtRir.Text);
            serie.EsRecordPersonal = false;

            new SerieCompletadaNegocio().Agregar(serie);

            Toasts.MostrarToast(this, "Serie registrada con exito. Segui asi!", "info", "Nueva serie");

            CargarModoActivo(id);
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            SesionEntrenamiento sesion = (SesionEntrenamiento)Session["sesionActiva"];
            if (sesion != null)
                new SesionEntrenamientoNegocio().FinSesionEntrenamiento(sesion);

            Session.Remove("sesionActiva");

            Toasts.MostrarToast(this, "Sesion de entrenamiento finalizada", "success", "Finalizada");
            
            if (!string.IsNullOrEmpty(id))
                CargarResumen(id);
        }

        private float ParsearEntero(string valor)
        {
            return float.TryParse(valor, out float resultado) ? resultado : 0;
        }
    }
}
