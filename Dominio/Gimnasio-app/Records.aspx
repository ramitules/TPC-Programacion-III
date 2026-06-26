<%@ Page Title="Records personales" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Records.aspx.cs" Inherits="Gimnasio_app.Records" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="space-y-6">
        <%-- Encabezado --%>
        <div class="d-flex justify-content-between align-items-start flex-wrap gap-2">
            <div>
                <h1 class="font-display text-3xl font-semibold tracking-tight">Records personales</h1>
                <p class="text-muted mt-1 mb-0">Tu mejor marca en cada ejercicio: el peso mas alto que levantaste y cuando lo lograste.</p>
            </div>
            <a href="SesionesEntrenamiento.aspx" class="btn btn-outline-secondary btn-sm">
                <i class="bi bi-clock-history me-1"></i>Historial
            </a>
        </div>

        <%-- Tabla de records --%>
        <div class="card my-3">
            <div class="table-responsive">
                <table class="table table-hover align-middle mb-0">
                    <thead class="table-light">
                        <tr>
                            <th>Ejercicio</th>
                            <th>Grupo muscular</th>
                            <th class="text-center">Peso (kg)</th>
                            <th class="text-center">Reps</th>
                            <th class="text-center">Fecha</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptRecords" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="fw-semibold">
                                        <i class="bi bi-trophy text-warning me-2"></i><%# Server.HtmlEncode(Eval("Ejercicio.NombreEjercicio").ToString()) %>
                                    </td>
                                    <td>
                                        <%# Eval("Ejercicio.GrupoMuscular.NombreGrupoMuscular") == null || Eval("Ejercicio.GrupoMuscular.NombreGrupoMuscular").ToString() == ""
                                            ? "<span class='text-muted'>-</span>"
                                            : Server.HtmlEncode(Eval("Ejercicio.GrupoMuscular.NombreGrupoMuscular").ToString()) %>
                                    </td>
                                    <td class="text-center"><span class="badge bg-warning text-dark"><%# Eval("PesoKG") %> kg</span></td>
                                    <td class="text-center"><%# Eval("Repeticiones") %></td>
                                    <td class="text-center"><%# Eval("FechaRecord", "{0:dd/MM/yyyy}") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <asp:PlaceHolder ID="phSinRecords" runat="server" Visible="false">
                <div class="card-body text-center text-muted">
                    Todavia no registraste records personales. Completa una sesion de entrenamiento para empezar a marcarlos.
                </div>
            </asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
