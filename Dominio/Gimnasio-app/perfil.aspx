<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="Gimnasio_app.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="space-y-6">
        <div>
            <h1 class="font-display text-3xl font-semibold tracking-tight">Mi perfil</h1>
            <p class="text-muted-foreground mt-1">Configuracion general de tu perfil</p>
        </div>

        <div>
            <ul class="nav nav-tabs" id="perfilTabs" role="tablist">
                <li class="nav-item" role="presentation">
                    <a class="nav-link active" id="personales-tab" data-bs-toggle="tab" href="#datosPersonales" role="tab" aria-controls="datosPersonales" aria-selected="true">Datos personales
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="suscripcion-tab" data-bs-toggle="tab" href="#datosSuscripcion" role="tab" aria-controls="datosSuscripcion" aria-selected="false">Suscripcion
                    </a>
                </li>
            </ul>
        </div>
    </div>

    <div class="tab-content mt-4" id="perfilTabsContent">

        <div class="tab-pane fade show active" id="datosPersonales" role="tabpanel" aria-labelledby="personales-tab">
            <div class="row">
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label">Nombre</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNombre" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Apellido</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtApellido" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Email</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Fecha de nacimiento</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNacimiento" TextMode="Date" ReadOnly="true" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label">Peso actual</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPeso" TextMode="Number" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Fecha de ingreso</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtIngreso" TextMode="Date" ReadOnly="true" />
                    </div>
                    <div class="mb-3">
                        <asp:Button ID="btnEditarDatos" OnClick="btnEditarDatos_click" Text="Editar" CssClass="btn btn-outline-info" runat="server" />
                        <%if (Editando)
                            { %>
                        <asp:Button ID="btnCancelarDatos" OnClick="btnCancelarDatos_click" Text="Cancelar" CssClass="btn btn-outline-warning" runat="server" />
                        <%} %>
                    </div>
                    <div class="mb-3 mt-4">
                        <asp:Button ID="btnDarDeBaja" OnClick="btnDarDeBaja_click" Text="Dar de baja mi cuenta" CssClass="btn btn-outline-danger" runat="server"
                            OnClientClick="return confirm('¿Estas seguro que deseas dar de baja tu cuenta? Esta accion no se puede deshacer.');" />
                    </div>

                </div>
            </div>

        </div>
        <div class="tab-pane fade" id="datosSuscripcion" role="tabpanel" aria-labelledby="suscripcion-tab">
            <div class="row">
                <div class="col-md-4">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label class="form-label">Plan Actual</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="ddlPlan" Enabled="false"></asp:DropDownList>
                                    <asp:Button ID="btnCambiarPlan" OnClick="btnCambiarPlan_click" Text="Cambiar plan" CssClass="btn btn-outline-info" runat="server" />
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Vencimiento</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtVencimiento" TextMode="Date" ReadOnly="true" />
                                    <asp:Button ID="btnRenovarPlan" OnClick="btnRenovarPlan_click" Text="Renovar suscripcion" CssClass="btn btn-outline-secondary" runat="server" Enabled="false" />
                                </div>
                                <asp:Label Text="" ID="lblVencimiento" runat="server" ForeColor="LightBlue" />
                            </div>

                            <%if (TienePlanProximo) {%>
                            <div class="mb-3">
                                <label class="form-label">Proximo plan pagado</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtProximoPlan" ReadOnly="true" />
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Vencimiento</label>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtVencimientoProximo" TextMode="Date" ReadOnly="true" />
                                </div>
                            </div>
                            <% } %>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <div class="mb-3">
                        <asp:Button ID="btnCancelarSuscripcion" OnClick="btnCancelarSuscripcion_click" runat="server" Text="Cancelar Suscripcion" CssClass="btn btn-outline-danger" />
                    </div>
                </div>

            </div>
        </div>

    </div>

    <%-- ====================== MODAL DE PAGO ======================
        Esta parte es simplemente un mock para simular una pasarela de pagos.
        No esta conectado a ningun proveedor de servicios de pago ni API.
        Solo corrobora que el formato sea el esperado y aprueba automatico,
        procesando la suscripcion como corresponde.--%>

    <asp:UpdatePanel ID="upPago" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfAccionPago" runat="server" />
            <div class="modal fade" id="modalPago" tabindex="-1" aria-labelledby="modalPagoLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalPagoLabel">Pago de suscripcion</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                        </div>
                        <div class="modal-body">
                            <div class="alert alert-secondary d-flex justify-content-between align-items-center py-2">
                                <span>Plan: <strong><asp:Label ID="lblPlanPago" runat="server" /></strong></span>
                                <span>Total: <strong><asp:Label ID="lblMontoPago" runat="server" /></strong></span>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Nombre en la tarjeta</label>
                                <asp:TextBox runat="server" ID="txtNombreTarjeta" CssClass="form-control" placeholder="Como figura en la tarjeta" />
                                <div class="invalid-feedback">Ingrese el nombre tal como figura en la tarjeta.</div>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Numero de tarjeta</label>
                                <asp:TextBox runat="server" ID="txtNumeroTarjeta" CssClass="form-control" placeholder="0000 0000 0000 0000"
                                    MaxLength="19" autocomplete="off" />
                                <div class="invalid-feedback">El numero debe tener 16 digitos.</div>
                            </div>

                            <div class="row">
                                <div class="col-6 mb-3">
                                    <label class="form-label">Vencimiento</label>
                                    <asp:TextBox runat="server" ID="txtVencimientoTarjeta" CssClass="form-control" placeholder="MM/AA"
                                        MaxLength="5" autocomplete="off" />
                                    <div class="invalid-feedback">Formato MM/AA. La tarjeta no debe estar vencida.</div>
                                </div>
                                <div class="col-6 mb-3">
                                    <label class="form-label">CVV</label>
                                    <asp:TextBox runat="server" ID="txtCvvTarjeta" CssClass="form-control" TextMode="Password" placeholder="123"
                                        MaxLength="4" autocomplete="off" />
                                    <div class="invalid-feedback">El CVV debe tener 3 o 4 digitos.</div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                            <asp:Button ID="btnConfirmarPago" runat="server" Text="Pagar" CssClass="btn btn-success"
                                OnClick="btnConfirmarPago_click" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        (function () {
            // Ids de cliente resueltos por ASP.NET (el modal vive dentro de un UpdatePanel)
            var ID = {
                numero: '<%= txtNumeroTarjeta.ClientID %>',
                vencimiento: '<%= txtVencimientoTarjeta.ClientID %>',
                cvv: '<%= txtCvvTarjeta.ClientID %>',
                nombre: '<%= txtNombreTarjeta.ClientID %>'
            };

            function soloDigitos(valor) { return (valor || '').replace(/\D/g, ''); }

            // Formateo en tiempo real
            function formatearNumero(e) {
                var d = soloDigitos(e.target.value).substring(0, 16);
                e.target.value = d.replace(/(.{4})/g, '$1 ').trim();
            }
            function formatearVencimiento(e) {
                var d = soloDigitos(e.target.value).substring(0, 4);
                e.target.value = d.length >= 3 ? d.substring(0, 2) + '/' + d.substring(2) : d;
            }
            function formatearCvv(e) {
                e.target.value = soloDigitos(e.target.value).substring(0, 4);
            }

            function engancharListeners() {
                var numero = document.getElementById(ID.numero);
                var vencimiento = document.getElementById(ID.vencimiento);
                var cvv = document.getElementById(ID.cvv);
                if (numero && !numero.dataset.bound) { numero.addEventListener('input', formatearNumero); numero.dataset.bound = '1'; }
                if (vencimiento && !vencimiento.dataset.bound) { vencimiento.addEventListener('input', formatearVencimiento); vencimiento.dataset.bound = '1'; }
                if (cvv && !cvv.dataset.bound) { cvv.addEventListener('input', formatearCvv); cvv.dataset.bound = '1'; }
            }

            // Evita backdrops grises colgados al re-renderizar el modal dentro del UpdatePanel.
            function sincronizarBackdrops() {
                var modal = document.getElementById('modalPago');
                var visible = modal && modal.classList.contains('show');
                var backdrops = document.querySelectorAll('.modal-backdrop');

                if (!visible) {
                    // El modal no esta abierto: limpiar cualquier backdrop residual.
                    backdrops.forEach(function (b) { b.remove(); });
                    document.body.classList.remove('modal-open');
                    document.body.style.removeProperty('overflow');
                    document.body.style.removeProperty('padding-right');
                } else {
                    // Modal abierto: dejar solo un backdrop.
                    for (var i = 0; i < backdrops.length - 1; i++) backdrops[i].remove();
                }
            }

            function alFinalizarPostback() {
                engancharListeners();
                sincronizarBackdrops();
            }

            engancharListeners();
            // Re-enganchar listeners y limpiar backdrops tras cada postback asincrono del UpdatePanel
            if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(alFinalizarPostback);
            }
        })();
    </script>
</asp:Content>
