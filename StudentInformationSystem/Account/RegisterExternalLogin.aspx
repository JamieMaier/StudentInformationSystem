<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="RegisterExternalLogin.aspx.vb" Inherits="StudentInformationSystem.RegisterExternalLogin" Async="true" %>

<%@ Import Namespace="StudentInformationSystem" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main>
        <h3>Mit Ihrem <%: ProviderName %>-Konto registrieren</h3>
        <asp:PlaceHolder runat="server">
            <div>
                <h4>Zuordnungsformular</h4>
                <hr />
                <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                <p class="text-info">
                    Sie haben sich authentifiziert mit <strong><%: ProviderName %></strong>. Geben Sie unten eine E-Mail-Adresse für die aktuelle Website ein,
                    und klicken Sie dann auf die Schaltfläche "Anmelden".
                </p>

                <div class="row">
                    <asp:Label runat="server" AssociatedControlID="email" CssClass="col-md-2 col-form-label">E-Mail</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="email" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="email"
                            Display="Dynamic" CssClass="text-danger" ErrorMessage="Die E-Mail-Adresse ist erforderlich." />
                        <asp:ModelErrorMessage runat="server" ModelStateKey="email" CssClass="text-error" />
                    </div>
                </div>

                <div class="row">
                    <div class="offset-md-2 col-md-10">
                        <asp:Button runat="server" Text="Anmelden" CssClass="btn btn-outline-dark" OnClick="LogIn_Click" />
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
    </main>
</asp:Content>
