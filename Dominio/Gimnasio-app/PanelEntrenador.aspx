<%@ Page Title="Panel de Entrenador" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntrenadorPanel.aspx.cs" Inherits="Gimnasio_app._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Panel de entrenador</h1>
            <p class="lead">Funciones del panel (en construcción)</p>
            <div class="row g-3 mb-4">
                <div class="col-md-4">
                    <a runat="server" href="~/PanelEntrenadorCrearRutina" class="btn btn-primary w-100 py-3">
                        Crear Rutina
                    </a>
                </div>
                <div class="col-md-4">
                    <a runat="server" href="~/PanelEntrenadorRutinas" class="btn btn-outline-primary w-100 py-3">
                        Panel de Rutinas
                    </a>
                </div>
                <div class="col-md-4">
                    <a runat="server" href="~/RutinasAsignadas" class="btn btn-outline-primary w-100 py-3">
                        Rutinas Asignadas
                    </a>
                </div>
            </div>
            <div class="card shadow-sm mb-4">
                <di class="card-body">
                    <h5 class="card-title">Buscar Cliente</h5>
                    <div class="input-group">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" 
                            Placeholder="Nombre o apellido..." />
                        <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" 
                            Text="Buscar" />
                    </div>
            </div>
        </section>

        

    </main>

</asp:Content>
