<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gimnasio_app.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Login</h1>
    <div class="mb-3">
        <label for="exampleInputEmail1" class="form-label">Email</label>
        <asp:TextBox ID="tbxEmail" CssClass="form-control" runat="server"></asp:TextBox>
        <div id="emailHelp" class="form-text"></div>
    </div>
    <div class="mb-3">
        <label for="exampleInputPassword1" class="form-label">Contraseña</label>
        <asp:TextBox ID="tbxPass" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false">Credenciales incorrectas</asp:Label>
    </div>


    <asp:Button ID="btnLogin" runat="server" Text="Entrar" CssClass="btn btn-primary" OnClick="btnLogin_Click" />

    <div class="mt-3">
        <a href="RecuperarContrasenia.aspx">¿Olvidaste tu contraseña?</a>
    </div>
</asp:Content>
