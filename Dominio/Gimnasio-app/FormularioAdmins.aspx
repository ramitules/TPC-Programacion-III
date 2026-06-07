<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioAdmins.aspx.cs" Inherits="Gimnasio_app.FormularioAdmins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-5 mb-5">
    <div class="row justify-content-center">
        <div class="col-md-8 col-lg-6">
            
            <div class="mb-3">
                <a href="Admins.aspx" class="btn btn-link text-decoration-none p-0 text-muted">
                    <i class="bi bi-arrow-left me-1"></i> Volver al listado
                </a>
            </div>

            <div class="card shadow-sm border-0">
                
                <div class="card-header bg-danger text-white p-3">
                    <h4 class="mb-0 fs-5">
                        <i class="bi bi-shield-lock-fill me-2"></i>
                        <asp:Literal ID="litTituloFormulario" runat="server" Text="Registrar Nuevo Administrador"></asp:Literal>
                    </h4>
                </div>

                <div class="card-body p-4">
                    
                    <asp:Panel ID="panelFeedback" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show" role="alert">
                        <asp:Label ID="lblMensajeFeedback" runat="server" Text="Operación realizada con éxito."></asp:Label>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </asp:Panel>

                    <h6 class="text-danger fw-bold mb-3 text-uppercase border-bottom pb-2" style="font-size: 0.85rem;">Datos Personales</h6>
                    
                    <div class="row g-3 mb-4">
                        <div class="col-sm-6">
                            <label for="txtNombre" class="form-label">Nombre</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej. Matías"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtApellido" class="form-label">Apellido</label>
                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ej. González"></asp:TextBox>
                        </div>
                        <div class="col-12">
                            <label for="txtDni" class="form-label">Documento (DNI)</label>
                            <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" placeholder="Sin puntos ni espacios"></asp:TextBox>
                        </div>
                    </div>

                    <h6 class="text-danger fw-bold mb-3 text-uppercase border-bottom pb-2" style="font-size: 0.85rem;">Credenciales de Sistema</h6>
                    
                    <div class="row g-3 mb-4">
                        <div class="col-12">
                            <label for="txtEmail" class="form-label">Correo Electrónico (Usuario)</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="admin@gimnasio.com"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtPassword" class="form-label">Contraseña</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="ddlEstadoAdmin" class="form-label">Estado de la Cuenta</label>
                            <asp:DropDownList ID="ddlEstadoAdmin" runat="server" CssClass="form-select">
                                <asp:ListItem Value="true" Selected="True">Activo (Acceso Permitido)</asp:ListItem>
                                <asp:ListItem Value="false">Inactivo (Acceso Bloqueado)</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="d-flex flex-column flex-sm-row justify-content-end gap-2 mt-4 pt-3 border-top">
                        
                        <asp:Button ID="btnEliminarLogico" runat="server" Text="Dar de Baja" 
                            CssClass="btn btn-outline-danger order-3 order-sm-1" 
                            Visible="false"
                            OnClientClick="return confirm('¿Está seguro de que desea deshabilitar esta cuenta de administrador?');" />
                        
                        <a href="Admins.aspx" class="btn btn-light order-2 text-muted">Cancelar</a>
                        
                        <asp:Button ID="btnGuardar" runat="server" Text="Registrar Alta" CssClass="btn btn-danger px-4 order-1 shadow-sm"/>
                            
                    </div>

                </div>
            </div>

        </div>
    </div>
</div>
</asp:Content>
