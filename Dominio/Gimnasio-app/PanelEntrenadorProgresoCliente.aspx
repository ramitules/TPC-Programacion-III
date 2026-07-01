<%@ Page Title="Progreso del Cliente" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenadorProgresoCliente.aspx.cs" Inherits="Gimnasio_app.PanelEntrenadorProgresoCliente" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section>
            <div class="d-flex justify-content-between align-items-start flex-wrap gap-2 mb-4">
                <div>
                    <h1>Progreso de <asp:Label ID="lblNombreCliente" runat="server" /></h1>
                    <p class="lead text-muted">Historial de sesiones de entrenamiento</p>
                </div>
                <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-outline-secondary" OnClick="btnVolver_Click" />
                    <i class="bi bi-arrow-left me-1"></i>Volver
                </a>
            </div>

            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">Sesiones de Entrenamiento</h5>
                    <asp:GridView ID="gvSesiones" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-hover align-middle mb-0"
                        HeaderStyle-CssClass="table-primary text-white"
                        GridLines="None"
                        OnRowDataBound="gvSesiones_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Rutina.Nombre" HeaderText="Rutina" NullDisplayText="Sesión libre" />
                            <asp:BoundField DataField="FechaHoraInicio" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:TemplateField HeaderText="Duración">
                                <ItemTemplate>
                                    <asp:Label ID="lblDuracion" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CantidadSeries" HeaderText="Series" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkVerDetalle" runat="server" CssClass="btn btn-sm btn-outline-primary">
                                        <i class="bi bi-eye"></i> Ver Detalle
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblSinSesiones" runat="server" CssClass="text-muted d-block text-center mt-3" Visible="false"
                        Text="Este cliente no tiene sesiones de entrenamiento registradas." />
                </div>
            </div>
        </section>
    </main>
</asp:Content>
