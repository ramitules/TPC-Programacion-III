<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="Gimnasio_app.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="space-y-6">
        <div>
            <h1 class="font-display text-3xl font-semibold tracking-tight">Mi perfil</h1>
            <p class="text-muted-foreground mt-1">Configuracion general de tu perfil</p>
        </div>

        <div>
            <ul class="nav nav-tabs" id="perfilTabs" role="tablist">
                <li class="nav-item" role="presentation">
                    <a class="nav-link active" id="personales-tab" data-bs-toggle="tab" href="#datosPersonales" role="tab" aria-controls="datosPersonales" aria-selected="true">Datos personales
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="suscripcion-tab" data-bs-toggle="tab" href="#datosSuscripcion" role="tab" aria-controls="datosSuscripcion" aria-selected="false">Suscripcion
                    </a>
                </li>
            </ul>
        </div>
    </div>

    <div class="tab-content mt-4" id="perfilTabsContent">

        <div class="tab-pane fade show active" id="datosPersonales" role="tabpanel" aria-labelledby="personales-tab">
            <div class="row">
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label">Nombre</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNombre" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Apellido</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtApellido" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Email</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Fecha de nacimiento</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNacimiento" TextMode="Date" ReadOnly="true" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label">Peso actual</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPeso" TextMode="Number" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Fecha de ingreso</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtIngreso" TextMode="Date" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <asp:Button ID="btnEditarDatos" OnClick="btnEditarDatos_click" Text="Editar" CssClass="btn btn-outline-info" runat="server" />
                        <%if (Editando)
                            { %>
                        <asp:Button ID="btnCancelarDatos" OnClick="btnCancelarDatos_click" Text="Cancelar" CssClass="btn btn-outline-warning" runat="server" />
                        <%} %>
                    </div>
                    <div class="mb-3 mt-4">
                        <asp:Button ID="btnDarDeBaja" OnClick="btnDarDeBaja_click" Text="Dar de baja mi cuenta" CssClass="btn btn-outline-danger" runat="server"
                            OnClientClick="return confirm('¿Estas seguro que deseas dar de baja tu cuenta? Esta accion no se puede deshacer.');" />
                    </div>

                </div>
            </div>

        </div>
        <div class="tab-pane fade" id="datosSuscripcion" role="tabpanel" aria-labelledby="suscripcion-tab">
            <div class="row">
                <div class="col-md-4">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label class="form-label">Plan Actual</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="ddlPlan" Enabled="false"></asp:DropDownList>
                                    <asp:Button ID="btnCambiarPlan" OnClick="btnCambiarPlan_click" Text="Cambiar plan" CssClass="btn btn-outline-info" runat="server" />
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Vencimiento</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtVencimiento" TextMode="Date" ReadOnly="true" />
                                    <asp:Button ID="btnRenovarPlan" OnClick="btnRenovarPlan_click" Text="Renovar suscripcion" CssClass="btn btn-outline-secondary" runat="server" Enabled="false" />
                                </div>
                                <asp:Label Text="" ID="lblVencimiento" runat="server" ForeColor="LightBlue" />
                            </div>

                            <%if (TienePlanProximo) {%>
                            <div class="mb-3">
                                <label class="form-label">Proximo plan pagado</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtProximoPlan" ReadOnly="true" />
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Vencimiento</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtVencimientoProximo" TextMode="Date" ReadOnly="true" />
                                </div>
                            </div>
                            <% } %>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <div class="mb-3">
                        <asp:Button ID="btnCancelarSuscripcion" OnClick="btnCancelarSuscripcion_click" runat="server" Text="Cancelar Suscripcion" CssClass="btn btn-outline-danger" />
                    </div>
                </div>

            </div>
        </div>

    </div>
</asp:Content>
