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
                    <span class="me-3"><i class="bi bi-calendar-week me-1"></i><asp:Literal ID="litDia" runat="server" /></span>
                    <span><i class="bi bi-clock-history me-1"></i>Creada el <asp:Literal ID="litFecha" runat="server" /></span>
                </p>
            </div>
            <a href="Rutinas.aspx" class="btn btn-outline-secondary btn-sm">
                <i class="bi bi-arrow-left me-1"></i>Volver
            </a>
        </div>

        <%-- Acciones --%>
        <div class="d-flex gap-2 my-3">
            <asp:Button ID="btnIniciar" runat="server" Text="Iniciar" CssClass="btn btn-success" />
            <button type="button" class="btn btn-outline-secondary" disabled>Sesiones realizadas</button>
        </div>

        <%-- Ejercicios --%>
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
                                    <td class="fw-semibold"><%# Eval("Ejercicio.NombreEjercicio") %></td>
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
    </div>
</asp:Content>
