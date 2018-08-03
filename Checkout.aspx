<%@ Page Title="<%$ Resources:WebResources, CheckoutTitle %>" Language="C#" MasterPageFile="~/Lyra.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Checkout" %>

<asp:Content ContentPlaceHolderID="Head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <asp:Label ID="ErrorMessage" CssClass="message" Visible="false" runat="server"></asp:Label>

    <form id="CheckoutForm" runat="server">
        <div class="column">
            <h2><asp:Localize runat="server" Text="<%$ Resources:WebResources, BuyerData %>" /></h2>

            <table>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="Civility" Text="<%$ Resources:WebResources, Civility %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="Civility" AutoPostBack="false" runat="server">
                            <asp:ListItem Selected="True" Value=""></asp:ListItem>
                            <asp:ListItem Value="<%$ Resources:WebResources, CivilityMr %>" Text="<%$ Resources:WebResources, CivilityMr %>" />
                            <asp:ListItem Value="<%$ Resources:WebResources, CivilityMrs %>" Text="<%$ Resources:WebResources, CivilityMrs %>" />
                            <asp:ListItem Value="<%$ Resources:WebResources, CivilityMs %>" Text="<%$ Resources:WebResources, CivilityMs %>" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="FirstName" Text="<%$ Resources:WebResources, FirstName %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="LastName" Text="<%$ Resources:WebResources, LastName %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="Address" Text="<%$ Resources:WebResources, Address %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Address" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="ZipCode" Text="<%$ Resources:WebResources, ZipCode %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ZipCode" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="City" Text="<%$ Resources:WebResources, City %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="City" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="Country" Text="<%$ Resources:WebResources, Country %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="Country" AutoPostBack="false" runat="server">
                            <asp:ListItem Selected="True" Value=""></asp:ListItem>
                            <asp:ListItem Value="FR" Text="<%$ Resources:WebResources, CountryFR %>" />
                            <asp:ListItem Value="DE" Text="<%$ Resources:WebResources, CountryDE %>" />
                            <asp:ListItem Value="ES" Text="<%$ Resources:WebResources, CountryES %>" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="Phone" Text="<%$ Resources:WebResources, Phone %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Phone" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="Email" Text="<%$ Resources:WebResources, Email %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

        <div class="column">
            <h2><asp:Localize runat="server" Text="<%$ Resources:WebResources, OrderData %>" /></h2>

            <table>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="OrderNumber" Text="<%$ Resources:WebResources, OrderNumber %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="OrderNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label AssociatedControlId="Amount" Text="<%$ Resources:WebResources, Amount %>" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Amount" runat="server">15</asp:TextBox> €<br />
                        <asp:CompareValidator Type="Double" ValueToCompare="0" ControlToValidate="Amount" Operator="GreaterThan" ErrorMessage="<%$ Resources:WebResources, InvalidField %>" runat="server" />
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td class="validate">
                        <asp:Button ID="ValidateButton" runat="server" PostBackUrl="~/CheckoutConfirm.aspx" Text="<%$ Resources:WebResources, ValidateOrder %>" />
                    </td>
                </tr>
                <tr>
                    <td class="validate">
                        <asp:Localize runat="server" Text="<%$ Resources:WebResources, ValidateDescription %>" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</asp:Content>