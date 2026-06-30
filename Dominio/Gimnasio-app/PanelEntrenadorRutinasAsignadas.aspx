<%@ Page Title="Rutinas Asignadas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenadorRutinasAsignadas.aspx.cs" Inherits="Gimnasio_app.PanelEntrenadorRutinasAsignadas"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Rutinas Asignadas</h1>
            <p class="lead">Rutinas asignadas a clientes</p>
            
            <asp:GridView ID="gvRutinasAsignadas" runat="server" AutoGenerateColumns="false"
                CssClass="table table-hover align-middle mb-0"
                HeaderStyle-CssClass="table-primary text-white"
                GridLines="None"
                EmptyDataText="No hay rutinas asignadas.">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:TemplateField HeaderText="Cliente">
                        <ItemTemplate>
                            <%# Eval("Cliente.Nombre") %> <%# Eval("Cliente.Apellido") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Dia" HeaderText="Día" />
                    <asp:BoundField DataField="FechaCreacion" HeaderText="Creada" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </asp:GridView>
        </section>
    </main>
</asp:Content>