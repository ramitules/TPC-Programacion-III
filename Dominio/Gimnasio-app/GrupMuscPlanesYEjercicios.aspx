<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GrupMuscPlanesYEjercicios.aspx.cs" Inherits="Gimnasio_app.GrupMuscPlanesYEjercicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="nav nav-tabs" id="myTab" role="tablist">
        <li class="nav-item"><a class="nav-link active" data-bs-toggle="tab" href="#ejercicios">Ejercicios</a></li>
        <li class="nav-item"><a class="nav-link" data-bs-toggle="tab" href="#musculos">Grupos Musculares</a></li>
        <li class="nav-item"><a class="nav-link" data-bs-toggle="tab" href="#planes">Planes</a></li>
    </ul>

    <div class="tab-content mt-3">
        <div class="tab-pane fade show active" id="ejercicios">
            <asp:Button ID="btnNuevoEjercicio" runat="server" Text="+ Nuevo Ejercicio" CssClass="btn btn-primary mb-2"  CommandArgument="Ejercicio" />
            <asp:GridView ID="dgvEjercicios" runat="server" CssClass="table table-dark table-hover" AutoGenerateColumns="false" >
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Ejercicio" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEditarEjer" runat="server" Text="✏️" CommandName="Editar" CommandArgument='<%# Eval("IdEjercicio") + "|Ejercicio" %>' CssClass="btn btn-sm btn-warning" />
                            <asp:Button ID="btnEliminarEjer" runat="server" Text="🗑️" CommandName="Eliminar" CommandArgument='<%# Eval("IdEjercicio") + "|Ejercicio" %>' CssClass="btn btn-sm btn-danger" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="tab-pane fade" id="musculos">
            <asp:Button ID="btnNuevoMusculo" runat="server" Text="+ Nuevo Grupo" CssClass="btn btn-primary mb-2"  CommandArgument="Musculo" />

            <asp:GridView ID="dgvMusculos" runat="server" CssClass="table table-dark table-hover" AutoGenerateColumns="false" >
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Grupo Muscular" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEditarMus" runat="server" Text="✏️" CommandName="Editar" CommandArgument='<%# Eval("IdGrupoMuscular") + "|Musculo" %>' CssClass="btn btn-sm btn-warning" />
                            <asp:Button ID="btnEliminarMus" runat="server" Text="🗑️" CommandName="Eliminar" CommandArgument='<%# Eval("IdGrupoMuscular") + "|Musculo" %>' CssClass="btn btn-sm btn-danger" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="tab-pane fade" id="planes">
            <asp:Button ID="btnNuevoPlan" runat="server" Text="+ Nuevo Plan" CssClass="btn btn-primary mb-2" CommandArgument="Plan" />

            <asp:GridView ID="dgvPlanes" runat="server" CssClass="table table-dark table-hover" AutoGenerateColumns="false" >
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Plan" />
                    <asp:BoundField DataField="PrecioPlan" HeaderText="Precio Mensual" DataFormatString="{0:C}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEditarPlan" runat="server" Text="✏️" CommandName="Editar" CommandArgument='<%# Eval("IdPlan") + "|Plan" %>' CssClass="btn btn-sm btn-warning" />
                            <asp:Button ID="btnEliminarPlan" runat="server" Text="🗑️" CommandName="Eliminar" CommandArgument='<%# Eval("IdPlan") + "|Plan" %>' CssClass="btn btn-sm btn-danger" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
