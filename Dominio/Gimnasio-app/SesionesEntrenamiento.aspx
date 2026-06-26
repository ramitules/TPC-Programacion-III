<%@ Page Title="Sesiones de entrenamiento" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SesionesEntrenamiento.aspx.cs" Inherits="Gimnasio_app.SesionesEntrenamiento" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="space-y-6">
        <%-- Encabezado --%>
        <div class="d-flex justify-content-between align-items-start flex-wrap gap-2">
            <div>
                <h1 class="font-display text-3xl font-semibold tracking-tight">Sesiones de entrenamiento</h1>
                <p class="text-muted mt-1 mb-0">Historial de tus entrenamientos. Desplega una sesion para ver las series realizadas.</p>
            </div>
            <a href="Rutinas.aspx" class="btn btn-outline-secondary btn-sm">
                <i class="bi bi-arrow-left me-1"></i>Volver
            </a>
        </div>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <%-- Filtros --%>
                <div class="card my-3">
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-4">
                                <label class="form-label small mb-1">Rutina</label>
                                <asp:DropDownList ID="ddlRutina" runat="server" CssClass="form-select" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label small mb-1">Desde</label>
                                <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDesde_TextChanged" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label small mb-1">Hasta</label>
                                <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtHasta_TextChanged" />
                            </div>
                            <div class="col-md-2 d-flex gap-2">
                                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary w-100" OnClick="btnFiltrar_Click" />
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary w-100" OnClick="btnLimpiar_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <%-- Tabla de sesiones --%>
                <div class="card">
                    <div class="table-responsive">
                        <table class="table table-hover align-middle mb-0">
                            <thead class="table-light">
                                <tr>
                                    <th>
                                        <asp:LinkButton ID="lnkSortRutina" runat="server" CssClass="text-decoration-none text-reset fw-semibold" OnClick="lnkSortRutina_Click">
                                            Rutina <asp:Literal ID="litCaretRutina" runat="server" />
                                        </asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton ID="lnkSortFecha" runat="server" CssClass="text-decoration-none text-reset fw-semibold" OnClick="lnkSortFecha_Click">
                                            Fecha <asp:Literal ID="litCaretFecha" runat="server" />
                                        </asp:LinkButton>
                                    </th>
                                    <th class="text-center">Duracion</th>
                                    <th class="text-center">Series</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSesiones" runat="server" OnItemCommand="rptSesiones_ItemCommand" OnItemDataBound="rptSesiones_ItemDataBound">
                                    <ItemTemplate>
                                        <tr style="cursor:pointer;" onclick="this.querySelector('a[data-row-action]').click()">
                                            <td class="fw-semibold">
                                                <asp:LinkButton runat="server" CssClass="d-none" data-row-action="toggle" CommandName="Toggle"
                                                    CommandArgument='<%# Eval("IdSesion") %>'>&nbsp;</asp:LinkButton>
                                                <i class='<%# (int)Eval("IdSesion") == IdSesionExpandida ? "bi bi-chevron-down me-2" : "bi bi-chevron-right me-2" %>'></i>
                                                <%# Eval("Rutina") == null ? "<span class='text-muted'>Sesion libre</span>" : Server.HtmlEncode(Eval("Rutina.Nombre").ToString()) %>
                                            </td>
                                            <td><%# Eval("FechaHoraInicio", "{0:dd/MM/yyyy HH:mm}") %></td>
                                            <td class="text-center"><%# FormatearDuracion((DateTime)Eval("FechaHoraInicio"), (DateTime)Eval("FechaHoraFin")) %></td>
                                            <td class="text-center"><span class="badge bg-secondary"><%# Eval("CantidadSeries") %></span></td>
                                        </tr>
                                        <%-- Fila de detalle (se llena solo si la sesion esta expandida) --%>
                                        <tr>
                                            <td colspan="4" class="p-0 border-0">
                                                <asp:PlaceHolder ID="phDetalle" runat="server" Visible="false">
                                                    <div class="p-3 bg-light">
                                                        <asp:Repeater ID="rptEjercicios" runat="server">
                                                            <ItemTemplate>
                                                                <div class="card mb-2">
                                                                    <div class="card-header py-2 d-flex justify-content-between align-items-center">
                                                                        <span class="fw-semibold"><%# Eval("NombreEjercicio") %></span>
                                                                        <small class="text-muted"><%# Eval("NombreGrupoMuscular") %></small>
                                                                    </div>
                                                                    <div class="table-responsive">
                                                                        <table class="table table-sm mb-0">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th style="width: 60px;">#</th>
                                                                                    <th class="text-center">Peso (kg)</th>
                                                                                    <th class="text-center">Reps</th>
                                                                                    <th class="text-center">RIR</th>
                                                                                    <th></th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:Repeater ID="rptSeries" runat="server" DataSource='<%# Eval("Series") %>'>
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td class="text-muted"><%# Container.ItemIndex + 1 %></td>
                                                                                            <td class="text-center"><%# Eval("PesoLevantadoKG") %></td>
                                                                                            <td class="text-center"><%# Eval("RepeticionesLogradas") %></td>
                                                                                            <td class="text-center"><%# Eval("RIR") %></td>
                                                                                            <td>
                                                                                                <%# (bool)Eval("EsRecordPersonal") ? "<span class='badge bg-warning text-dark'><i class='bi bi-trophy me-1'></i>Record</span>" : "" %>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <asp:PlaceHolder ID="phSinSeries" runat="server" Visible="false">
                                                            <div class="text-center text-muted py-2">Esta sesion no tiene series registradas.</div>
                                                        </asp:PlaceHolder>
                                                    </div>
                                                </asp:PlaceHolder>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <asp:PlaceHolder ID="phSinSesiones" runat="server" Visible="false">
                        <div class="card-body text-center text-muted">
                            No se encontraron sesiones de entrenamiento con los filtros seleccionados.
                        </div>
                    </asp:PlaceHolder>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
