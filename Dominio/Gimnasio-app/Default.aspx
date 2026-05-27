<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gimnasio_app._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Panel de control Admin</h1>
            <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
            <p><a href="http://www.asp.net" class="btn btn-primary btn-md">Learn more &raquo;</a></p>
        </section>

        <div class="row">
            <h1>Clientes</h1>
            <asp:GridView ID="dgvCliente" runat="server" CssClass="table"></asp:GridView>
        </div>

        <div class="row">
            <h1>Profesores</h1>
            <asp:GridView ID="dgvProfesor" runat="server" CssClass="table"></asp:GridView>
        </div>

        <div class="row">
            <h1>Administrativos</h1>
            <asp:GridView ID="dgvAdministrador" runat="server" CssClass="table"></asp:GridView>
        </div>

        <div class="row">
            <h1>Admin</h1>
            <asp:GridView ID="dgvAdmin" runat="server" CssClass="table"></asp:GridView>
        </div>

    </main>

</asp:Content>
