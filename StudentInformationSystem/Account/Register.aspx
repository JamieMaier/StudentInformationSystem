<%@ Page Title="Register" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.vb" Inherits="StudentInformationSystem.Register" %>

<%-- Injects content into the MainContent placeholder of Site.Master --%>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main>
        <%-- Page heading --%>
        <h2>Register</h2>

        <%-- Display error message from code-behind --%>
        <p class="text-danger">
            <asp:Literal runat="server" ID="ErrorMessage" />
        </p>

        <%-- Validation summary for all validation errors --%>
        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <%-- Username (email) field --%>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Email" CssClass="control-label">Username (Email)</asp:Label>
            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                CssClass="text-danger" ErrorMessage="Username is required." />
        </div>

        <%-- Password field --%>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">Password</asp:Label>
            <asp:TextBox runat="server" ID="Password" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                CssClass="text-danger" ErrorMessage="Password is required." />
        </div>

        <%-- Confirm password field --%>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="control-label">Confirm Password</asp:Label>
            <asp:TextBox runat="server" ID="ConfirmPassword" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                CssClass="text-danger" ErrorMessage="Confirmation is required." />
            <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                CssClass="text-danger" ErrorMessage="Passwords do not match." />
        </div>

        <%-- Role selection dropdown --%>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Role" CssClass="control-label">Role</asp:Label>
            <asp:DropDownList runat="server" ID="Role" CssClass="form-control">
                <asp:ListItem Text="User" Value="user" />
                <asp:ListItem Text="Admin" Value="admin" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Role"
                CssClass="text-danger" InitialValue="" ErrorMessage="Role is required." />
        </div>

        <%-- Submit button --%>
        <div class="form-group">
            <asp:Button runat="server" Text="Register" CssClass="btn btn-primary" OnClick="CreateUser_Click" />
        </div>
    </main>
</asp:Content>
