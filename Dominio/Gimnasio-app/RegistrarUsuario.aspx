<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarUsuario.aspx.cs" Inherits="Gimnasio_app.RegistrarUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-5 mb-5">
    <div class="row justify-content-center">
        <div class="col-md-8 col-lg-6">

            <asp:Panel ID="pnlFormulario" runat="server">

            <div class="card shadow-sm border-0">

                <div class="card-header bg-primary text-white p-3">
                    <h4 class="mb-0 fs-5">
                        <i class="bi bi-person-plus-fill me-2"></i>
                        Crear mi cuenta
                    </h4>
                </div>

                <div class="card-body p-4">

                    <h6 class="text-primary fw-bold mb-3 text-uppercase border-bottom pb-2" style="font-size: 0.85rem;">Datos Personales</h6>

                    <div class="row g-3 mb-4">
                        <div class="col-sm-6">
                            <label for="txtNombre" class="form-label">Nombre</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Jose"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtApellido" class="form-label">Apellido</label>
                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Josefino"></asp:TextBox>
                        </div>
                        <div class="col-12">
                            <label for="txtFechaNacimiento" class="form-label">Fecha de nacimiento</label>
                            <asp:TextBox TextMode="Date" ID="txtFechaNacimiento" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <h6 class="text-primary fw-bold mb-3 text-uppercase border-bottom pb-2" style="font-size: 0.85rem;">Credenciales de Acceso</h6>

                    <div class="row g-3 mb-4">
                        <div class="col-12">
                            <label for="txtEmail" class="form-label">Correo Electronico</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="cliente@gimnasio.com"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtPassword" class="form-label">Contraseña</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label for="txtConfirmarPassword" class="form-label">Confirmar contraseña</label>
                            <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                        </div>
                    </div>

                    <div class="d-flex flex-column flex-sm-row justify-content-between align-items-center gap-2 mt-4 pt-3 border-top">

                        <a href="Login.aspx" class="text-decoration-none text-muted small order-2 order-sm-1">
                            ¿Ya tenes cuenta? Inicia sesión
                        </a>

                        <asp:Button ID="btnRegistrar" runat="server" Text="Crear cuenta" CssClass="btn btn-primary px-4 order-1 order-sm-2 shadow-sm" OnClick="btnRegistrar_Click" UseSubmitBehavior="false" OnClientClick="this.disabled = true; this.value = 'Creando...';" />

                    </div>

                </div>
            </div>

            </asp:Panel>

            <asp:Panel ID="pnlExito" runat="server" Visible="false">

            <div class="card shadow-sm border-0">
                <div class="card-body p-5 text-center">
                    <i class="bi bi-check-circle-fill text-success" style="font-size: 3rem;"></i>
                    <h4 class="mt-3 mb-2">¡Cuenta creada con éxito!</h4>
                    <p class="text-muted mb-4">Ya podés iniciar sesión con tu correo y contraseña.</p>
                    <a href="Login.aspx" class="btn btn-primary px-4 shadow-sm">
                        <i class="bi bi-box-arrow-in-right me-1"></i> Ir a iniciar sesión
                    </a>
                </div>
            </div>

            </asp:Panel>

        </div>
    </div>
</div>
</asp:Content>
