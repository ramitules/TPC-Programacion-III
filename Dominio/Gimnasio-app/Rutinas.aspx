<%@ Page Title="Rutinas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rutinas.aspx.cs" Inherits="Gimnasio_app.Rutinas" %>

<%@ Import Namespace="Dominio" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .dia-row {
            display: flex;
            align-items: flex-start;
            gap: 1rem;
            padding: 0.75rem 0;
            border-bottom: 1px solid #dee2e6;
        }

            .dia-row:last-child {
                border-bottom: none;
            }

        .dia-label {
            min-width: 110px;
            font-weight: 600;
            color: #6c757d;
            padding-top: 0.4rem;
            font-size: 0.9rem;
        }

        .cards-scroll {
            display: flex;
            gap: 0.75rem;
            overflow-x: auto;
            padding-bottom: 0.5rem;
            flex: 1;
            min-height: 90px;
            align-items: flex-start;
        }

            .cards-scroll::-webkit-scrollbar {
                height: 4px;
            }

            .cards-scroll::-webkit-scrollbar-thumb {
                background: #dee2e6;
                border-radius: 2px;
            }

        .rutina-card {
            min-width: 175px;
            flex-shrink: 0;
        }

        .sin-rutinas {
            color: #adb5bd;
            font-size: 0.82rem;
            align-self: center;
        }
    </style>

    <div class="space-y-6">
        <div>
            <h1 class="font-display text-3xl font-semibold tracking-tight">Mis rutinas</h1>
            <p class="text-muted-foreground mt-1">Plantillas de entrenamiento personalizadas y generales para el dia a dia</p>
        </div>

        <div>
            <ul class="nav nav-tabs" id="rutinasTabs" role="tablist">
                <li class="nav-item" role="presentation">
                    <a class="nav-link active" id="propias-tab" data-bs-toggle="tab" href="#rutinasPropias" role="tab" aria-controls="rutinasPropias" aria-selected="true">Rutinas propias</a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="generales-tab" data-bs-toggle="tab" href="#rutinasGenerales" role="tab" aria-controls="rutinasGenerales" aria-selected="false">Rutinas generales</a>
                </li>
            </ul>
        </div>
    </div>

    <div class="tab-content mt-4" id="rutinasTabsContent">
        <%-- Tab: Rutinas propias --%>
        <div class="tab-pane fade show active" id="rutinasPropias" role="tabpanel" aria-labelledby="propias-tab">
            <asp:Repeater ID="rptPropias" runat="server">
                <ItemTemplate>
                    <div class="dia-row">
                        <span class="dia-label"><%# Eval("DiaDeRutina") %></span>
                        <div class="cards-scroll">
                            <asp:Repeater runat="server" DataSource='<%# Eval("Rutinas") %>'>
                                <ItemTemplate>
                                    <div class="card rutina-card">
                                        <div class="card-body">
                                            <h6 class="card-title mb-1"><%# Eval("Nombre") %></h6>
                                            <p class="card-text small text-muted mb-2">
                                                <%# Eval("FechaCreacion", "{0:dd/MM/yyyy}") %>
                                            </p>
                                            <a href="#" class="btn btn-sm btn-outline-secondary">Ver</a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:PlaceHolder runat="server" Visible='<%# !((DiaRutina)Container.DataItem).TieneRutinas %>'>
                                <span class="sin-rutinas">Sin rutinas</span>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%-- Tab: Rutinas generales. Hardcoded temporalmente para testing --%>
        <div class="tab-pane fade" id="rutinasGenerales" role="tabpanel" aria-labelledby="generales-tab">
            <asp:Repeater ID="rptGenerales" runat="server">
                <ItemTemplate>
                    <div class="dia-row">
                        <span class="dia-label"><%# Eval("DiaDeRutina") %></span>
                        <div class="cards-scroll">
                            <asp:Repeater runat="server" DataSource='<%# Eval("Rutinas") %>'>
                                <ItemTemplate>
                                    <div class="card rutina-card">
                                        <div class="card-body">
                                            <h6 class="card-title mb-1"><%# Eval("Nombre") %></h6>
                                            <p class="card-text small text-muted mb-2">
                                                <%# Eval("FechaCreacion", "{0:dd/MM/yyyy}") %>
                                            </p>
                                            <a href="#" class="btn btn-sm btn-outline-secondary">Ver</a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:PlaceHolder runat="server" Visible='<%# !((DiaRutina)Container.DataItem).TieneRutinas %>'>
                                <span class="sin-rutinas">Sin rutinas</span>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
