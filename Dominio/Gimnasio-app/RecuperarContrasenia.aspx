<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecuperarContrasenia.aspx.cs" Inherits="Gimnasio_app.RecuperarContrasenia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Recuperar contraseña</h1>

    <div>
        <asp:Label ID="lblInfo" runat="server" ForeColor="Green" Visible="false"></asp:Label>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    </div>

    <asp:Panel ID="pnlPedirCodigo" runat="server">
        <div class="mb-3">
            <label for="tbxEmail" class="form-label">Email</label>
            <asp:TextBox ID="tbxEmail" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="btnEnviarCodigo" runat="server" Text="Enviar código" CssClass="btn btn-primary" OnClick="btnEnviarCodigo_Click" />
    </asp:Panel>

    <asp:Panel ID="pnlRestablecer" runat="server" Visible="false">
        <div class="mb-3">
            <label for="tbxCodigo" class="form-label">Código recibido por email</label>
            <asp:TextBox ID="tbxCodigo" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label for="tbxPassNueva" class="form-label">Contraseña nueva</label>
            <asp:TextBox ID="tbxPassNueva" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label for="tbxPassNuevaConfirmar" class="form-label">Confirmar contraseña nueva</label>
            <asp:TextBox ID="tbxPassNuevaConfirmar" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="btnRestablecer" runat="server" Text="Restablecer contraseña" CssClass="btn btn-primary" OnClick="btnRestablecer_Click" />
    </asp:Panel>

    <div class="mt-3">
        <a href="Login.aspx">Volver al login</a>
    </div>
</asp:Content>
