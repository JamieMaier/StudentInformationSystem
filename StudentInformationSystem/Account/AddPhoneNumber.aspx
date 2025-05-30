<%@ Page Title="Telefonnummer" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="AddPhoneNumber.aspx.vb" Inherits="StudentInformationSystem.AddPhoneNumber" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>

        <div>
            <h4>Telefonnummer hinzufügen</h4>
            <hr />
            <asp:ValidationSummary runat="server" CssClass="text-danger" />
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorMessage" />
            </p>
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="PhoneNumber" CssClass="col-md-2 col-form-label">Telefonnummer</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="PhoneNumber" CssClass="form-control" TextMode="Phone" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNumber"
                        CssClass="text-danger" ErrorMessage="Das Feld quot;PhoneNumber&quot; ist erforderlich." />
                </div>
            </div>
            <div class="row">
                <div class="offset-md-2 col-md-10">
                    <asp:Button runat="server" OnClick="PhoneNumber_Click"
                        Text="Absenden" CssClass="btn btn-outline-dark" />
                </div>
            </div>
        </div>
    </main>
</asp:Content>
