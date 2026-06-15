<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminControl.aspx.cs" Inherits="Gimnasio_app.AdminControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Admin Control</h1>

    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <h3 class="text-center mb-4 text-primary">Panel de Control General</h3>

                <ul class="list-group shadow-sm">

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-people-fill text-primary fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Clientes</h5>
                                <small class="text-muted">Gestionar altas, bajas, modificaciones y suscripciones.</small>
                            </div>
                        </div>
                        <a href="Clientes.aspx" class="btn btn-outline-primary btn-sm">Acceder</a>
                    </li>

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-person-badge text-success fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Entrenadores</h5>
                                <small class="text-muted">Administrar profesores, especialidades y horarios.</small>
                            </div>
                        </div>
                        <a href="Entrenadores.aspx" class="btn btn-outline-success btn-sm">Acceder</a>
                    </li>

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-person-workspace text-warning fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Recepcionistas</h5>
                                <small class="text-muted">Control de personal de atención y asignación de turnos.</small>
                            </div>
                        </div>
                        <a href="Recepcionistas.aspx" class="btn btn-outline-warning btn-sm">Acceder</a>
                    </li>

                    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center p-3">
                        <div class="d-flex align-items-center">
                            <i class="bi bi-shield-lock-fill text-danger fs-4 me-3"></i>
                            <div>
                                <h5 class="mb-0">Administradores (Admins)</h5>
                                <small class="text-muted">Configuración del sistema y permisos de cuentas staff.</small>
                            </div>
                        </div>
                        <a href="Admins.aspx" class="btn btn-outline-danger btn-sm">Acceder</a>
                    </li>

                </ul>
            </div>
        </div>
    </div>
</asp:Content>
