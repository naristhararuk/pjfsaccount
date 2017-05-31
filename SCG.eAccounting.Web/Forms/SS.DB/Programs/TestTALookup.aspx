<%@ Page Language="C#"  MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="TestTALookup.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SS.DB.Programs.TestTALookup" EnableTheming="true" StylesheetTheme="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

<asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <table width="600px">  
        <tr><td><asp:Label EnableViewState="false" ID="Label0" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label1" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label2" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label3" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label4" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label5" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label6" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label7" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label8" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr>
        <tr><td><asp:Label EnableViewState="false" ID="Label9" Text="abcdefghijklmnopqrstuvwxyz" runat="server"></asp:Label></td></tr> 
    </table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
