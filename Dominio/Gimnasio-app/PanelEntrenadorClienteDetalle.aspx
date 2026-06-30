<%@ Page Title="Detalle del Cliente" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenadorClienteDetalle.aspx.cs" Inherits="Gimnasio_app.PanelEntrenadorClienteDetalle" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section>
            <h1>Detalle del Cliente</h1>
            <p class="lead">
                <asp:Label ID="lblNombreCliente" runat="server" />
            </p>

            <div class="card shadow-sm mb-4">
                <div class="card-body">
                    <h5 class="card-title">Datos del Cliente</h5>
                    <div class="row">
                        <div class="col-md-3">
                            <strong>Nombre:</strong>
                            <asp:Label ID="lblNombre" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <strong>Apellido:</strong>
                            <asp:Label ID="lblApellido" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <strong>Edad:</strong>
                            <asp:Label ID="lblEdad" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <strong>Peso (kg):</strong>
                            <asp:Label ID="lblPeso" runat="server" />
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-md-6">
                            <strong>Fecha de Ingreso:</strong>
                            <asp:Label ID="lblFechaIngreso" runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card shadow-sm mb-4">
                <div class="card-body">
                    <h5 class="card-title">Rutinas Asignadas</h5>
                    <asp:GridView ID="gvRutinas" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-hover align-middle mb-0"
                        HeaderStyle-CssClass="table-primary text-white"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha de Creación" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Dia" HeaderText="Día" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="row g-3 mb-4">
                <div class="col-md-4">
                    <asp:Button ID="btnCrearRutina" runat="server" CssClass="btn btn-primary w-100 py-3" Text="Crear Rutina" OnClick="btnCrearRutina_Click" />
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnVerProgreso" runat="server" CssClass="btn btn-info w-100 py-3" Text="Ver Progreso" OnClick="btnVerProgreso_Click" />
                </div>
                <div class="col-md-4">
                    <a runat="server" href="~/PanelEntrenador" class="btn btn-outline-secondary w-100 py-3"> Volver </a>
                </div>
            </div>
        </section>
    </main>

</asp:Content>
