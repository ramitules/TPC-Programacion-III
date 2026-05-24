<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Gimnasio_app.Usuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Usuarios</h2>
    <asp:GridView ID="gdvUsuario" runat="server" CssClass="table"></asp:GridView>
</asp:Content>
