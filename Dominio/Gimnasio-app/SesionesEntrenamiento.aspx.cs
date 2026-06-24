using Dominio;
using Integraciones;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gimnasio_app
{
    public partial class SesionesEntrenamiento : System.Web.UI.Page
    {
        // Valores especiales del filtro de rutina.
        private const string FiltroTodas = "TODAS";
        private const string FiltroLibre = "LIBRE";

        // Ultimo valor valido de cada campo de fecha, para poder revertir ante un rango invalido.
        private const string KeyDesdeValido = "DesdeValido";
        private const string KeyHastaValido = "HastaValido";

        /// <summary>
        /// Id de la sesion actualmente desplegada (-1 si ninguna). Usado tambien desde el markup.
        /// </summary>
        protected int IdSesionExpandida
        {
            get { return ViewState["IdSesionExpandida"] == null ? -1 : (int)ViewState["IdSesionExpandida"]; }
            set { ViewState["IdSesionExpandida"] = value; }
        }

        /// <summary>
        /// Ordenar columna. Por defecto "Fecha"
        /// </summary>
        private string SortColumn
        {
            get { return ViewState["SortColumn"] == null ? "Fecha" : (string)ViewState["SortColumn"]; }
            set { ViewState["SortColumn"] = value; }
        }

        private bool SortAsc
        {
            get { return ViewState["SortAsc"] != null && (bool)ViewState["SortAsc"]; }
            set { ViewState["SortAsc"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //  TESTING
            Session.Add("cliente", new ClienteNegocio().Get("9", true));
            //  TESTING

            if (!(Seguridad.SessionActiva(Session["cliente"])))
            {
                Response.Redirect("Default", false);
                return;
            }

            if (!IsPostBack)
            {
                Cliente cliente = (Cliente)Session["cliente"];
                // Cargar lista de sesiones de entrenamiento en Session
                List<SesionEntrenamiento> sesiones = new SesionEntrenamientoNegocio().GetSesionesDeCliente(cliente.IdUsuario);
                Session["sesionesCliente"] = sesiones;

                CargarFiltroRutinas(sesiones);

                // Pre-filtrar por rutina si llega ?id= (ideal desde el boton "Sesiones realizadas" de DetalleRutina).
                string idRutina = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(idRutina) && ddlRutina.Items.FindByValue(idRutina) != null)
                    ddlRutina.SelectedValue = idRutina;

                // Orden inicial: por fecha, mas recientes primero.
                SortColumn = "Fecha";
                SortAsc = false;

                BindGrid();
            }
        }

        /// <summary>
        /// Llena el dropdown de rutinas con las rutinas distintas presentes en las sesiones del cliente.
        /// </summary>
        private void CargarFiltroRutinas(List<SesionEntrenamiento> sesiones)
        {
            ddlRutina.Items.Clear();
            ddlRutina.Items.Add(new ListItem("Todas las rutinas", FiltroTodas));

            bool haySesionLibre = sesiones.Any(s => s.Rutina == null);

            List<Rutina> rutinas = sesiones
                .Where(sesion => sesion.Rutina != null)
                .Select(sesion => sesion.Rutina)
                .GroupBy(rutina => rutina.IdRutina)
                .Select(grupo => grupo.First())
                .OrderBy(rutina => rutina.Nombre)
                .ToList();

            foreach (Rutina rutina in rutinas)
                ddlRutina.Items.Add(new ListItem(rutina.Nombre, rutina.IdRutina.ToString()));

            if (haySesionLibre)
                ddlRutina.Items.Add(new ListItem("Sesion libre", FiltroLibre));
        }

        /// <summary>
        /// Aplica filtros y orden en memoria sobre la lista cacheada y rebindea la grilla.
        /// </summary>
        private void BindGrid()
        {
            List<SesionEntrenamiento> sesiones = (List<SesionEntrenamiento>)Session["sesionesCliente"] ?? new List<SesionEntrenamiento>();
            IEnumerable<SesionEntrenamiento> filtradas = sesiones;

            // Filtro por rutina
            string filtroRutina = ddlRutina.SelectedValue;
            if (filtroRutina == FiltroLibre)
                filtradas = filtradas.Where(s => s.Rutina == null);
            else if (filtroRutina != FiltroTodas && int.TryParse(filtroRutina, out int idRutina))
                filtradas = filtradas.Where(s => s.Rutina != null && s.Rutina.IdRutina == idRutina);

            // Filtro por rango de fechas (inclusivo)
            if (DateTime.TryParse(txtDesde.Text, out DateTime desde))
                filtradas = filtradas.Where(s => s.FechaHoraInicio.Date >= desde.Date);
            if (DateTime.TryParse(txtHasta.Text, out DateTime hasta))
                filtradas = filtradas.Where(s => s.FechaHoraInicio.Date <= hasta.Date);

            // Orden
            if (SortColumn == "Rutina")
                filtradas = SortAsc
                    ? filtradas.OrderBy(s => s.Rutina == null ? "" : s.Rutina.Nombre)
                    : filtradas.OrderByDescending(s => s.Rutina == null ? "" : s.Rutina.Nombre);
            else  // Por fecha
                filtradas = SortAsc
                    ? filtradas.OrderBy(s => s.FechaHoraInicio)
                    : filtradas.OrderByDescending(s => s.FechaHoraInicio);

            List<SesionEntrenamiento> resultado = filtradas.ToList();

            ActualizarCarets();

            rptSesiones.DataSource = resultado;
            rptSesiones.DataBind();

            phSinSesiones.Visible = resultado.Count == 0;
        }

        private void ActualizarCarets()
        {
            string caret = SortAsc ? "<i class='bi bi-caret-up-fill'></i>" : "<i class='bi bi-caret-down-fill'></i>";
            litCaretRutina.Text = SortColumn == "Rutina" ? caret : "";
            litCaretFecha.Text = SortColumn == "Fecha" ? caret : "";
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            IdSesionExpandida = -1;
            BindGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlRutina.SelectedValue = FiltroTodas;
            txtDesde.Text = "";
            txtHasta.Text = "";
            ViewState[KeyDesdeValido] = "";
            ViewState[KeyHastaValido] = "";
            IdSesionExpandida = -1;
            BindGrid();
        }

        /// <summary>
        /// Valida que "Desde" no sea mayor que "Hasta". Si es invalido, revierte el campo al ultimo valor
        /// valido y avisa con un toast. Un campo vacio no se considera invalido.
        /// </summary>
        protected void txtDesde_TextChanged(object sender, EventArgs e)
        {
            if (EsRangoValido())
                ViewState[KeyDesdeValido] = txtDesde.Text;
            else
            {
                txtDesde.Text = (string)ViewState[KeyDesdeValido] ?? "";
                Toasts.ToastAdvertencia(this, "La fecha 'Desde' no puede ser mayor que 'Hasta'.", "Rango invalido");
            }
        }

        /// <summary>
        /// Valida que "Hasta" no sea menor que "Desde". Si es invalido, revierte el campo al ultimo valor
        /// valido y avisa con un toast. Un campo vacio no se considera invalido.
        /// </summary>
        protected void txtHasta_TextChanged(object sender, EventArgs e)
        {
            if (EsRangoValido())
                ViewState[KeyHastaValido] = txtHasta.Text;
            else
            {
                txtHasta.Text = (string)ViewState[KeyHastaValido] ?? "";
                Toasts.ToastAdvertencia(this, "La fecha 'Hasta' no puede ser menor que 'Desde'.", "Rango invalido");
            }
        }

        /// <summary>
        /// El rango es valido si alguno de los campos esta vacio, o si "Desde" &lt;= "Hasta".
        /// </summary>
        private bool EsRangoValido()
        {
            if (!DateTime.TryParse(txtDesde.Text, out DateTime desde))
                return true;
            if (!DateTime.TryParse(txtHasta.Text, out DateTime hasta))
                return true;
            return desde.Date <= hasta.Date;
        }

        protected void lnkSortRutina_Click(object sender, EventArgs e)
        {
            AlternarOrden("Rutina");
        }

        protected void lnkSortFecha_Click(object sender, EventArgs e)
        {
            AlternarOrden("Fecha");
        }

        private void AlternarOrden(string columna)
        {
            if (SortColumn == columna)
                SortAsc = !SortAsc;
            else
            {
                SortColumn = columna;
                SortAsc = true;
            }
            BindGrid();
        }

        protected void rptSesiones_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Toggle")
                return;

            int idSesion = int.Parse(e.CommandArgument.ToString());
            // Acordeon: si ya estaba abierta se cierra, si no se abre (cerrando la anterior).
            IdSesionExpandida = IdSesionExpandida == idSesion ? -1 : idSesion;

            BindGrid();
        }

        protected void rptSesiones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            SesionEntrenamiento sesion = (SesionEntrenamiento)e.Item.DataItem;
            if (sesion.IdSesion != IdSesionExpandida)
                return;

            // Solo aca se consultan las series: unicamente para la sesion desplegada.
            PlaceHolder phDetalle = (PlaceHolder)e.Item.FindControl("phDetalle");
            Repeater rptEjercicios = (Repeater)e.Item.FindControl("rptEjercicios");
            PlaceHolder phSinSeries = (PlaceHolder)e.Item.FindControl("phSinSeries");

            List<SeriesPorEjercicio> grupos = new SerieCompletadaNegocio().GetSeriesAgrupadasDeSesion(sesion.IdSesion);

            phDetalle.Visible = true;
            rptEjercicios.DataSource = grupos;
            rptEjercicios.DataBind();
            phSinSeries.Visible = grupos.Count == 0;
        }

        /// <summary>
        /// Formatea la duracion de una sesion como "Xh Ym" (o "Ym" si es menos de una hora). Usado desde el markup.
        /// </summary>
        protected string FormatearDuracion(DateTime inicio, DateTime fin)
        {
            TimeSpan duracion = fin - inicio;
            if (duracion.TotalMinutes < 1)
                return "-";

            int horas = (int)duracion.TotalHours;
            int minutos = duracion.Minutes;

            return horas > 0 ? $"{horas}h {minutos}m" : $"{minutos}m";
        }
    }
}
