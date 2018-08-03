<%@ Page Title="<%$ Resources:WebResources, PaymentResultTitle %>" Language="C#" MasterPageFile="~/Lyra.master" AutoEventWireup="true" CodeFile="PaymentResult.aspx.cs" Inherits="PaymentResult" %>

<asp:Content ContentPlaceHolderID="Head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <form id="PaymentResultForm" runat="server">
        <asp:Label ID="ResultMsg" CssClass="result" runat="server"></asp:Label>
        <br />

        <h2><asp:Localize runat="server" Text="<%$ Resources:WebResources, ResponseContent %>" /></h2>
        <div class="res-row">
            <div class="res-column"><asp:Label ID="ResultData1" runat="server"></asp:Label></div>
            <div class="res-column"><asp:Label ID="ResultData2" runat="server"></asp:Label></div>
            <div class="res-column"><asp:Label ID="ResultData3" runat="server"></asp:Label></div>
        </div>
        <asp:Label ID="Signature" runat="server"></asp:Label>
        <br />

        <h2><asp:Localize runat="server" Text="<%$ Resources:WebResources, SignatureProcessing %>" /></h2>
        <asp:TextBox ID="UnhashedSignature" ReadOnly="True" TextMode="MultiLine" CssClass="logger" runat="server"></asp:TextBox>
    </form>
</asp:Content>