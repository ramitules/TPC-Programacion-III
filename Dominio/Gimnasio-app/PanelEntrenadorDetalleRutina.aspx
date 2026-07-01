<%@ Page Title="Detalle de Rutina" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenadorDetalleRutina.aspx.cs" Inherits="Gimnasio_app.PanelEntrenadorDetalleRutina" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section>
            <div class="d-flex justify-content-between align-items-start flex-wrap gap-2 mb-4">
                <div>
                    <h1>
                        <asp:Label ID="lblNombreRutina" runat="server" />
                    </h1>
                    <p class="lead text-muted">
                        <asp:Label ID="lblDia" runat="server" /> - Creada el <asp:Label ID="lblFechaCreacion" runat="server" />
                    </p>
                </div>
                <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-outline-secondary" OnClick="btnVolver_Click" />
            </div>

            <div class="card shadow-sm mb-4">
                <div class="card-body">
                    <h5 class="card-title">Ejercicios</h5>
                    <asp:GridView ID="gvEjercicios" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-hover align-middle mb-0"
                        HeaderStyle-CssClass="table-primary text-white"
                        GridLines="None"
                        EmptyDataText="Esta rutina no tiene ejercicios asignados.">
                        <Columns>
                            <asp:BoundField DataField="OrdenEjercicio" HeaderText="#" />
                            <asp:BoundField DataField="Ejercicio.NombreEjercicio" HeaderText="Ejercicio" />
                            <asp:BoundField DataField="Ejercicio.GrupoMuscular.NombreGrupoMuscular" HeaderText="Grupo Muscular" />
                            <asp:BoundField DataField="ObjetivoSeries" HeaderText="Series" />
                            <asp:BoundField DataField="ObjetivoRepeticiones" HeaderText="Reps" />
                            <asp:BoundField DataField="ObjetivoKG" HeaderText="Peso (kg)" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="row g-3 mb-4">
                <div class="col-md-6">
                    <asp:Button ID="btnUsarPlantilla" runat="server" Text="Usar como Plantilla" CssClass="btn btn-primary w-100 py-3" OnClick="btnUsarPlantilla_Click" />
                </div>
                <div class="col-md-6">
                    <asp:Button ID="btnAsignarCliente" runat="server" Text="Asignar a Cliente" CssClass="btn btn-info w-100 py-3" OnClick="btnAsignarCliente_Click" />
                </div>
            </div>
        </section>
    </main>
</asp:Content>
