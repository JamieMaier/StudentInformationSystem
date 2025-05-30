<%@ Page Title="Das Kennwort wurde geändert." Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPasswordConfirmation.aspx.vb" Inherits="StudentInformationSystem.ResetPasswordConfirmation" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <div>
            <p>Ihr Kennwort wurde geändert. Klicken Sie <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login">hier</asp:HyperLink> um sich anzumelden. </p>
        </div>
    </main>
</asp:Content>
