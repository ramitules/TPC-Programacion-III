<%@ Page Title="Crear Rutina" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelEntrenadorCrearRutina.aspx.cs" Inherits="Gimnasio_app.PanelEntrenadorCrearRutina" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Crear Rutina</h1>

    <div class="row mb-4">
        <div class="col-md-6">
            <label class="form-label">Nombre de la rutina</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Placeholder="Ej: Empuje/Tiro/Piernas" />
        </div>
    </div>

    <div class="card shadow-sm mb-4">
        <div class="card-body">
            <h5 class="card-title">Agregar Ejercicio</h5>
            <div class="row g-2 align-items-end">
                <div class="col-md-4">
                    <label class="form-label">Ejercicio</label>
                    <asp:DropDownList ID="ddlEjercicios" runat="server" CssClass="form-select" />
                </div>
                <div class="col-md-2">
                    <label class="form-label">Peso (kg)</label>
                    <asp:TextBox ID="txtPeso" runat="server" CssClass="form-control" 
                        TextMode="Number" Text="0" />
                </div>
                <div class="col-md-2">
                    <label class="form-label">Series</label>
                    <asp:TextBox ID="txtSeries" runat="server" CssClass="form-control" TextMode="Number" Text="3" />
                </div>
                <div class="col-md-2">
                    <label class="form-label">Repeticiones</label>
                    <asp:TextBox ID="txtReps" runat="server" CssClass="form-control" TextMode="Number" Text="10" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Agregar" OnClick="btnAgregar_Click" />
                </div>
            </div>
        </div>

    <div class="card shadow-sm mb-4">
        <div class="card-body">
            <h5 class="card-title">Ejercicios en la rutina</h5>
            <asp:GridView ID="gvEjercicios" runat="server" AutoGenerateColumns="false"
                CssClass="table table-hover align-middle mb-0" GridLines="None"
                ShowHeaderWhenEmpty="true" EmptyDataText="No hay ejercicios agregados.">
                <Columns>
                    <asp:BoundField DataField="OrdenEjercicio" HeaderText="#" />
                    <asp:BoundField DataField="Ejercicio.NombreEjercicio" HeaderText="Ejercicio" />
                    <asp:BoundField DataField="ObjetivoKG" HeaderText="Peso (kg)" />
                    <asp:TemplateField HeaderText="Series x Reps">
                        <ItemTemplate>
                            <%# Eval("ObjetivoSeries") %> x <%# Eval("ObjetivoRepeticiones") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnQuitar" runat="server" Text="Quitar" CssClass="btn btn-outline-danger btn-sm" OnClick="btnQuitar_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="text-end">
            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success px-4 py-2" Text="Guardar Rutina" OnClick="btnGuardar_Click" />
        </div>
    </div>
</div>

</asp:Content>
