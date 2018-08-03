<%@ Page Title="<%$ Resources:WebResources, CheckoutConfirmTitle %>" Language="C#" MasterPageFile="~/Lyra.master" AutoEventWireup="true" CodeFile="CheckoutConfirm.aspx.cs" Inherits="CheckoutConfirm" %>
<%@ PreviousPageType VirtualPath="~/Checkout.aspx" %>

<asp:Content ContentPlaceHolderID="Head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <form id="CheckoutConfirmForm" runat="server">   
        <div class="column">
            <h2><asp:Localize runat="server" Text="<%$ Resources:WebResources, BuyerData %>" /></h2>

            <table>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, Civility %>" AssociatedControlID="Civility" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Civility" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, FirstName %>" AssociatedControlId="FirstName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="FirstName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, LastName %>" AssociatedControlId="LastName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LastName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, Address %>" AssociatedControlId="Address" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Address" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, ZipCode %>" AssociatedControlId="ZipCode" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="ZipCode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, City %>" AssociatedControlId="City" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="City" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, Country %>" AssociatedControlId="Country" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Country" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, Phone %>" AssociatedControlId="Phone" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Phone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, Email %>" AssociatedControlId="Email" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Email" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>

        <div class="column">
            <h2><asp:Localize runat="server" Text="<%$ Resources:WebResources, OrderData %>" /></h2>

            <table>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, OrderNumber %>" AssociatedControlId="OrderNumber" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="OrderNumber" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label Text="<%$ Resources:WebResources, Amount %>" AssociatedControlId="Amount" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Amount" runat="server"></asp:Label> €
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>
                        <img class="card-logo" alt="Visa"src="images/visa.png" />
                        <img class="card-logo" alt="CB" src="images/cb.png" />
                        <img class="card-logo" alt="MasterCard" src="images/mastercard.png" />
                        <img class="card-logo" alt="E-CarteBleue" src="images/e-cartebleue.png" />
                    </td>
                    <td>
                        <asp:Button ID="PayButton" onclick="PayButton_Click" Text="<%$ Resources:WebResources, Payment %>" runat="server" />
                    </td>
                </tr>
            </table>

            <asp:Localize runat="server" Text="<%$ Resources:WebResources, ConfirmDescription1 %>" /><br />
            <asp:Localize runat="server" Text="<%$ Resources:WebResources, ConfirmDescription2 %>" /><br />
            <asp:Localize runat="server" Text="<%$ Resources:WebResources, ConfirmDescription3 %>" /><br />
            <asp:Localize runat="server" Text="<%$ Resources:WebResources, ConfirmDescription4 %>" />


            <asp:Label ID="lblOutput" runat="server"></asp:Label>
        </div>    
    </form>
</asp:Content>