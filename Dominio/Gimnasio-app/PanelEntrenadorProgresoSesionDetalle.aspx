<%@ Page Title="Detalle de Sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenadorProgresoSesionDetalle.aspx.cs" Inherits="Gimnasio_app.PanelEntrenadorProgresoSesionDetalle" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section>
            <div class="d-flex justify-content-between align-items-start flex-wrap gap-2 mb-4">
                <div>
                    <h1>Detalle de Sesión</h1>
                    <p class="lead text-muted">
                        <asp:Label ID="lblFechaSesion" runat="server" />
                    </p>
                </div>
                <asp:Button ID="btnVolver" runat="server" Text="Volver" 
                    CssClass="btn btn-outline-secondary" OnClick="btnVolver_Click" />
            </div>

            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">Series Realizadas</h5>
                    <asp:GridView ID="gvSeries" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-hover align-middle mb-0"
                        HeaderStyle-CssClass="table-primary text-white"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Ejercicio.NombreEjercicio" HeaderText="Ejercicio" />
                            <asp:BoundField DataField="Ejercicio.GrupoMuscular.NombreGrupoMuscular" HeaderText="Grupo Muscular" />
                            <asp:BoundField DataField="PesoLevantadoKG" HeaderText="Peso (kg)" />
                            <asp:BoundField DataField="RepeticionesLogradas" HeaderText="Reps" />
                            <asp:BoundField DataField="RIR" HeaderText="RIR" />
                            <asp:TemplateField HeaderText="Record">
                                <ItemTemplate>
                                    <%# (bool)Eval("EsRecordPersonal") ? "<span class='badge bg-warning text-dark'><i class='bi bi-trophy me-1'></i>Record</span>" : "" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblSinSeries" runat="server" CssClass="text-muted d-block text-center mt-3" Visible="false"
                        Text="Esta sesión no tiene series registradas." />
                </div>
            </div>
        </section>
    </main>
</asp:Content>
