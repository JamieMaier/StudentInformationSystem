﻿<%@ Page Title="" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManageLogins.aspx.vb" Inherits="StudentInformationSystem.ManageLogins" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title">Externe Anmeldungen verwalten.</h2>
        <asp:PlaceHolder runat="server" ID="SuccessMessagePlaceholder" Visible="false" ViewStateMode="Disabled">
                <p class="text-success"><%: SuccessMessage %></p>
            </asp:PlaceHolder>
        <div>
            <section id="externalLoginsForm">

                <asp:ListView runat="server"
                    ItemType="Microsoft.AspNet.Identity.UserLoginInfo"
                    SelectMethod="GetLogins" DeleteMethod="RemoveLogin" DataKeyNames="LoginProvider,ProviderKey">

                    <LayoutTemplate>
                        <h4>Registrierte Anmeldungen</h4>
                        <table class="table">
                            <tbody>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </tbody>
                        </table>

                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#: Item.LoginProvider %></td>
                            <td>
                                <asp:Button runat="server" Text="Entfernen" CommandName="Delete" CausesValidation="false"
                                    ToolTip='<%# "Dieses entfernen: " + Item.LoginProvider + " Anmeldung aus Ihrem Konto" %>'
                                    Visible="<%# CanRemoveExternalLogins %>" CssClass="btn btn-outline-dark" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>

            </section>
        </div>
        <div>
            <uc:OpenAuthProviders runat="server" ReturnUrl="~/Account/ManageLogins" />
        </div>
    </main>
</asp:Content>