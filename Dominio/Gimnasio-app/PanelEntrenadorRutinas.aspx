<%@ Page Title="Panel de Entrenador: Rutinas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenadorRutinas.aspx.cs" Inherits="Gimnasio_app.PanelEntrenadorRutinas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Panel de entrenador</h1>
            <p class="lead">Rutinas generales disponibles</p>
            <asp:GridView ID="gvRutinas" runat="server" AutoGenerateColumns="false" CssClass="table table-hover align-middle mb-0" GridLines="None" EmptyDataText="No hay rutinas generales.">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="FechaCreacion" HeaderText="Creada" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </asp:GridView>
        </section>       

    </main>

</asp:Content>
