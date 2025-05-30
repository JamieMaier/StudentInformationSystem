<%@ Page Title="Kennwort vergessen" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forgot.aspx.vb" Inherits="StudentInformationSystem.ForgotPassword" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>

        <div class="row">
            <div class="col-md-8">
                <asp:PlaceHolder id="loginForm" runat="server">
                    <div>
                        <h4>Kennwort vergessen?</h4>
                        <hr />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="row">
                            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 col-form-label">E-Mail</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                    CssClass="text-danger" ErrorMessage="Das E-Mail-Feld ist erforderlich." />
                            </div>
                        </div>
                        <div class="row">
                            <div class="offset-md-2 col-md-10">
                                <asp:Button runat="server" OnClick="Forgot" Text="E-Mail-Link" CssClass="btn btn-outline-dark" />
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="DisplayEmail" Visible="false">
                    <p class="text-info">
                        Bitte überprüfen Sie Ihre E-Mail, um das Kennwort zurückzusetzen.
                    </p>
                </asp:PlaceHolder>
            </div>
        </div>
    </main>
</asp:Content>
