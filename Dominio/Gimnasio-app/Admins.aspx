<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admins.aspx.cs" Inherits="Gimnasio_app.Admins" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row align-items-center mb-4">
            <div class="col-md-6">
                <h3 class="text-primary mb-0">
                    <i class="bi bi-people-fill me-2"></i>Administración de Admins
                </h3>
                <p class="text-muted mb-0">Listado general, búsqueda y gestión de estados de admins.</p>
            </div>
            <div class="col-md-6 text-md-end mt-3 mt-md-0">
                <asp:Button ID="btnRegistrar" runat="server" CssClass="btn btn-primary shadow-sm" Text="Registrar Nuevo Admin" OnClick="btnRegistrar_Click" />
            </div>
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
        <%-- Modo Ejemplo ejemplo --%>

        <%-- Hasta aca llega el ejemplo --%>
    </div>
    <%-- El GridView deberia verse igual que el codigo de arriba (falta probar trayendo los datos de la base de datos) --%>
    <div class="card shadow-sm">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="dgvAdmins" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-hover align-middle mb-0"
                            HeaderStyle-CssClass="table-primary text-white"
                            GridLines="None">

                            <Columns>

                                <%-- Columna ID --%>
                                <asp:BoundField DataField="IdUsuario" HeaderText="ID" ItemStyle-CssClass="ps-3 fw-bold" HeaderStyle-CssClass="ps-3" />

                                <%-- Columna Nombre --%>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"/>

                                <%-- Columna Apellido --%>
                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"/>

                                <%-- Columna Email --%>
                                <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"/>

                                <%-- Columna Fecha de nacimiento --%>
                                <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha de Nacimiento" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"/>

                                <%-- Columna Rol --%>
                                <asp:BoundField DataField="Rol.RolDescripcion" HeaderText="Nombre Rol" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"/>

                                <%-- Columna Fecha de ingresp --%>
                                <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha de ingreso" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"/>

                                <%-- Columna Estado (Podés meter lógica en el code-behind para el look) --%>
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
                                        <a href='FormularioAdmins.aspx?idRol=1&id=<%# Eval("IdUsuario") %>' class="btn btn-outline-primary btn-sm px-3 shadow-sm">
                                            <i class="bi bi-pencil-square me-1"></i>Ver Detalle
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    
                </asp:UpdatePanel>

            </div>
    </div>
    </div>
</asp:Content>
