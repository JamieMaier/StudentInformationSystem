<%@ Page Title="Login" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="StudentInformationSystem.Login" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main>
        <%-- Page heading --%>
        <h2>Login</h2>

        <%-- Placeholder for login error messages --%>
        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>

        <%-- Username input field --%>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Email" CssClass="control-label">Username</asp:Label>
            <asp:TextBox runat="server" ID="Email" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                ErrorMessage="Username is required." CssClass="text-danger" />
        </div>

        <%-- Password input field --%>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">Password</asp:Label>
            <asp:TextBox runat="server" ID="Password" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                ErrorMessage="Password is required." CssClass="text-danger" />
        </div>

        <%-- Login button --%>
        <div class="form-group">
            <asp:Button runat="server" Text="Login" CssClass="btn btn-primary" OnClick="LogIn" />
        </div>

        <%-- Registration link for new users --%>
        <p>
            <asp:HyperLink runat="server" ID="RegisterHyperLink" NavigateUrl="~/Account/Register.aspx">
                Don't have an account? Register
            </asp:HyperLink>
        </p>
    </main>
</asp:Content>
