<%@ Page Title="Detalle de rutina" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleRutina.aspx.cs" Inherits="Gimnasio_app.DetalleRutina" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="space-y-6">
        <%-- Encabezado --%>
        <div class="d-flex justify-content-between align-items-start flex-wrap gap-2">
            <div>
                <h1 class="font-display text-3xl font-semibold tracking-tight">
                    <asp:Literal ID="litNombre" runat="server" />
                </h1>
                <p class="text-muted mt-1 mb-0">
                    <span class="me-3"><i class="bi bi-calendar-week me-1"></i>
                        <asp:Literal ID="litDia" runat="server" /></span>
                    <span><i class="bi bi-clock-history me-1"></i>Creada el
                        <asp:Literal ID="litFecha" runat="server" /></span>
                </p>
            </div>
            <a href="Rutinas.aspx" class="btn btn-outline-secondary btn-sm">
                <i class="bi bi-arrow-left me-1"></i>Volver
            </a>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <%-- Acciones --%>
                <div class="d-flex gap-2 my-3">
                    <asp:Button ID="btnIniciar" runat="server" Text="Iniciar" CssClass="btn btn-success" OnClick="btnIniciar_Click" />
                    <button type="button" class="btn btn-outline-secondary" disabled>Sesiones realizadas</button>
                </div>

                <%-- MODO RESUMEN: rutina aun no iniciada (tabla de solo lectura) --%>
                <asp:PlaceHolder ID="phResumen" runat="server">
                    <div class="card">
                        <div class="card-header fw-semibold">Ejercicios</div>
                        <div class="table-responsive">
                            <table class="table table-hover align-middle mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th style="width: 60px;">#</th>
                                        <th>Ejercicio</th>
                                        <th>Grupo muscular</th>
                                        <th class="text-center">Series</th>
                                        <th class="text-center">Reps</th>
                                        <th class="text-center">Objetivo (kg)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptEjercicios" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-muted"><%# Eval("OrdenEjercicio") %></td>
                                                <td class="fw-semibold">
                                                    <a href='<%# Eval("Ejercicio.LinkExplicacion") %>' target="_blank" rel="noopener">
                                                        <%# Eval("Ejercicio.NombreEjercicio") %></a>
                                                </td>
                                                <td><%# Eval("Ejercicio.GrupoMuscular.NombreGrupoMuscular") %></td>
                                                <td class="text-center"><%# Eval("ObjetivoSeries") %></td>
                                                <td class="text-center"><%# Eval("ObjetivoRepeticiones") %></td>
                                                <td class="text-center"><%# Eval("ObjetivoKG") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                        <asp:PlaceHolder ID="phSinEjercicios" runat="server" Visible="false">
                            <div class="card-body text-center text-muted">
                                Esta rutina todavia no tiene ejercicios asignados.
           
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </asp:PlaceHolder>

                <%-- MODO ACTIVO: sesion en curso, se registran las series ejecutadas --%>
                <asp:PlaceHolder ID="phSesionActiva" runat="server" Visible="false">
                    <div class="alert alert-success d-flex justify-content-between align-items-center" role="alert">
                        <span><i class="bi bi-play-circle me-1"></i>Sesion en curso. Registra cada serie a medida que la completas.</span>
                        <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar sesion" CssClass="btn btn-danger btn-sm" OnClick="btnFinalizar_Click" />
                    </div>

                    <asp:Repeater ID="rptEjecucion" runat="server" OnItemCommand="rptEjecucion_ItemCommand">
                        <ItemTemplate>
                            <div class="card mb-3">
                                <div class="card-header d-flex justify-content-between align-items-center">
                                    <span class="fw-semibold">
                                        <%# Eval("OrdenEjercicio") %>. <a href='<%# Eval("Ejercicio.LinkExplicacion") %>' target="_blank" rel="noopener"><%# Eval("Ejercicio.NombreEjercicio") %></a>
                                        <small class="text-muted ms-2"><%# Eval("Ejercicio.GrupoMuscular.NombreGrupoMuscular") %></small>
                                    </span>
                                    <span class="badge bg-secondary">
                                        <%# ContarSeries((int)Eval("Ejercicio.IdEjercicio")) %> / <%# Eval("ObjetivoSeries") %> series
                                    </span>
                                </div>
                                <div class="card-body">
                                    <p class="text-muted small mb-3">
                                        Objetivo: <%# Eval("ObjetivoSeries") %> series x <%# Eval("ObjetivoRepeticiones") %> reps @ <%# Eval("ObjetivoKG") %> kg
                   
                                    </p>
                                    <div class="row g-2 align-items-end">
                                        <div class="col-auto">
                                            <label class="form-label small mb-1">Peso (kg)</label>
                                            <asp:TextBox ID="txtPeso" runat="server" TextMode="Number" CssClass="form-control" Text='<%# Eval("ObjetivoKG") %>' />
                                        </div>
                                        <div class="col-auto">
                                            <label class="form-label small mb-1">Reps</label>
                                            <asp:TextBox ID="txtReps" runat="server" TextMode="Number" CssClass="form-control" Text='<%# Eval("ObjetivoRepeticiones") %>' />
                                        </div>
                                        <div class="col-auto">
                                            <label class="form-label small mb-1">RIR</label>
                                            <asp:TextBox ID="txtRir" runat="server" TextMode="Number" CssClass="form-control" Text="0" />
                                        </div>
                                        <div class="col-auto">
                                            <asp:HiddenField ID="hfObjetivoSeries" runat="server" Value='<%# Eval("ObjetivoSeries") %>' />
                                            <asp:Button ID="btnRegistrarSerie" runat="server" Text="Registrar serie"
                                                CssClass="btn btn-success" CommandName="RegistrarSerie"
                                                CommandArgument='<%# Eval("Ejercicio.IdEjercicio") %>'
                                                Enabled='<%# ContarSeries((int)Eval("Ejercicio.IdEjercicio")) < (int)Eval("ObjetivoSeries") + 1 %>' />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
