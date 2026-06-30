<%@ Page Title="Panel de Entrenador" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenador.aspx.cs" Inherits="Gimnasio_app.PanelEntrenador" %>

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
                    <!-- PENDIENTE: Crear página RutinasAsignadas.aspx -->
                    <a runat="server" href="~/RutinasAsignadas" class="btn btn-outline-primary w-100 py-3">
                        Rutinas Asignadas
                    </a>
                </div>
            </div>
            <div class="card shadow-sm mb-4">
                <div class="card-body">
                    <h5 class="card-title">Buscar Cliente</h5>
                    <div class="input-group">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" 
                            Placeholder="Nombre o apellido..." />
                        <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" 
                            Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>
            </div>
            <div class="card shadow-sm mb-4">
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="dgvClientes" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-hover align-middle mb-0"
                            HeaderStyle-CssClass="table-primary text-white"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                <asp:TemplateField HeaderText="Edad">
                                    <ItemTemplate>
                                        <%# CalcularEdad(Eval("FechaNacimiento")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PesoCorporal" HeaderText="Peso (kg)" />
                                <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha de Ingreso" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <!-- PENDIENTE: Crear PanelEntrenadorClienteDetalle.aspx para ver datos del cliente -->
                                        <a href=PanelEntrenadorAsignarRutina.aspx?idCliente=<%# Eval("IdUsuario") %>' class="btn btn-outline-primary btn-sm">
                                            Ver Detalle
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </section>
    </main>

</asp:Content>
