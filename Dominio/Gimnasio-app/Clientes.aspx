<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Gimnasio_app.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row align-items-center mb-4">
            <div class="col-md-6">
                <h3 class="text-primary mb-0">
                    <i class="bi bi-people-fill me-2"></i>Administración de Clientes
            </h3>
                <p class="text-muted mb-0">Listado general, búsqueda y gestión de estados de clientes.</p>
            </div>
            <%--<div class="col-md-6 text-md-end mt-3 mt-md-0">
                <asp:Button ID="btnRegistrar" runat="server" CssClass="btn btn-primary shadow-sm" Text="Registrar Nuevo Cliente" />
            </div>--%>
        </div>

        <div class="card shadow-sm mb-4">
            <div class="card-body bg-light">
                <div class="row g-3 align-items-center">
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-text bg-white text-muted">
                                <i class="bi bi-search"></i>
                            </span>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Placeholder="Buscar por Nombre y/o Apellido" OnTextChanged="txtBuscar_TextChanged" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlEstadoSuscripcion" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoSuscripcion_SelectedIndexChanged">
                            <%--<asp:ListItem Value="todos" Selected="True">Todos los estados</asp:ListItem>
                            <asp:ListItem Value="activos">Activos</asp:ListItem>
                            <asp:ListItem Value="inactivos">Inactivos</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                            <asp:ListItem Value="todos" Selected="True">Todos los estados</asp:ListItem>
                            <asp:ListItem Value="activos">Activos</asp:ListItem>
                            <asp:ListItem Value="inactivos">Inactivos</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-outline-secondary w-100" Text="Filtrar" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <%-- El GridView deberia verse igual que el codigo de arriba (falta probar trayendo los datos de la base de datos) --%>
    <div class="card shadow-sm">
        <div class="card-body p-0">
            <div class="table-responsive">

                <asp:GridView ID="dgvClientes" runat="server" AutoGenerateColumns="false" OnRowCommand="dgvClientes_RowCommand"
                    CssClass="table table-hover align-middle mb-0"
                    HeaderStyle-CssClass="table-primary text-white"
                    GridLines="None">

                    <Columns>

                        <%-- Columna ID --%>
                        <asp:BoundField DataField="IdUsuario" HeaderText="ID" ItemStyle-CssClass="ps-3 fw-bold" HeaderStyle-CssClass="ps-3" />

                        <%-- Columna Nombre --%>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />

                        <%-- Columna Apellido --%>
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />

                        <%-- Columna Email --%>
                        <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />

                        <%-- Columna Fecha de nacimientp --%>
                        <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha de nacimiento" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />

                        <%-- Columna Fecha de ingreso --%>
                        <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha de ingreso" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />

                        <%-- Columna Peso corporal --%>
                        <asp:BoundField DataField="PesoCorporal" HeaderText="Peso" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />

                        <%-- Columna Estado Suscripcion --%>
                        <asp:TemplateField HeaderText="Suscripción" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <%# ObtenerBadgeEstado(Eval("SuscripcionCliente.Estado")) %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Columna Estado --%>
                        <asp:TemplateField HeaderText="Activo" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <span class="badge <%# Convert.ToBoolean(Eval("Activo")) ? "bg-success" : "bg-danger" %>">
                                    <%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <%-- Columna Acciones (Última columna con el botón de Bootstrap) --%>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEstado" runat="server" OnClick="btnEstado_Click" 
                                    CommandArgument='<%# Eval("IdUsuario") %>'
                                    CommandName="CambiarEstado"
                                    CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-outline-danger btn-sm px-3 shadow-sm" : "btn btn-outline-success btn-sm px-3 shadow-sm" %>'>
            
                                    <i class='<%# Convert.ToBoolean(Eval("Activo")) ? "bi bi-person-x me-1" : "bi bi-person-check me-1" %>'></i>
                                    <%# Convert.ToBoolean(Eval("Activo")) ? "Inactivar Cliente" : "Activar Cliente" %>
                                    
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
