<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Recepcionistas.aspx.cs" Inherits="Gimnasio_app.Recepcionistas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row align-items-center mb-4">
            <div class="col-md-6">
                <h3 class="text-primary mb-0">
                    <i class="bi bi-people-fill me-2"></i>Administración de Recepcionistas
        </h3>
                <p class="text-muted mb-0">Listado general, búsqueda y gestión de recepcionistas.</p>
            </div>
            <div class="col-md-6 text-md-end mt-3 mt-md-0">
                <asp:Button ID="btnRegistrar" runat="server" CssClass="btn btn-primary shadow-sm" Text="Registrar Nuevo Recepcionista" />
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
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Placeholder="Buscar por Nombre, Apellido o DNI..." />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" AutoPostBack="true">
                            <asp:ListItem Value="todos">Todos los estados</asp:ListItem>
                            <asp:ListItem Value="activos" Selected="True">Activos</asp:ListItem>
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
        <div class="card shadow-sm">
            <div class="card-body p-0">
                <div class="table-responsive">

                    <table class="table table-hover align-middle mb-0">
                        <thead class="table-primary text-white">
                            <tr>
                                <th scope="col" class="ps-3">ID</th>
                                <th scope="col">Nombre y Apellido</th>
                                <th scope="col">DNI</th>
                                <th scope="col">Suscripción</th>
                                <th scope="col">Estado</th>
                                <th scope="col" class="text-center">Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="ps-3 fw-bold">1024</td>
                                <td>
                                    <div class="d-flex align-items-center">
                                        <div class="bg-light rounded-circle p-2 me-3 text-primary">
                                            <i class="bi bi-person-circle fs-5"></i>
                                        </div>
                                        <span>Juan Pérez</span>
                                    </div>
                                </td>
                                <td>38.123.456</td>
                                <td><span class="badge bg-info text-dark">Pase Libre Mensual</span></td>
                                <td><span class="badge bg-success">Activo</span></td>
                                <td class="text-center">
                                    <a href="FormularioCliente.aspx?id=1024" class="btn btn-outline-primary btn-sm px-3 shadow-sm">
                                        <i class="bi bi-pencil-square me-1"></i>Ver Detalle
                                    </a>
                                </td>
                            </tr>

                            <tr>
                                <td class="ps-3 fw-bold">1025</td>
                                <td>
                                    <div class="d-flex align-items-center">
                                        <div class="bg-light rounded-circle p-2 me-3 text-secondary">
                                            <i class="bi bi-person-circle fs-5"></i>
                                        </div>
                                        <span class="text-muted">María Rodríguez</span>
                                    </div>
                                </td>
                                <td>40.987.654</td>
                                <td><span class="badge bg-light text-muted">Ninguno</span></td>
                                <td><span class="badge bg-danger">Inactivo</span></td>
                                <td class="text-center">
                                    <a href="FormularioRecepcionista.aspx?id=1025" class="btn btn-outline-primary btn-sm px-3 shadow-sm">
                                        <i class="bi bi-pencil-square me-1"></i>Ver Detalle
                                    </a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <%-- Hasta aca llega el ejemplo --%>
    </div>
    <%-- El GridView deberia verse igual que el codigo de arriba (falta probar trayendo los datos de la base de datos) --%>
    <div class="card shadow-sm">
        <div class="card-body p-0">
            <div class="table-responsive">

                <asp:GridView ID="dgvRecepcionistas" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-hover align-middle mb-0"
                    HeaderStyle-CssClass="table-primary text-white"
                    GridLines="None">

                    <Columns>

                        <%-- Columna ID --%>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="ps-3 fw-bold" HeaderStyle-CssClass="ps-3" />

                        <%-- Columna Nombre y Apellido --%>
                        <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre y Apellido" />

                        <%-- Columna DNI --%>
                        <asp:BoundField DataField="Dni" HeaderText="DNI" />

                        <%-- Columna Estado (Podés meter lógica en el code-behind para el look) --%>
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />

                        <%-- Columna Acciones (Última columna con el botón de Bootstrap) --%>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <a href='FormularioRecepcionista.aspx?id=<%# Eval("Id") %>' class="btn btn-outline-primary btn-sm px-3 shadow-sm">
                                    <i class="bi bi-pencil-square me-1"></i>Ver Detalle
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
