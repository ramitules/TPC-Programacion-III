<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminControl.aspx.cs" Inherits="Gimnasio_app.AdminControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Admin Control</h1>

    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <h3 class="text-center mb-4 text-primary">Panel de Control General</h3>

                <ul class="list-group shadow-sm">

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-people-fill text-primary fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Clientes</h5>
                                <small class="text-muted">Gestionar altas, bajas, modificaciones y suscripciones.</small>
                            </div>
                        </div>
                        <a href="Clientes.aspx" class="btn btn-outline-primary btn-sm">Acceder</a>
                    </li>

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-person-badge text-success fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Entrenadores</h5>
                                <small class="text-muted">Administrar profesores, especialidades y horarios.</small>
                            </div>
                        </div>
                        <a href="Entrenadores.aspx" class="btn btn-outline-success btn-sm">Acceder</a>
                    </li>

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-person-workspace text-warning fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Recepcionistas</h5>
                                <small class="text-muted">Control de personal de atención y asignación de turnos.</small>
                            </div>
                        </div>
                        <a href="Recepcionistas.aspx" class="btn btn-outline-warning btn-sm">Acceder</a>
                    </li>

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-shield-lock-fill text-danger fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Administradores (Admins)</h5>
                                <small class="text-muted">Configuración del sistema y permisos de cuentas staff.</small>
                            </div>
                        </div>
                        <a href="Admins.aspx" class="btn btn-outline-danger btn-sm">Acceder</a>
                    </li>

                </ul>
            </div>
        </div>
    </div>
    <br />



    <asp:UpdatePanel ID="panelGeneral" runat="server">
        <ContentTemplate>
            <ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item"><a class="nav-link active" data-bs-toggle="tab" href="#ejercicios">Ejercicios</a></li>
                <li class="nav-item"><a class="nav-link" data-bs-toggle="tab" href="#musculos">Grupos Musculares</a></li>
                <li class="nav-item"><a class="nav-link" data-bs-toggle="tab" href="#planes">Planes</a></li>
            </ul>

            <div class="tab-content mt-3">
                <div class="tab-pane fade show active" id="ejercicios">
                    <asp:Button ID="btnNuevoEjercicio" runat="server" Text="+ Nuevo Ejercicio" CssClass="btn btn-primary mb-2" OnClick="btnNuevoEjercicio_Click" CommandArgument="Ejercicio" />
                    <asp:GridView ID="dgvEjercicios" runat="server" CssClass="table table-dark table-hover" OnRowCommand="dgvEjercicios_RowCommand" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="IdEjercicio" HeaderText="Id" />
                            <asp:BoundField DataField="NombreEjercicio" HeaderText="Ejercicio" />
                            <asp:BoundField DataField="GrupoMuscular.NombreGrupoMuscular" HeaderText="Grupo Muscular" />
                            <asp:BoundField DataField="LinkExplicacion" HeaderText="Referencia" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnEditarEjer" runat="server" Text="✏️" CommandName="Editar" CommandArgument='<%# Eval("IdEjercicio") %>' CssClass="btn btn-sm btn-warning" />
                                    <asp:Button ID="btnEliminarEjer" runat="server" Text="🗑️" CommandName="Eliminar" CommandArgument='<%# Eval("IdEjercicio") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este ejercicio? Esta acción no se puede deshacer.');"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="tab-pane fade" id="musculos">
                    <asp:Button ID="btnNuevoMusculo" runat="server" Text="+ Nuevo Grupo" CssClass="btn btn-primary mb-2" OnClick="btnNuevoMusculo_Click" CommandArgument="Musculo" />
                    <asp:GridView ID="dgvGruposMusculares" runat="server" CssClass="table table-dark table-hover" OnRowCommand="dgvGruposMusculares_RowCommand" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="IdGrupoMuscular" HeaderText="Id Grupo Muscular" />
                            <asp:BoundField DataField="NombreGrupoMuscular" HeaderText="Grupo Muscular" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnEditarMus" runat="server" Text="✏️" CommandName="Editar" CommandArgument='<%# Eval("IdGrupoMuscular") %>' CssClass="btn btn-sm btn-warning" />
                                    <asp:Button ID="btnEliminarMus" runat="server" Text="🗑️" CommandName="Eliminar" CommandArgument='<%# Eval("IdGrupoMuscular") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este ejercicio? Esta acción no se puede deshacer.');"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="tab-pane fade" id="planes">
                    <asp:Button ID="btnNuevoPlan" runat="server" Text="+ Nuevo Plan" CssClass="btn btn-primary mb-2" OnClick="btnNuevoPlan_Click" CommandArgument="Plan" />
                    <asp:GridView ID="dgvPlanes" runat="server" CssClass="table table-dark table-hover" OnRowCommand="dgvPlanes_RowCommand" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="IdPlan" HeaderText="Id Plan" />
                            <asp:BoundField DataField="NombrePlan" HeaderText="Nombre" />
                            <asp:BoundField DataField="PrecioPlan" HeaderText="Precio Mensual" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="DuracionDiasPlan" HeaderText="Dias Mensuales" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnEditarPlan" runat="server" Text="✏️" CommandName="Editar" CommandArgument='<%# Eval("IdPlan") %>' CssClass="btn btn-sm btn-warning" />
                                    <asp:Button ID="btnEliminarPlan" runat="server" Text="🗑️" CommandName="Eliminar" CommandArgument='<%# Eval("IdPlan") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este ejercicio? Esta acción no se puede deshacer.');"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:Panel ID="pnlFormularioABM" runat="server" Visible="false" CssClass="card bg-dark text-white mt-4 border-secondary">
                <div class="card-header border-secondary">
                    <asp:Label ID="lblTituloForm" runat="server" Font-Bold="true" CssClass="h5 text-warning"></asp:Label>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label">Nombre / Descripción:</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control bg-secondary text-white border-0"></asp:TextBox>
                    </div>

                    <div id="divCamposPlan" runat="server" visible="false">
                        <div class="mb-3">
                            <label class="form-label">Precio Mensual:</label>
                            <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control bg-secondary text-white border-0" placeholder="0.00"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Días Mensuales:</label>
                            <asp:TextBox ID="txtDias" runat="server" CssClass="form-control bg-secondary text-white border-0" placeholder="30"></asp:TextBox>
                        </div>
                    </div>

                    <div id="divCamposEjercicio" runat="server" visible="false">
                        <div class="mb-3">
                            <label class="form-label">Link de Explicación (URL):</label>
                            <asp:TextBox ID="txtLink" runat="server" CssClass="form-control bg-secondary text-white border-0"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Grupo Muscular:</label>
                            <asp:DropDownList ID="ddlGrupoMuscular" DataTextField="NombreGrupoMuscular" DataValueField="IdGrupoMuscular" runat="server" CssClass="form-select bg-secondary text-white border-0"></asp:DropDownList>
                        </div>
                    </div>

                    <asp:HiddenField ID="hfIdEntidad" runat="server" />
                    <asp:HiddenField ID="hfTipoEntidad" runat="server" />

                    <div class="mt-4">
                        <asp:Button ID="btnGuardar" runat="server" Text="💾 Guardar Cambios" CssClass="btn btn-success me-2" OnClick="btnGuardar_Click"/>
                        <asp:Button ID="btnCancelar" runat="server" Text="❌ Cancelar" CssClass="btn btn-outline-light" OnClick="btnCancelar_Click"/>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
